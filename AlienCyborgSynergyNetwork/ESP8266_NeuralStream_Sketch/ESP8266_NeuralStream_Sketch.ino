#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <DHT.h>

// —— Configuration ——
const char* SSID        = "Your_WiFi_SSID";
const char* PASS        = "Your_WiFi_Password";
const char* MQTT_SERVER = "192.168.1.42";
const uint8_t DHT_PIN   = D2;
const uint8_t LED_PIN   = D1;
const unsigned long INTERVAL_MS = 60UL * 1000UL;

// —— Globals ——
DHT dht(DHT_PIN, DHT11);
WiFiClient  wifiClient;
PubSubClient mqtt(wifiClient);
unsigned long lastSend = 0;

// —— Helpers ——
void ensureMqtt() {
  if (mqtt.connected()) return;
  while (!mqtt.connect("ESP8266Client")) {
    delay(5000);
  }
}

void setup() {
  Serial.begin(115200);
  pinMode(LED_PIN, OUTPUT);
  dht.begin();
  WiFi.begin(SSID, PASS);
  while (WiFi.status() != WL_CONNECTED) delay(500);
  mqtt.setServer(MQTT_SERVER, 1883);
}

void loop() {
  ensureMqtt();
  mqtt.loop();

  unsigned long now = millis();
  if (now - lastSend >= INTERVAL_MS) {
    lastSend = now;
    float h = dht.readHumidity();
    float t = dht.readTemperature();
    if (!isnan(h) && !isnan(t)) {
      char buf[32];
      snprintf(buf, sizeof(buf), "%.1f,%.1f", t, h);
      mqtt.publish("esp8266/dht", buf);
      digitalWrite(LED_PIN, HIGH);
      delay(100);
      digitalWrite(LED_PIN, LOW);
    }
  }
}