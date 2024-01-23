using Newtonsoft.Json.Linq;

namespace GymPassport.GymPassportAPI.Helpers
{
    public static class ApiUtils
    {
        public static string CreateSignedAndEncryptedJwt(JObject jObject, string secretKey)
        {
            // Convierte el JObject en un JWT firmado
            string signedJwt = JwtUtils.ConvertToSignedJWT(jObject, secretKey);

            // Encripta el JWT firmado con AES-GCM
            string signedAndEncryptedJwt = AesGcmUtils.EncryptWithAESGCM(signedJwt, secretKey);

            // Crea un nuevo JObject con "jwe" como campo y el token encriptado como su valor
            JObject signedAndEncryptedJwtAsJObject = new JObject { { "jwe", signedAndEncryptedJwt } };

            // Devuelve el JWT firmado y encriptado
            return signedAndEncryptedJwtAsJObject.ToString();
        }
    }
}
