using GymPassport.Domain.Models;
using GymPassport.GymPassportAPI.ApiConnectors;
using GymPassport.GymPassportAPI.Services.AuthenticationServices;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace GymPassport.GymPassportAPI.Tests.Services.AuthenticationServices
{
    public class AuthenticationServiceTests
    {
        [Fact]
        public async Task Login_WithCorrectAdminCredentials_ReturnsExpectedAccount()
        {
            // Arrange
            string username = "claudio33usr";
            string password = "claudio33pswd";
            Account expectedAccount = new Account()
            {
                Username = username,
                Role = "admin",
                GymName = "macfit mataro",
                Name = "Claudio"
            };

            AppSettings fakeAppSettings = new AppSettings();
            IOptions<AppSettings> fakeOptions = Substitute.For<IOptions<AppSettings>>();
            fakeOptions.Value.Returns(fakeAppSettings);

            IHttpClientFactory fakeHttpClientFactory = Substitute.For<IHttpClientFactory>();

            fakeHttpClientFactory.CreateClient(Arg.Any<string>()).Returns(new HttpClient());

            LoginApiConnector loginApiConnector = new LoginApiConnector(fakeHttpClientFactory);

            AuthenticationService authenticationService = new AuthenticationService(fakeOptions, loginApiConnector);

            // Act
            Account result = await authenticationService.Login(username, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAccount.Username, result.Username);
            Assert.Equal(expectedAccount.Role, result.Role);
            Assert.Equal(expectedAccount.GymName, result.GymName);
            Assert.Equal(expectedAccount.Name, result.Name);
        }

        [Fact]
        public async Task Login_WithCorrectNormalCredentials_ReturnsExpectedAccount()
        {
            // Arrange
            string username = "julia33usr";
            string password = "julia33pswd";
            Account expectedAccount = new Account()
            {
                Username = username,
                Role = "normal",
                GymName = "macfit mataro"
            };

            AppSettings fakeAppSettings = new AppSettings();
            IOptions<AppSettings> fakeOptions = Substitute.For<IOptions<AppSettings>>();
            fakeOptions.Value.Returns(fakeAppSettings);

            IHttpClientFactory fakeHttpClientFactory = Substitute.For<IHttpClientFactory>();

            fakeHttpClientFactory.CreateClient(Arg.Any<string>()).Returns(new HttpClient());

            LoginApiConnector loginApiConnector = new LoginApiConnector(fakeHttpClientFactory);

            AuthenticationService authenticationService = new AuthenticationService(fakeOptions, loginApiConnector);

            // Act
            Account result = await authenticationService.Login(username, password);

            // Assert
            // Add assertions based on your expected behavior
            Assert.NotNull(result);
            Assert.Equal(expectedAccount.Username, result.Username);
            Assert.Equal(expectedAccount.Role, result.Role);
            Assert.Equal(expectedAccount.GymName, result.GymName);

            // Additional assertions as needed
        }
    }
}