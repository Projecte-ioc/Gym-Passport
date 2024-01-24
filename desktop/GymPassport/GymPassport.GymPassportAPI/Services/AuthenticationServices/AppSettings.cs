namespace GymPassport.GymPassportAPI.Services.AuthenticationServices
{
    public class AppSettings
    {
        public ApiSettings ApiSettings { get; set; }
        public string SecretKey { get; set; }
    }

    public class ApiSettings
    {
        public string BaseUrl { get; set; }
    }
}