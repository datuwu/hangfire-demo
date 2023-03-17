using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NoteManagementAPI.DAL;
using NoteManagementAPI.Entities;
using NoteManagementAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoteManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly UnitOfWork _unitOfWork;

        public TokensController(IConfiguration config, UnitOfWork unitOfWork)
        {
            _configuration = config;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (loginVM == null || loginVM.Username == null || loginVM.Password == null)
                return BadRequest();

            var user = _unitOfWork.UserRepo
                .Get(filter: u => u.Username == loginVM.Username && u.Password == loginVM.Password
                    , includeProperties: "Role")
                .FirstOrDefault();

            if (user == null)
                return BadRequest("Invalid credentials");

            //create claims details based on the user information
            var claims = new[] {
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", user.Id.ToString()),
                new Claim("Username", user.Username),
                new Claim("role", user.Role.Name),
                new Claim("Status", user.Status.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            user.LastLoginTime = DateTime.Now;

            _unitOfWork.LoginRecordRepo.Insert(new LoginRecord { UserId = user.Id, LoginTime = DateTime.Now });
            _unitOfWork.UserRepo.Update(user);
            _unitOfWork.Save();

            return Ok(new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
