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


    /// <summary>
    /// Retrieve a character's full object by name
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns cref="GetCharacterResponse"/>Character details. On error returns 404</returns>
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

    /// <summary>
    /// Deal damage to a character.
    /// Can only deal damage if character is not immune to the damage type.
    /// Can only deal damage if character has HP.
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="request" cref="DamageRequest">Request containing amount and type of damage.</param>
    /// <returns> Returns <see cref="IActionResult"/> with <see cref="DamageResponse"/>. Otherwise returns 404 or 400.</returns>
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
        await characterService.SaveCharacterAsync(name, character);

        response = new DamageResponse
        {
            Name = character.Name,
            HitPoints = character.HitPoints,
            TemporaryHitPoints = character.TemporaryHitPoints,
            DamageTaken = amount,
        };

        return Ok(response);

    }

    /// <summary>
    /// Heal a character.
    /// Can only heal if character is not at max HP but does not exceed max HP. Does not throw error if character is at max HP.
    /// Temporary HP is not affected by healing.
    /// </summary>
    /// <param name="name">Name of the character.</param>
    /// <param name="request" cref="HealRequest"> Request containing the amount to be healed.</param>
    /// <returns> Returns <see cref="IActionResult"/> with <see cref="HealResponse"/>. Otherwise returns 404.</returns>
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

        await characterService.SaveCharacterAsync(name, character);

        response = new HealResponse
        {
            Name = character.Name,
            HitPoints = character.HitPoints,
            TemporaryHitPoints = character.TemporaryHitPoints,
            MaxHitPoints = character.MaxHitPoints
        };

        return Ok(response);
    }

    /// <summary>
    /// Add temporary HP to a character.
    /// Can only add temporary HP if character has HP.
    /// If character already has temporary HP, the higher value is kept.
    /// </summary>
    /// <param name="name">Name of the character.</param>
    /// <param name="request" cref="TempHPRequest"> Request containing the amount add to temporary hit points.</param>
    /// <returns> Returns <see cref="IActionResult"/> with <see cref="TempHPResponse"/>. Otherwise returns 404.</returns>
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

        await characterService.SaveCharacterAsync(name, character);

        var tempHPResponse = new TempHPResponse
        {
            Name = character.Name,
            HitPoints = character.HitPoints,
            TemporaryHitPoints = character.TemporaryHitPoints,
        };

        return Ok(tempHPResponse);
    }


}
