using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AlienCyborgSynergyNetwork
{
    /// <summary>
    /// Provides methods to hash passwords and verify them later
    /// </summary>
    public class PasswordHasher
    {
        // Size of random salt
        private const int SaltSize = 16;

        // Size of the derived key
        private const int KeySize = 32;

        // Number of iterations for PBKDF2
        private const int Iterations = 10000;

        /// <summary>
        /// Hashes a password using PBKDF2 with a random salt.
        /// </summary>
        /// <param name="password">the plaintext password to hash</param>
        /// <returns>a composite string with iterations, salt and key</returns>
        public static string Hash(string password)
        {
            // Random number gen
            using var rng = RandomNumberGenerator.Create();

            // Allocate a byte array for the salt
            var salt = new byte[SaltSize];

            // Fill the salt with random bytes
            rng.GetBytes(salt);

            // Create a PBKDF2 instance with the password, salt, iterations and hash algorithm
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);

            // Generate the key
            var key = pbkdf2.GetBytes(KeySize);

            // Return a composite string containing the iterations, salt and key
            return $"{Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(key)}";
        }

        /// <summary>
        /// Verifies a password against a previously hashed password.
        /// </summary>
        /// <param name="hash">The stored hash string</param>
        /// <param name="password">The plaintext password</param>
        /// <returns>True if the password matches the has</returns>
        public static bool Verify(string hash, string password)
        {
            // Split the hash into its components
            var parts = hash.Split(':', 3);

            // Parse iteration on first part
            var iterations = int.Parse(parts[0]);

            // Decode salt from base 64
            var salt = Convert.FromBase64String(parts[1]);

            // Decode key from base 64
            var key = Convert.FromBase64String(parts[2]);

            // Create a new PBKDF2 instance with the same parameters
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);

            // Generate the key using the same parameters
            var computedKey = pbkdf2.GetBytes(KeySize);

            // Use a constant time comparison to prevent timing attacks
            return CryptographicOperations.FixedTimeEquals(key, computedKey);
        }
    }
}
