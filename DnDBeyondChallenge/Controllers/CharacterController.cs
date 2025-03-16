using DnDBeyondChallenge.Models;
using DnDBeyondChallenge.Services;
using Microsoft.AspNetCore.Mvc;

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

        var response = new GetCharacterResponse
        {
            Name = character.Name,
            HitPoints = character.HitPoints,
            TemporaryHitPoints = character.TemporaryHitPoints,
            Level = character.Level,
            Classes = character.Classes,
            Stats = character.Stats,
            Items = character.Items,
            Defenses = character.Defenses
        };

        return Ok(response);
    }

    [HttpPost("{name}/damage")]
    public async Task<IActionResult> DealDamage(string name, [FromBody] DamageRequest request)
    {
        if (!request.TryParseDamageType(out DamageType damageType))
        {
            return BadRequest("Invalid damage type.");
        }

        var character = await characterService.GetCharacterAsync(name);

        if (character == null)
        {
            return NotFound($"Character '{name}' not found.");
        }

        if (character.HitPoints <= 0 && character.TemporaryHitPoints <= 0)
        {
            return BadRequest($"{name} HP is at 0. Cannot take any more damage.");
        }

        DamageResponse response;
        if (character.IsImmune(damageType))
        {
            response = new DamageResponse
            {
                Name = character.Name,
                HitPoints = character.HitPoints,
                TemporaryHitPoints = character.TemporaryHitPoints,
                DamageTaken = 0
            };
            return Ok(response);
        }

        var amount = character.TakeDamage(damageType, request.Amount);
        await characterService.SaveCharacterAsync(character);

        response = new DamageResponse
        {
            Name = character.Name,
            HitPoints = character.HitPoints,
            TemporaryHitPoints = character.TemporaryHitPoints,
            DamageTaken = amount,
        };

        return Ok(response);

    }

    [HttpPost("{name}/heal")]
    public async Task<IActionResult> Heal(string name, [FromBody] HealRequest request)
    {
        var character = await characterService.GetCharacterAsync(name);

        if (character == null)
        {
            return NotFound($"Character '{name}' not found.");
        }

        HealResponse response;

        if (character.MaxHitPoints == character.HitPoints)
        {
            response = new HealResponse
            {
                Name = character.Name,
                HitPoints = character.HitPoints,
                TemporaryHitPoints = character.TemporaryHitPoints,
                MaxHitPoints = character.MaxHitPoints
            };
            return Ok(response);
        }

        character.Heal(request.Amount);

        await characterService.SaveCharacterAsync(character);

        response = new HealResponse
        {
            Name = character.Name,
            HitPoints = character.HitPoints,
            TemporaryHitPoints = character.TemporaryHitPoints,
            MaxHitPoints = character.MaxHitPoints
        };

        return Ok(response);
    }

    [HttpPost("{name}/addTempHP")]
    public async Task<IActionResult> AddTemporaryHP(string name, [FromBody] TempHPRequest request)
    {
        var character = await characterService.GetCharacterAsync(name);

        if (character == null)
        {
            return NotFound($"Character '{name}' not found.");
        }

        if (character.HitPoints <= 0)
        {
            return BadRequest($"{name} HP is at 0. Cannot add temporary health.");
        }

        character.AddTemporaryHitPoints(request.Amount);

        await characterService.SaveCharacterAsync(character);

        var tempHPResponse = new TempHPResponse
        {
            Name = character.Name,
            HitPoints = character.HitPoints,
            TemporaryHitPoints = character.TemporaryHitPoints,
        };

        return Ok(tempHPResponse);
    }


}
