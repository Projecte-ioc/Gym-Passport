using System.Security.Cryptography;
using System.Text;

namespace GymPassport.GymPassportAPI.Helpers
{
    public static class AesGcmUtils
    {
        public static string EncryptWithAESGCM(string plaintext, string aesKey)
        {
            // Generar un nonce aleatorio (12 bytes para AES-GCM)
            byte[] nonce = GenerateRandomNonce();

            // Tu clave secreta (reemplazar con tu clave real)
            byte[] keyBytes = Base64UrlDecode(aesKey);

            // Convertir la cadena de texto plano a bytes
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

            // Inicializar el cifrado AES-GCM
            using (AesGcm aesGcm = new AesGcm(keyBytes))
            {
                // Crear un búfer para contener el texto cifrado y la etiqueta de autenticación
                byte[] ciphertext = new byte[plaintextBytes.Length];
                byte[] tag = new byte[16]; // 128 bits

                // Cifrar el texto plano
                aesGcm.Encrypt(nonce, plaintextBytes, ciphertext, tag);

                // Concatenar el nonce con el texto cifrado y la etiqueta
                byte[] encryptedResult = new byte[nonce.Length + ciphertext.Length + tag.Length];
                Buffer.BlockCopy(nonce, 0, encryptedResult, 0, nonce.Length);
                Buffer.BlockCopy(ciphertext, 0, encryptedResult, nonce.Length, ciphertext.Length);
                Buffer.BlockCopy(tag, 0, encryptedResult, nonce.Length + ciphertext.Length, tag.Length);

                // Convertir el resultado a Base64 para una representación más sencilla
                return Convert.ToBase64String(encryptedResult);
            }
        }

        public static string DecryptWithAESGCM(string encryptedText, string aesKey)
        {
            // Convertir el texto cifrado codificado en base64 a bytes
            byte[] encryptedBytes = Base64UrlDecode(encryptedText);

            // Extraer el nonce de los bytes cifrados
            byte[] nonce = new byte[12];
            Buffer.BlockCopy(encryptedBytes, 0, nonce, 0, nonce.Length);

            // Tu clave secreta (reemplazar con tu clave real)
            byte[] keyBytes = Base64UrlDecode(aesKey);

            // Extraer el texto cifrado de los bytes cifrados
            byte[] ciphertext = new byte[encryptedBytes.Length - nonce.Length - 16]; // 16 es el tamaño de la etiqueta
            Buffer.BlockCopy(encryptedBytes, nonce.Length, ciphertext, 0, ciphertext.Length);

            // Extraer la etiqueta de los bytes cifrados
            byte[] tag = new byte[16];
            Buffer.BlockCopy(encryptedBytes, nonce.Length + ciphertext.Length, tag, 0, tag.Length);

            // Inicializar el cifrado AES-GCM
            using (AesGcm aesGcm = new AesGcm(keyBytes))
            {
                // Crear un búfer para contener el texto plano descifrado
                byte[] decryptedBytes = new byte[ciphertext.Length];

                // Descifrar el texto cifrado
                aesGcm.Decrypt(nonce, ciphertext, tag, decryptedBytes);

                // Convertir los bytes descifrados a una cadena de texto
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        public static byte[] Base64UrlDecode(string base64UrlEncoded)
        {
            // Reemplazar caracteres específicos de base64url y completar la cadena si es necesario
            string base64 = base64UrlEncoded.Replace('-', '+').Replace('_', '/');

            // Asegurarse de que la longitud sea un múltiplo de 4 (agregar relleno si es necesario)
            int padding = (4 - base64.Length % 4) % 4;
            base64 += new string('=', padding);

            // Decode the base64 string
            return Convert.FromBase64String(base64);
        }

        public static byte[] GenerateRandomNonce()
        {
            // Generar un nonce aleatorio (12 bytes para AES-GCM)
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] nonce = new byte[12];
                rng.GetBytes(nonce);
                return nonce;
            }
        }
    }
}
