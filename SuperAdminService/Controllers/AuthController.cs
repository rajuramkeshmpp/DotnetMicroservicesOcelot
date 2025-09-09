using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SuperAdminService.Models;
using SuperAdminService.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SuperAdminService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IDynamoDBContext _dbContext;
        private readonly IConfiguration _config;

        public AuthController(IDynamoDBContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        // ✅ Register User
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            // check if email already exists
            var existingUsers = await _dbContext.ScanAsync<User>(
                new List<ScanCondition>
                {
                    new ScanCondition("Email", ScanOperator.Equal, user.Email)
                }).GetRemainingAsync();

            if (existingUsers.Any())
                return BadRequest("User already exists");

            // get max Id
            var allUsers = await _dbContext.ScanAsync<User>(new List<ScanCondition>()).GetRemainingAsync();
            int nextId = 1;
            if (allUsers.Any())
            {
                nextId = allUsers.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            }

            // assign numeric ID
            user.Id = nextId;

            await _dbContext.SaveAsync(user);

            return Ok(new
            {
                Message = "User registered successfully",
                User = user
            });
        }

        // ✅ Login User
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            // find user by email + password
            var users = await _dbContext.ScanAsync<User>(
                new List<ScanCondition>
                {
            new ScanCondition("Email", ScanOperator.Equal, loginDto.Email),
            new ScanCondition("Password", ScanOperator.Equal, loginDto.Password)
                }).GetRemainingAsync();

            var user = users.FirstOrDefault();
            if (user == null)
                return Ok(new { token = "", name = "", role = "", success = "400" }); // user not found

            // get userRole mapping
            var userRoles = await _dbContext.ScanAsync<UserRole>(
                new List<ScanCondition>
                {
            new ScanCondition("UserId", ScanOperator.Equal, user.Id)
                }).GetRemainingAsync();

            var userRole = userRoles.FirstOrDefault();
            if (userRole == null)
                return Ok(new { token = "", name = "", role = "", success = "401" }); // role not assigned

            // get role details
            var role = await _dbContext.LoadAsync<Role>(userRole.RoleId);

            // generate JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: null,
                expires: DateTime.Now.AddHours(Convert.ToDouble(_config["Jwt:ExpiresInHours"])),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = jwt,
                user = user,     // full user details
                role = role,     // role object
                success = "200"
            });
        }

    }
}
