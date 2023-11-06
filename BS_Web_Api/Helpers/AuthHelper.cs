using Azure;
using BS_Web_Api.Data;
using BS_Web_Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BS_Web_Api.Helpers
{
    public class AuthHelper
    {

        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;

        public AuthHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _appDbContext =new AppDbContext();
        }

       

       public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

       

        public RefreshToken GenerateRefreshToken(String userId)
        {
            User? user = _appDbContext.Users.FirstOrDefault(x => x.Id.ToString().Equals(userId));
            if (user == null)
            {
                throw new Exception("Invalid User Id");
            }
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.Created;
            user.TokenExpires = refreshToken.Expires;
            _appDbContext.Users.Update(user);
            _appDbContext.SaveChanges();
            return refreshToken;

        }

        public RefreshToken? GetRefreshToken(String userId)
        {
            User? user = _appDbContext.Users.FirstOrDefault(x => x.Id.ToString().Equals(userId));
            if (user == null)
            {
                return null;
            }
            var refreshToken = new RefreshToken
            {
                Token = user.RefreshToken,
                Expires = user.TokenExpires,
                Created = user.TokenCreated
            };
            return refreshToken;

        }



    }
}
