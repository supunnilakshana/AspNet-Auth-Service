using System.ComponentModel.DataAnnotations;

namespace BS_Web_Api.Models
{
    public class User
    {

        [Key]
        [Required]
        public Guid Id { get; set; } 
        [Required]
        
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
