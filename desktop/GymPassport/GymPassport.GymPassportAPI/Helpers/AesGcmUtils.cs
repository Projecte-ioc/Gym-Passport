using System.Security.Cryptography;
using System.Text;

namespace GymPassport.GymPassportAPI.Helpers
{
    /// <summary>
    /// Clase de utilidad para realizar operaciones de cifrado y descifrado utilizando el algoritmo AES-GCM.
    /// </summary>
    public static class AesGcmUtils
    {
        /// <summary>
        /// Encripta un array de bytes utilizando el algoritmo de cifrado simétrico AES-GCM.
        /// </summary>
        /// <param name="data">Array de bytes que se va a cifrar.</param>
        /// <param name="key">Clave secreta utilizada para el cifrado.</param>
        /// <param name="nonceSize">Tamaño del nonce (IV) que se va a utilizar en GCM (en bytes).</param>
        /// <param name="tagSize">Tamaño de la etiqueta de autenticación que se va a utilizar (en bytes).</param>
        /// <returns>Un array de bytes que representa el resultado cifrado, incluyendo el nonce y el tag.</returns>
        public static byte[] Encrypt(byte[] data, byte[] key, int nonceSize, int tagSize)
        {
            // Crea una instancia de AesGcm utilizando la clave proporcionada y los tamaños de etiqueta y nonce especificados
            using AesGcm aes = new AesGcm(key, tagSize);

            // Genera un nonce aleatorio con el tamaño especificado. El nonce es único para cada cifrado y no debe repetirse.
            var nonce = GenerateRandomNonce(nonceSize);

            // Crea un array para almacenar el tag resultante del cifrado
            var tag = new byte[tagSize];

            // Crea un array para almacenar el texto cifrado resultante del cifrado
            var cipher = new byte[data.Length];

            // Realiza el cifrado utilizando AesGcm
            aes.Encrypt(nonce, data, cipher, tag);

            // Combina el nonce, el texto cifrado y el tag en un solo array de bytes
            List<byte> result = [.. nonce, .. cipher, .. tag];

            // Convierte la lista a un array de bytes y devuelve el resultado cifrado
            return result.ToArray();
        }

        /// <summary>
        /// Encripta una cadena utilizando el algoritmo de cifrado simétrico AES-GCM.
        /// </summary>
        /// <param name="data">Cadena que se va a cifrar.</param>
        /// <param name="key">Clave secreta utilizada para el cifrado (base64url codificada).</param>
        /// <param name="nonceSize">Tamaño del nonce (IV) que se va a utilizar en GCM (en bytes).</param>
        /// <param name="tagSize">Tamaño de la etiqueta de autenticación que se va a utilizar (en bytes).</param>
        /// <returns>Una cadena que representa el resultado cifrado en base64.</returns>
        public static string Encrypt(string data, string key, int nonceSize, int tagSize)
        {
            // Decodifica la clave base64url a un array de bytes
            byte[] keyBytes = Base64UrlDecode(key);

            // Convierte la cadena de entrada a un array de bytes utilizando UTF-8
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            // Encripta los bytes y convierte el resultado a una cadena en formato base64
            return Convert.ToBase64String(Encrypt(dataBytes, keyBytes, nonceSize, tagSize));
        }

        /// <summary>
        /// Desencripta un array de bytes cifrado utilizando el algoritmo AES-GCM.
        /// </summary>
        /// <param name="data">Array de bytes cifrado que se va a desencriptar.</param>
        /// <param name="key">Clave secreta utilizada para el desencriptado.</param>
        /// <param name="nonceSize">Tamaño del nonce (IV) utilizado en GCM (en bytes).</param>
        /// <param name="tagSize">Tamaño de la etiqueta de autenticación utilizada (en bytes).</param>
        /// <returns>Un array de bytes que representa el resultado desencriptado.</returns>
        public static byte[] Decrypt(byte[] data, byte[] key, int nonceSize, int tagSize)
        {
            // Crea una instancia de AesGcm utilizando la clave proporcionada y los tamaños de etiqueta y nonce especificados
            using AesGcm aes = new AesGcm(key, tagSize);

            // Extrae el nonce, el texto cifrado y el tag del array de bytes cifrado
            byte[] nonce = data[..nonceSize];
            byte[] cipher = data[nonceSize..^tagSize];
            byte[] tag = data[^tagSize..];

            // Crea un array para almacenar el resultado desencriptado
            byte[] result = new byte[data.Length - (nonceSize + tagSize)];

            // Desencripta los bytes utilizando AesGcm
            aes.Decrypt(nonce, cipher, tag, result);

            // Devuelve el resultado desencriptado
            return result;
        }

