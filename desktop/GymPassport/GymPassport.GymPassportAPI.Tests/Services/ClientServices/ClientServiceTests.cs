using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.Services.AuthenticationServices;
using GymPassport.GymPassportAPI.Services.ClientServices;
using System.Net;

namespace GymPassport.GymPassportAPI.Tests.Services.ClientServices
{
    /// <summary>
    /// Clase de prueba para ClientService.
    /// </summary>
    [Collection("API")]
    public class ClientServiceTests
    {
        private readonly DefaultHttpClientFactory _httpClientFactory;
        private readonly AuthenticationService _authenticationService;
        private readonly ClientService _clientService;
        private readonly LoginData _loginDataAdmin = new LoginData
        {
            Username = "claudio33usr",
            Password = "claudio33pswd"
        };

        /// <summary>
        /// Constructor que inicializa el HttpClientFactory.
        /// </summary>
        public ClientServiceTests()
        {
            _httpClientFactory = new DefaultHttpClientFactory();
            _authenticationService = new AuthenticationService(_httpClientFactory);
            _clientService = new ClientService(_httpClientFactory);
        }

        /// <summary>
        /// Método de prueba para GetAllProfileInfo con datos de usuario válidos.
        /// </summary>
        [Fact]
        public async Task GetAllProfileInfo_WithValidAccessToken_ShouldReturnUserProfile()
        {
            // Arrange
            Account loggedAccount = await _authenticationService.Login(_loginDataAdmin);

            // Act
            // Llamada al método GetAllProfileInfo y obtención del resultado
            UserProfile result = await _clientService.GetAllProfileInfo(loggedAccount.AuthToken);

            // Assert
            // Verificación de varias condiciones en el objeto UserProfile devuelto
            Assert.NotNull(result);
            Assert.NotEmpty(result.Username);
            Assert.NotEmpty(result.UserRole);
            Assert.NotEmpty(result.GymName);
            Assert.NotEmpty(result.GymAddress);
            Assert.NotEmpty(result.GymPhoneNumber);
        }

        /// <summary>
        /// Método de prueba para GetAllProfileInfo con datos de usuario inválidos.
        /// </summary>
        [Fact]
        public async Task GetAllProfileInfo_WithInvalidAccessToken_ShouldThrowException()
        {
            // Arrange
            Account loggedAccount = new Account
            {
                AuthToken = "token"
            };

            // Act & Assert
            // Verificación de que el método GetAllProfileInfo lanza HttpRequestException en caso de token invalido
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await _clientService.GetAllProfileInfo(loggedAccount.AuthToken));

            // Verificación adicional del mensaje de la excepción
            var expectedMessage = $"La petición a la API ha fallado con código: {HttpStatusCode.InternalServerError}.";
            Assert.Contains(expectedMessage, exception.Message);
        }

        /// <summary>
        /// Método de prueba para InsertClient con datos válidos de nuevo cliente.
        /// </summary>
        [Fact]
        public async Task InsertClient_WithValidAccessTokenAndValidClientData_ShouldNotThrowException()
        {
            // Arrange
            Account loggedAccount = await _authenticationService.Login(_loginDataAdmin);

            // Datos del nuevo cliente
            Client newClient = new Client
            {
                Name = "Ainhoa",
                Role = "admin",
                Username = "ainhoa24usr",
                Password = "ainhoa24psw"
            };

            // Act & Assert
            try
            {
                await _clientService.InsertClient(loggedAccount.AuthToken, newClient);
                await _clientService.DeleteClient(loggedAccount.AuthToken, newClient.Username);
            }
            catch (HttpRequestException ex)
            {
                // Maneja la excepción si es lanzada
                Assert.Fail($"Se lanzó una excepción inesperada: {ex.Message}");
            }

            // Si no se lanzó ninguna excepción, la prueba pasa automáticamente
            Assert.True(true);
        }

        /// <summary>
        /// Método de prueba para InsertClient con token inválido
        /// </summary>
        [Fact]
        public async Task InsertClient_WithInvalidAccessTokenAndValidClientData_ShouldThrowException()
        {
            // Arrange
            Account loggedAccount = new Account
            {
                AuthToken = "token"
            };

            // Datos del nuevo cliente
            Client newClient = new Client
            {
                Name = "Ainhoa",
                Role = "admin",
                Username = "ainhoa24usr",
                Password = "ainhoa24psw"
            };

            // Act & Assert
            // Verificación de que el método InsertClient lanza HttpRequestException en caso de token invalido
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await _clientService.InsertClient(loggedAccount.AuthToken, newClient));

