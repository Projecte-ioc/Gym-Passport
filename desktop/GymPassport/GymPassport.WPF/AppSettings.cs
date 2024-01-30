using System.ComponentModel.DataAnnotations;

namespace GymPassport.WPF
{
    public sealed class AppSettings
    {
        public const string ApiSettingsSection = "ApiSettings";

        [Required, Url]
        public string BaseAddress { get; init; } = string.Empty;

        [Required]
        public string SecretKey { get; init; } = string.Empty;

        [Required]
        public int NonceSize { get; init; }

        [Required]
        public int TagSize { get; init; }
    }
}
