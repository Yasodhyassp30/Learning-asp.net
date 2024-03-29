

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using AllInOne.Models;
using AllInOne.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AllInOne.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController:ControllerBase{
    private readonly UserService _userService;
    private readonly ILogger<UserController> _logger;
    public UserController(UserService userService, ILogger<UserController> logger){
        _userService = userService;
        _logger = logger;
    }

    private string GenerateJwtToken(UserModel user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        IConfigurationSection JwtSettings = this.HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(JwtSettings["SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id)
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [HttpPost]  
    [Route("register")]
    public async Task<ActionResult<UserModel>> CreateUser([FromBody] UserModel user)
    {
        try{
            var userExists = await _userService.GetUserByEmail(user.Email);
            if(userExists != null){
                return StatusCode(StatusCodes.Status400BadRequest,JsonSerializer.Serialize(new {message = "User with this email already exists"}));
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            UserModel data =await _userService.CreateUser(user);
            data.Token = GenerateJwtToken(data);
            return data;

        }catch(Exception ex){
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,JsonSerializer.Serialize(new {message = "Error Creating new User"}));
        }
    }
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<UserModel>> Login([FromBody] UserLoginModel user)
    {
        try{
            var userData = await _userService.GetUserByEmail(user.Email);
            if(userData == null){
                return StatusCode(StatusCodes.Status400BadRequest,JsonSerializer.Serialize(new {message = "User with this email does not exist"}));
            }
            bool isValid = BCrypt.Net.BCrypt.Verify(user.Password,userData.Password);
            if(isValid){
                userData.Token = GenerateJwtToken(userData);
                return userData;
            }else{
                return StatusCode(StatusCodes.Status400BadRequest,JsonSerializer.Serialize(new {message = "Invalid Password"}));
            }
        }catch(Exception ex){
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,JsonSerializer.Serialize(new {message = "Error Logging in User"}));
        }
    }
}