            // Verificación adicional del mensaje de la excepción
            var expectedMessage = $"La petición a la API ha fallado con código: {HttpStatusCode.InternalServerError}.";
            Assert.Contains(expectedMessage, exception.Message);
        }

        
        /// <summary>
        /// Método de prueba para UpdateClient con datos válidos de cliente actualizado.
        /// </summary>
        [Fact]
        public async Task UpdateClient_WithValidAccessTokenAndValidUpdatedClientData_ShouldNotThrowException()
        {
            // Arrange
            Account loggedAccount = await _authenticationService.Login(_loginDataAdmin);

            // Datos de un nuevo cliente
            Client newClient = new Client
            {
                Name = "Ainhoa",
                Role = "admin",
                Username = "ainhoa24usr",
                Password = "ainhoa24psw"
            };

            await _clientService.InsertClient(loggedAccount.AuthToken, newClient);

            // Datos del nuevo cliente actualizados
            var updatedClient = new Client
            {
                Name = "Ainhoa",
                Role = "normal",
                Username = "ainhoa24usr",
                Password = "ainhoa24psw"
            };

            // Act & Assert
            try
            {
                await _clientService.UpdateClient(loggedAccount.AuthToken, updatedClient);
            }
            catch (HttpRequestException ex)
            {
                // Maneja la excepción si es lanzada
                Assert.Fail($"Se lanzó una excepción inesperada: {ex.Message}");
            }

            // Si no se lanzó ninguna excepción, la prueba pasa automáticamente
            Assert.True(true);
        }

        /// <summary>
        /// Método de prueba para UpdateClient con token inválido
        /// </summary>
        [Fact]
        public async Task UpdateClient_WithInvalidAccessTokenAndValidUpdatedClientData_ShouldThrowException()
        {
            // Arrange
            Account loggedAccount = await _authenticationService.Login(_loginDataAdmin);

            // Datos de un nuevo cliente
            Client newClient = new Client
            {
                Name = "Ainhoa",
                Role = "admin",
                Username = "ainhoa24usr",
                Password = "ainhoa24psw"
            };

            await _clientService.InsertClient(loggedAccount.AuthToken, newClient);

            // Datos del nuevo cliente actualizados
            var updatedClient = new Client
            {
                Name = "Ainhoa",
                Role = "normal",
                Username = "ainhoa24usr",
                Password = "ainhoa24psw"
            };

            // Act & Assert
            // Verificación de que el método UpdateClient lanza HttpRequestException en caso de token invalido
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await _clientService.UpdateClient("token", updatedClient));
            await _clientService.DeleteClient(loggedAccount.AuthToken, updatedClient.Username);

            // Verificación adicional del mensaje de la excepción
            var expectedMessage = $"La petición a la API ha fallado con código: {HttpStatusCode.InternalServerError}.";
            Assert.Contains(expectedMessage, exception.Message);
        }
        
        /// <summary>
        /// Método de prueba para DeleteClient con datos válidos de cliente a eliminar.
        /// </summary>
        [Fact]
        public async Task DeleteClient_WithValidAccessTokenAndValidDeletedClientData_ShouldNotThrowException()
        {
            // Arrange
            Account loggedAccount = await _authenticationService.Login(_loginDataAdmin);

            // Datos de un nuevo cliente
            Client newClient = new Client
            {
                Name = "Ainhoa",
                Role = "admin",
                Username = "ainhoa24usr",
                Password = "ainhoa24psw"
            };

            await _clientService.InsertClient(loggedAccount.AuthToken, newClient);

            // Nombre de usuario del cliente a eliminar
            var deletedClientUserName = newClient.Username;

            // Act & Assert
            try
            {
                await _clientService.DeleteClient(loggedAccount.AuthToken, deletedClientUserName);
            }
            catch (HttpRequestException ex)
            {
                // Maneja la excepción si es lanzada
                Assert.Fail($"Se lanzó una excepción inesperada: {ex.Message}");
            }

            // Si no se lanzó ninguna excepción, la prueba pasa automáticamente
            Assert.True(true);
        }

        /// <summary>
        /// Método de prueba para DeleteClient con token invalido
        /// </summary>
        [Fact]
        public async Task DeleteClient_WithInvalidAccessTokenAndValidDeletedClientData_ShouldNotThrowException()
        {
            // Arrange
            Account loggedAccount = await _authenticationService.Login(_loginDataAdmin);

            // Datos de un nuevo cliente
            Client newClient = new Client
            {
                Name = "Ainhoa",
                Role = "admin",
                Username = "ainhoa24usr",
                Password = "ainhoa24psw"
            };

            await _clientService.InsertClient(loggedAccount.AuthToken, newClient);

            // Nombre de usuario del cliente a eliminar
            var deletedClientUserName = newClient.Username;

            // Act & Assert
            // Verificación de que el método DeleteClient lanza HttpRequestException en caso de token invalido
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await _clientService.DeleteClient("token", deletedClientUserName));
            await _clientService.DeleteClient(loggedAccount.AuthToken, newClient.Username);

            // Verificación adicional del mensaje de la excepción
            var expectedMessage = $"La petición a la API ha fallado con código: {HttpStatusCode.InternalServerError}.";
            Assert.Contains(expectedMessage, exception.Message);
        }
    }
}