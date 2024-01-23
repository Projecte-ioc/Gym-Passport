using System.Security.Cryptography;
using System.Text;

namespace GymPassport.GymPassportAPI.Helpers
{
    public static class AesGcmUtils
    {
        public static string EncryptWithAESGCM(string plaintext, string aesKey)
        {
            // Generate a random nonce (12 bytes for AES-GCM)
            byte[] nonce = GenerateRandomNonce();

            // Your secret key (replace with your actual key)
            byte[] keyBytes = Base64UrlDecode(aesKey);

            // Convert the plaintext string to bytes
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

            // Initialize AES-GCM cipher
            using (AesGcm aesGcm = new AesGcm(keyBytes))
            {
                // Create a buffer to hold the ciphertext and authentication tag
                byte[] ciphertext = new byte[plaintextBytes.Length];
                byte[] tag = new byte[16]; // 128 bits

                // Encrypt the plaintext
                aesGcm.Encrypt(nonce, plaintextBytes, ciphertext, tag);

                // Concatenate the nonce with the ciphertext and tag
                byte[] encryptedResult = new byte[nonce.Length + ciphertext.Length + tag.Length];
                Buffer.BlockCopy(nonce, 0, encryptedResult, 0, nonce.Length);
                Buffer.BlockCopy(ciphertext, 0, encryptedResult, nonce.Length, ciphertext.Length);
                Buffer.BlockCopy(tag, 0, encryptedResult, nonce.Length + ciphertext.Length, tag.Length);

                // Convert the result to Base64 for easier representation
                return Convert.ToBase64String(encryptedResult);
            }
        }

        public static string DecryptWithAESGCM(string encryptedText, string aesKey)
        {
            // Convert the base64-encoded encrypted text to bytes
            byte[] encryptedBytes = Base64UrlDecode(encryptedText);

            // Extract the nonce from the encrypted bytes
            byte[] nonce = new byte[12];
            Buffer.BlockCopy(encryptedBytes, 0, nonce, 0, nonce.Length);

            // Your secret key (replace with your actual key)
            byte[] keyBytes = Base64UrlDecode(aesKey);

            // Extract the ciphertext from the encrypted bytes
            byte[] ciphertext = new byte[encryptedBytes.Length - nonce.Length - 16]; // 16 is the size of the tag
            Buffer.BlockCopy(encryptedBytes, nonce.Length, ciphertext, 0, ciphertext.Length);

            // Extract the tag from the encrypted bytes
            byte[] tag = new byte[16];
            Buffer.BlockCopy(encryptedBytes, nonce.Length + ciphertext.Length, tag, 0, tag.Length);

            // Initialize AES-GCM cipher
            using (AesGcm aesGcm = new AesGcm(keyBytes))
            {
                // Create a buffer to hold the decrypted plaintext
                byte[] decryptedBytes = new byte[ciphertext.Length];

                // Decrypt the ciphertext
                aesGcm.Decrypt(nonce, ciphertext, tag, decryptedBytes);

                // Convert the decrypted bytes to a string
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        public static byte[] Base64UrlDecode(string base64UrlEncoded)
        {
            // Replace base64url-specific characters and pad the string
            string base64 = base64UrlEncoded.Replace('-', '+').Replace('_', '/');

            // Ensure that the length is a multiple of 4 (add padding if needed)
            int padding = (4 - base64.Length % 4) % 4;
            base64 += new string('=', padding);

            // Decode the base64 string
            return Convert.FromBase64String(base64);
        }

        public static byte[] GenerateRandomNonce()
        {
            // Generate a random nonce (12 bytes for AES-GCM)
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] nonce = new byte[12];
                rng.GetBytes(nonce);
                return nonce;
            }
        }
    }
}
