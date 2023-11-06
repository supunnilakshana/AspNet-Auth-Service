namespace BS_Web_Api.Models
{
    public class AuthRequest
    {
        public string Email { get; set; }
        public string Role { get; set; } = "user";
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
