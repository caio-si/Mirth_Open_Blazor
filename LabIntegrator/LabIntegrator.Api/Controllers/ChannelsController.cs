using LabIntegrator.Core.Contracts.Channels;
using LabIntegrator.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabIntegrator.Api.Controllers;

/// <summary>
/// Controlador responsável pela gestão de canais de integração.
/// Fornece endpoints CRUD consumidos pelo dashboard e scripts operacionais.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ChannelsController : ControllerBase
{
    private readonly ILabChannelService _channelService;

    /// <summary>
    /// Construtor com injeção do serviço de canais.
    /// </summary>
    public ChannelsController(ILabChannelService channelService)
    {
        _channelService = channelService;
    }

    /// <summary>
    /// Retorna todos os canais cadastrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ChannelDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var channels = await _channelService.GetAllAsync(cancellationToken);
        return Ok(channels);
    }

    /// <summary>
    /// Busca um canal específico pelo identificador.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ChannelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var channel = await _channelService.GetByIdAsync(id, cancellationToken);
        return channel is null ? NotFound() : Ok(channel);
    }

    /// <summary>
    /// Cadastra um novo canal.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ChannelDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateChannelRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var channel = await _channelService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = channel.Id }, channel);
    }

    /// <summary>
    /// Atualiza os dados de um canal existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ChannelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateChannelRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var channel = await _channelService.UpdateAsync(id, request, cancellationToken);

        return channel is null ? NotFound() : Ok(channel);
    }

    /// <summary>
    /// Remove um canal e mensagens relacionadas.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var removed = await _channelService.DeleteAsync(id, cancellationToken);
        return removed ? NoContent() : NotFound();
    }
}



