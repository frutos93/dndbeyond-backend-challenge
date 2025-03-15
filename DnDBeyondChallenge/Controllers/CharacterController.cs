using DnDBeyondChallenge.Models;
using DnDBeyondChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;
using System.Threading.Tasks;

namespace DnDBeyondChallenge.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly CharacterService characterService;
    public CharacterController(CharacterService charService)
    {
        characterService = charService;
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetCharacter(string name)
    {
        var character = await characterService.GetCharacterAsync(name);
        if (character == null)
        {
            return NotFound($"Character '{name}' not found.");
        }
        return Ok(character);
    }
}