        /// <summary>
        /// Desencripta una cadena cifrada utilizando el algoritmo AES-GCM.
        /// </summary>
        /// <param name="data">Cadena cifrada en formato base64 que se va a desencriptar.</param>
        /// <param name="key">Clave secreta utilizada para el desencriptado (base64url codificada).</param>
        /// <param name="nonceSize">Tamaño del nonce (IV) utilizado en GCM (en bytes).</param>
        /// <param name="tagSize">Tamaño de la etiqueta de autenticación utilizada (en bytes).</param>
        /// <returns>Una cadena que representa el resultado desencriptado.</returns>
        public static string Decrypt(string data, string key, int nonceSize, int tagSize)
        {
            // Decodifica la clave base64url a un array de bytes
            byte[] keyBytes = Base64UrlDecode(key);

            // Decodifica la cadena cifrada en base64url a un array de bytes
            byte[] dataBytes = Base64UrlDecode(data);

            // Desencripta los bytes y convierte el resultado a una cadena utilizando UTF-8
            return Encoding.UTF8.GetString(Decrypt(dataBytes, keyBytes, nonceSize, tagSize));
        }

        /// <summary>
        /// Decodifica una cadena codificada en base64url a su representación de bytes.
        /// </summary>
        /// <param name="base64UrlEncoded">Cadena codificada en base64url.</param>
        /// <returns>Un array de bytes que representa la decodificación de la cadena en base64url.</returns>
        /// <exception cref="ArgumentException">Se lanza si la cadena no es válida en base64url.</exception>
        private static byte[] Base64UrlDecode(string base64UrlEncoded)
        {
            // Valida que la cadena de entrada no sea nula ni de longitud cero.
            if (string.IsNullOrEmpty(base64UrlEncoded))
                throw new ArgumentException("La cadena de entrada no puede ser nula ni de longitud cero.", nameof(base64UrlEncoded));

            // Reemplaza caracteres específicos de base64url y completa la cadena con relleno.
            string base64Encoded = base64UrlEncoded.Replace('-', '+').Replace('_', '/');

            // Se asegura de que la longitud sea un múltiplo de 4 (agrega relleno si es necesario).
            int padding = (4 - base64Encoded.Length % 4) % 4;
            base64Encoded += new string('=', padding);

            // Decodifica la cadena base64.
            try
            {
                return Convert.FromBase64String(base64Encoded);
            }
            catch (FormatException ex)
            {
                // Manejar la excepción específica para cadenas base64url mal formadas.
                throw new ArgumentException("La cadena no es una cadena válida en base64url.", nameof(base64UrlEncoded), ex);
            }
        }

        /// <summary>
        /// Genera un nonce aleatorio utilizando un generador de números aleatorios seguro.
        /// </summary>
        /// <param name="size">Tamaño del nonce que se va a generar. Debe ser mayor que cero.</param>
        /// <returns>Un array de bytes que representa el nonce aleatorio generado.</returns>
        /// <exception cref="ArgumentException">Se lanza si el tamaño especificado no es válido (menor o igual a cero).</exception>
        /// <exception cref="InvalidOperationException">Se lanza si ocurre un error inesperado durante la generación de bytes aleatorios.</exception>
        private static byte[] GenerateRandomNonce(int size)
        {
            // Valida que el tamaño del nonce sea válido.
            if (size <= 0)
                throw new ArgumentException("El tamaño del nonce debe ser mayor que cero.", nameof(size));

            // Crea una instancia de RandomNumberGenerator para generar bytes aleatorios de manera segura.
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                // Crea un array de bytes para almacenar el nonce con el tamaño especificado.
                byte[] nonce = new byte[size];

                try
                {
                    // Llena el array de nonce con bytes aleatorios generados por el generador de números aleatorios.
                    rng.GetBytes(nonce);

                    // Devuelve el nonce generado.
                    return nonce;
                }
                catch (Exception ex)
                {
                    // Maneja cualquier excepción inesperada durante la generación de bytes.
                    // Puede loggearse, relanzarse, o manejar de otra manera según las necesidades.
                    throw new InvalidOperationException("Error al generar el nonce aleatorio.", ex);
                }
            }
        }
    }
}