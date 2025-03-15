using DnDBeyondChallenge.Models;
using DnDBeyondChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

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

    [HttpPost("{name}/damage")]
    public async Task<IActionResult> DealDamage(string name, [FromBody] DamageRequest request)
    {
        if (!request.TryParseDamageType(out DamageType damageType))
            return BadRequest("Invalid damage type.");

        var character = await characterService.GetCharacterAsync(name);

        if (character == null)
            return NotFound($"Character '{name}' not found.");

        if (character.IsImmune(damageType))
            return Ok($"{name} is immune to {damageType} damage. Took no damage.");

        var amount = character.TakeDamage(damageType, request.Amount);
        await characterService.SaveCharacterAsync(character);

        return Ok($"{name} took {amount} {damageType} damage.");

    }

    [HttpPost("{name}/heal")]
    public async Task<IActionResult> Heal(string name, [FromBody] HealRequest request)
    {
        var character = await characterService.GetCharacterAsync(name);

        if (character == null)
            return NotFound($"Character '{name}' not found.");

        character.Heal(request.Amount);

        return Ok($"{name} healed for {request.Amount}.");
    }

    [HttpPost("{name}/addTempHP")]
    public async Task<IActionResult> AddTemporaryHP(string name, [FromBody] TempHPRequest request)
    {
        var character = await characterService.GetCharacterAsync(name);

        if (character == null)
            return NotFound($"Character '{name}' not found.");

        character.AddTemporaryHitPoints(request.Amount);

        return Ok($"{name} was added {request.Amount} temporary HP.");
    }
}
