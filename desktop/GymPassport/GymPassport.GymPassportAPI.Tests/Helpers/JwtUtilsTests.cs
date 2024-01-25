using GymPassport.GymPassportAPI.Helpers;
using Newtonsoft.Json.Linq;

namespace GymPassport.GymPassportAPI.Tests.Helpers
{
    public class JwtUtilsTests
    {
        [Fact]
        public void ConvertToSignedJWT_ReturnsValidToken()
        {
            // Arrange
            var payload = new JObject
            {
                {"userId", 123},
                {"username", "john_doe"}
            };
            string secretKey = "__PROBANDO__probando__";

            // Act
            string jwtToken = JwtUtils.ConvertToSignedJWT(payload, secretKey);

            // Assert
            Assert.NotNull(jwtToken);
        }
    }
}
