namespace GymPassport.GymPassportAPI.Tests.Services
{
    /// <summary>
    /// Clase personalizada HttpClientFactory que implementa IHttpClientFactory.
    /// </summary>
    public class DefaultHttpClientFactory : IHttpClientFactory
    {
        /// <summary>
        /// Crea y devuelve una nueva instancia de HttpClient.
        /// </summary>
        public HttpClient CreateClient(string name)
        {
            return new HttpClient();
        }
    }
}
