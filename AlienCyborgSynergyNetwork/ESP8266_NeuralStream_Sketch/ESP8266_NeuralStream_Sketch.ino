#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <DHT.h>
#include <ESP8266HTTPClient.h>
#include <ESP8266httpUpdate.h>

// Ultrasonic requires 5v which isn't available on the ESP8266 12e
// Wi-Fi & MQTT
const char* SSID = "YourSSID";
const char* PASS = "YourPassword";
const char* MQTT_SERVER = "192.168.1.42";
const char* firmware_url = "http://yourserver.com/firmware/firmware.bin";

// Pins
#define DHT_PIN   D2
#define TRIG_PIN  D5
#define ECHO_PIN  D6
#define DHT_TYPE  DHT11

DHT dht(DHT_PIN, DHT_TYPE);
WiFiClient wifiClient;
PubSubClient mqtt(wifiClient);

unsigned long lastBioLogTime = 0;
const unsigned long BIO_INTERVAL = 60UL * 1000UL;  // 1 minute
const unsigned long NEURAL_INTERVAL = 1000;        // 1 second
unsigned long lastNeuralTime = 0;

void ensureMqtt() {
  if (!mqtt.connected()) {
    while (!mqtt.connect("ESP8266Client")) {
      delay(2000);
    }
  }
}

void setup() {
  Serial.begin(115200);
  dht.begin();
  pinMode(TRIG_PIN, OUTPUT);
  pinMode(ECHO_PIN, INPUT);

  WiFi.begin(SSID, PASS);
  while (WiFi.status() != WL_CONNECTED) delay(500);

  mqtt.setServer(MQTT_SERVER, 1883);

  t_httpUpdate_return ret = ESPhttpUpdate.update(wifiClient, String(firmware_url));

  switch(ret){
    case HTTP_UPDATE_FAILED:
      Serial.printf("HTTP update failed: %s\n", ESPhttpUpdate.getLastErrorString().c_str());
      break;
    case HTTP_UPDATE_NO_UPDATES:
      Serial.println("No updates available.");
      break;
    case HTTP_UPDATE_OK:
      Serial.println("Update successful!");
      break;
  }
}

float getDistanceCM() {
  digitalWrite(TRIG_PIN, LOW);
  delayMicroseconds(2);
  digitalWrite(TRIG_PIN, HIGH);
  delayMicroseconds(10);
  digitalWrite(TRIG_PIN, LOW);
  long duration = pulseIn(ECHO_PIN, HIGH, 30000);  // 30 ms max
  float distance = duration * 0.034 / 2;
  return distance;
}

void loop() {
  ensureMqtt();
  mqtt.loop();

  unsigned long now = millis();

  // --- NEURAL STREAM ---
  if (now - lastNeuralTime >= NEURAL_INTERVAL) {
    lastNeuralTime = now;
    float dist = getDistanceCM();
    if (dist > 0 && dist < 400) {
      char buf[32];
      snprintf(buf, sizeof(buf), "%.1f", dist);
      mqtt.publish("esp8266/neuralstream", buf);
    }
  }

  // --- BIOLOG STREAM ---
  if (now - lastBioLogTime >= BIO_INTERVAL) {
    lastBioLogTime = now;
    float h = dht.readHumidity();
    float t = dht.readTemperature();
    if (!isnan(h) && !isnan(t)) {
      char buf[32];
      snprintf(buf, sizeof(buf), "%.1f,%.1f", t, h);
      mqtt.publish("esp8266/biolog", buf);
    }
  }
}