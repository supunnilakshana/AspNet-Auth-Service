using BS_Web_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using BS_Web_Api.Services.UserService;
using BS_Web_Api.Helpers;
using System.Runtime;

namespace BS_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
      
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly   AuthHelper _authHelper;
        public AuthController(IConfiguration configuration,IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
            _authHelper=new AuthHelper(configuration);
        }
        [HttpPost("register")]
        public ActionResult<AuthResponse> Register(AuthRequest request)
        {
            User savedUser = _userService.GetByUserName(request.Email);
            if (savedUser != null)
            {
                return BadRequest("This Email is already used");
            }

            _authHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            User user = new User();
            user.UserName = request.Email;
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Role = request.Role;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _userService.Create(user);
            string token = _authHelper.CreateToken(user);
            savedUser = _userService.GetByUserName(request.Email)!;

            var refreshToken = _authHelper.GenerateRefreshToken(savedUser.Id.ToString());
           
            UserDto userDto = new UserDto {
                Id=savedUser.Id.ToString(),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName= user.LastName,
                Role = user.Role,
                UserName = user.UserName,         
            };
            AuthResponse response =
                new AuthResponse {
                    AccesToken = token,
                    RefreshToken = refreshToken.Token,
                    User=userDto
            };

            return Ok(response);

        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(AuthRequest request)
        {
            var user = _userService.GetByUserName(request.Email);
            if (user==null)
            {
                return BadRequest("User not found.");
            }

            if (!_authHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            string token = _authHelper.CreateToken(user);

            var refreshToken = _authHelper.GenerateRefreshToken(user.Id.ToString());

            UserDto userDto = new UserDto
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                UserName = user.UserName,
            };
            AuthResponse response =
                new AuthResponse
                {
                    AccesToken = token,
                    RefreshToken = refreshToken.Token,
                    User = userDto
                };

            return Ok(new
            {
                token = token,
                
                user = new
                {
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.Role,
                }

            });
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                RefreshToken? refreshToken = _authHelper.GetRefreshToken(request.UserId);
                if (refreshToken == null)
                {
                    return Unauthorized("Invalid Refresh Token.");
                }

                if (!refreshToken.Token.Equals(request.RefreshToken))
                {
                    return Unauthorized("Invalid Refresh Token.");
                }
                else if (refreshToken.Expires < DateTime.Now)
                {
                    return Unauthorized("Token expired.");
                }

                var user = _userService.GetById(request.UserId);
                string token = _authHelper.CreateToken(user);

                var newRefreshToken = _authHelper.GenerateRefreshToken(request.UserId);

                return Ok(new RefreshTokenResponse
                {
                    AccesToken = token,
                    RefreshToken = newRefreshToken.Token
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }

    }
}
