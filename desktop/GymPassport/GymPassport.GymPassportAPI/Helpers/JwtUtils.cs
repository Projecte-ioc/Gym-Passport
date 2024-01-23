using GymPassport.Domain.Models;
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

        /// <summary>
        /// Mapea los JWT claims a un objeto Account a partir de los claims.
        /// </summary>
        /// <param name="claims">Los claims del JWT.</param>
        /// <returns>Un objeto Account con los datos de los claims.</returns>
        public static Account MapJwtClaimsToAccount(JObject claims)
        {
            // Extract values from claims
            string username = claims.GetValue("user_name")?.ToString();
            string userRole = claims.GetValue("rol_user")?.ToString();
            string gymName = claims.GetValue("gym_name")?.ToString();
            string name = claims.GetValue("name")?.ToString();

            // Create an instance of the Account class
            return new Account(username, userRole, gymName, name, null); // You may replace null with the actual token value
        }

        /*static IEnumerable<Claim> ConvertJObjectToClaims(JObject jObject)
        {
            var claims = new List<Claim>();
            foreach (var property in jObject.Properties())
            {
                claims.Add(new Claim(property.Name, property.Value.ToString()));
            }

            return claims;
        }*/
    }
}
