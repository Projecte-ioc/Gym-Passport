using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.Services.AuthenticationServices;
using System.Net;
using System.Net.Http;

namespace GymPassport.GymPassportAPI.Tests.Services.AuthenticationServices
{
    /// <summary>
    /// Clase de prueba para AuthenticationService.
    /// </summary>
    [Collection("API")]
    public class AuthenticationServiceTests
    {
        private readonly DefaultHttpClientFactory _httpClientFactory;
        private readonly AuthenticationService _authenticationService;

        /// <summary>
        /// Constructor que inicializa el HttpClientFactory.
        /// </summary>
        public AuthenticationServiceTests()
        {
            _httpClientFactory = new DefaultHttpClientFactory();
            _authenticationService = new AuthenticationService(_httpClientFactory);
        }

        /// <summary>
        /// Método de prueba para Login con datos válidos de inicio de sesión de usuario administrador.
        /// </summary>
        [Fact]
        public async Task Login_WithValidAdminUserLoginData_ShouldReturnAdminRoleAccount()
        {
            // Arrange
            // Datos de entrada para el inicio de sesión
            var loginData = new LoginData
            {
                Username = "claudio33usr",
                Password = "claudio33pswd",
            };

            // Rol esperado después de un inicio de sesión exitoso
            var expectedRole = "admin";

            // Act
            // Llamada al método Login y obtención del resultado
            Account result = await _authenticationService.Login(loginData);

            // Assert
            // Verificación de varias condiciones en el objeto Account devuelto
            Assert.NotNull(result);
            Assert.NotEmpty(result.Username);
            Assert.NotEmpty(result.Role);
            Assert.NotEmpty(result.GymName);
            Assert.NotEmpty(result.Name);
            Assert.NotEmpty(result.AuthToken);
            Assert.Equal(expectedRole, result.Role);
        }

        /// <summary>
        /// Método de prueba para Login con datos válidos de inicio de sesión de usuario normal.
        /// </summary>
        [Fact]
        public async Task Login_WithValidNormalUserLoginData_ShouldReturnNormalRoleAccount()
        {
            // Arrange
            // Datos de entrada para el inicio de sesión
            var loginData = new LoginData
            {
                Username = "julia33usr",
                Password = "julia33pswd",
            };

            // Rol esperado después de un inicio de sesión exitoso
            var expectedRole = "normal";

            // Act
            // Llamada al método Login y obtención del resultado
            Account result = await _authenticationService.Login(loginData);

            // Assert
            // Verificación de varias condiciones en el objeto Account devuelto
            Assert.NotNull(result);
            Assert.NotEmpty(result.Username);
            Assert.NotEmpty(result.Role);
            Assert.NotEmpty(result.GymName);
            Assert.NotEmpty(result.Name);
            Assert.NotEmpty(result.AuthToken);
            Assert.Equal(expectedRole, result.Role);
        }

        /// <summary>
        /// Método de prueba para Login con datos inválidos de inicio de sesión.
        /// Debería lanzar una excepción HttpRequestException y verificar el mensaje.
        /// </summary>
        [Fact]
        public async Task Login_WithInvalidLoginData_ShouldThrowHttpRequestExceptionWithMessage()
        {
            // Arrange
            // Datos de inicio de sesión inválidos
            var invalidLoginData = new LoginData
            {
                Username = "usuario_invalido",
                Password = "clave_invalida",
            };

            // Act & Assert
            // Verificación de que el método Login lanza HttpRequestException en caso de credenciales inválidas
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await _authenticationService.Login(invalidLoginData));

            // Verificación adicional del mensaje de la excepción
            var expectedMessage = $"La petición a la API ha fallado con código: {HttpStatusCode.Unauthorized}.";
            Assert.Contains(expectedMessage, exception.Message);
        }
    }
}