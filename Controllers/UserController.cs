

using AllInOne.Models;
using AllInOne.Services;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost]  
    public async Task<ActionResult<UserModel>> CreateUser([FromBody] UserModel user)
    {
        try{
            UserModel data =await _userService.CreateUser(user);
            return data;

        }catch(Exception ex){
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,"Error Creating new User");
        }
    }
}

