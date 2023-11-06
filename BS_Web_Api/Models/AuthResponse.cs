namespace BS_Web_Api.Models
{
    public class AuthResponse
    {
        public string AccesToken { get; set; }
        public string RefreshToken { get; set; }
        public UserDto User { get; set; }

    }
}
