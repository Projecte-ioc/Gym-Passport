using Jose;
using Newtonsoft.Json.Linq;
using System.Text;

namespace GymPassport.GymPassportAPI.Helpers
{
    /// <summary>
    /// Clase de utilidades para la manipulación de tokens JWT (JSON Web Tokens) en operaciones con la API.
    /// </summary>
    public static class ApiUtils
    {
        /// <summary>
        /// Codifica y encripta un payload como un token JWT firmado y encriptado.
        /// </summary>
        /// <param name="payload">Objeto JSON que representa el contenido del payload del token.</param>
        /// <param name="secretKey">Clave secreta utilizada para firmar y encriptar el token.</param>
        /// <param name="nonceSize">Tamaño del nonce utilizado en el cifrado AES-GCM.</param>
        /// <param name="tagSize">Tamaño de la etiqueta utilizado en el cifrado AES-GCM.</param>
        /// <returns>Una instancia de <see cref="StringContent"/> que contiene el token JWT firmado y encriptado.</returns>
        public static StringContent EncodeAndEncryptJwt(JObject payload, string secretKey, int nonceSize, int tagSize)
        {
            // Codifica el payload como un token JWT firmado usando la librería JOSE
            string signedJwt = JWT.Encode(payload.ToString(), Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS256);

            // Encripta el JWT firmado con AES-GCM
            string signedAndEncryptedJwt = AesGcmUtils.Encrypt(signedJwt, secretKey, nonceSize, tagSize);

            // Crea un nuevo JObject con "jwe" como campo y el token encriptado como su valor
            JObject signedAndEncryptedJwtAsJObject = new JObject { { "jwe", signedAndEncryptedJwt } };

            // Crea el contenido que debe ser enviado en la petición HTTP
            StringContent content = new StringContent(signedAndEncryptedJwtAsJObject.ToString(), Encoding.UTF8, "application/json");

            // Devuelve el JWT firmado y encriptado
            return content;
        }

        /// <summary>
        /// Desencripta y decodifica un token JWT recibido como respuesta de la API.
        /// </summary>
        /// <param name="responseContent">Contenido de la respuesta HTTP que contiene el token JWT encriptado.</param>
        /// <param name="secretKey">Clave secreta utilizada para desencriptar y decodificar el token.</param>
        /// <param name="nonceSize">Tamaño del nonce utilizado en el cifrado AES-GCM.</param>
        /// <param name="tagSize">Tamaño de la etiqueta utilizado en el cifrado AES-GCM.</param>
        /// <returns>El contenido decodificado del token JWT.</returns>
        public static string DecryptAndDecodeJwt(string responseContent, string secretKey, int nonceSize, int tagSize)
        {
            // Devuelve la respuesta en formato JObject
            JObject jObjectResponseContent = JObject.Parse(responseContent);

            // Almacena el JWT encriptado recibido
            string encryptedResponse = jObjectResponseContent["jwe"]!.ToString();

            // Desencripta el JWT recibido
            string decryptedResponse = AesGcmUtils.Decrypt(encryptedResponse, secretKey, nonceSize, tagSize);

            // Decodificar el JWT recibido
            return JWT.Decode(decryptedResponse, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS256);
        }
    }
}
