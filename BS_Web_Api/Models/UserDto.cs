using System.ComponentModel.DataAnnotations;

namespace BS_Web_Api.Models
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } 
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; } = string.Empty;



    }
}
