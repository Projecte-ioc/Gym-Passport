using Jose;
using Newtonsoft.Json.Linq;
using System.Text;

namespace GymPassport.GymPassportAPI.Helpers
{
    public static class JwtUtils
    {
        /// <summary>
        /// Convierte el JObject pasado por parámetro en un JWT firmado con la clave secreta proporcionada.
        /// </summary>
        /// <param name="payload">El JObject que contiene el payload del JWT.</param>
        /// <param name="secretKey">La clave secreta para firmar el JWT.</param>
        /// <returns>Devuelve un JWT firmado.</returns>
        public static string ConvertToSignedJWT(JObject jObject, string secretKey)
        {
            // Codifica el payload como un token JWT firmado usando la librería JOSE
            return JWT.Encode(jObject.ToString(), Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS256);

            /*IEnumerable<Claim> claims = ConvertJObjectToClaims(jObject);

            // Convierte las reclamaciones a un diccionario antes de llamar a JWT.Encode
            Dictionary<string, string> claimsDictionary = claims.ToDictionary(c => c.Type, c => c.Value);

            return JWT.Encode(claimsDictionary, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS256);*/
        }
    }
}
