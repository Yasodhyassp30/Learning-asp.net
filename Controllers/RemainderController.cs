
using System.Text.Json;
using AllInOne.Models;
using AllInOne.Services;
using Microsoft.AspNetCore.Mvc;
namespace AllInOne.Controllers;

public class RemainderController:ControllerBase{

    private readonly RemainderService _remainderService;
    private readonly ILogger<RemainderController> _logger;
    public RemainderController(RemainderService remainderService, ILogger<RemainderController> logger){
        _remainderService = remainderService;
        _logger = logger;
    }

    [HttpGet]
    [Route("api/remainder/{userId}")]
    public async Task<ActionResult<List<RemainderModel>>> GetRemainders(string userId)
    {
        try{
            var data = await _remainderService.GetRemainders(userId);
            return data;
        }catch(Exception ex){
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,JsonSerializer.Serialize(new {message = "Error Fetching Remainders"}));
        }
    }

}

