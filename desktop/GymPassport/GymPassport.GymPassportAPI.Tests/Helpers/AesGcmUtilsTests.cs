using GymPassport.GymPassportAPI.Helpers;
using Newtonsoft.Json;

namespace GymPassport.GymPassportAPI.Tests.Helpers
{
    public class AesGcmUtilsTests
    {
        [Fact]
        public void EncryptWithInvalidKeyLength_ThrowsFormatException()
        {
            // Arrange
            var originalObject = new { Name = "John", Age = 30 };
            string plaintext = JsonConvert.SerializeObject(originalObject);
            string invalidAesKey = "llave";

            // Act & Assert
            Assert.Throws<FormatException>(() => AesGcmUtils.EncryptWithAESGCM(plaintext, invalidAesKey));
        }

        [Fact]
        public void EncryptAndDecrypt_ReturnsOriginalJson()
        {
            // Arrange
            var originalObject = new { Name = "John", Age = 30 };
            string plaintext = JsonConvert.SerializeObject(originalObject);
            string aesKey = "__PROBANDO__probando__";

            // Act
            string encryptedText = AesGcmUtils.EncryptWithAESGCM(plaintext, aesKey);
            string decryptedText = AesGcmUtils.DecryptWithAESGCM(encryptedText, aesKey);

            // Assert
            var decryptedObject = JsonConvert.DeserializeAnonymousType(decryptedText, originalObject);
            Assert.Equal(originalObject, decryptedObject);
        }

        [Fact]
        public void EncryptAndDecrypt_WithDifferentKeys_ReturnsSameJson()
        {
            // Arrange
            var originalObject = new { Name = "John", Age = 30 };
            string plaintext = JsonConvert.SerializeObject(originalObject);
            string aesKey1 = "__PROBANDO__probando__";
            string aesKey2 = "__patata__ZANAHORIA___";

            // Act
            string encryptedText1 = AesGcmUtils.EncryptWithAESGCM(plaintext, aesKey1);
            string encryptedText2 = AesGcmUtils.EncryptWithAESGCM(plaintext, aesKey2);

            // Assert
            Assert.NotEqual(encryptedText1, encryptedText2);

            // Desencripta y comprueba que los objetos JSON sean iguales
            string decryptedText1 = AesGcmUtils.DecryptWithAESGCM(encryptedText1, aesKey1);
            string decryptedText2 = AesGcmUtils.DecryptWithAESGCM(encryptedText2, aesKey2);

            var decryptedObject1 = JsonConvert.DeserializeAnonymousType(decryptedText1, originalObject);
            var decryptedObject2 = JsonConvert.DeserializeAnonymousType(decryptedText2, originalObject);

            Assert.Equal(originalObject, decryptedObject1);
            Assert.Equal(originalObject, decryptedObject2);
        }

        [Theory]
        [InlineData("SGVsbG8gd29ybGQh", "Hello world!")]
        [InlineData("aGFyZC1kYXRhLXRleHQ=", "hard-data-text")]
        public void Base64UrlDecode_DecodesCorrectly(string base64UrlEncoded, string expectedDecoded)
        {
            // Act
            byte[] decodedBytes = AesGcmUtils.Base64UrlDecode(base64UrlEncoded);

            // Assert
            string decodedString = System.Text.Encoding.UTF8.GetString(decodedBytes);
            Assert.Equal(expectedDecoded, decodedString);
        }

        [Fact]
        public void Base64UrlDecode_WithInvalidCharacter_ThrowsFormatException()
        {
            // Arrange
            string base64UrlEncoded = "invalid@encoded";

            // Act & Assert
            Assert.Throws<FormatException>(() => AesGcmUtils.Base64UrlDecode(base64UrlEncoded));
        }
    }
}
