using LabIntegrator.Core.Contracts.Messages;
using LabIntegrator.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabIntegrator.Api.Controllers;

/// <summary>
/// Controlador responsável pela ingestão e consulta de mensagens laboratoriais.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly ILabMessageService _messageService;

    /// <summary>
    /// Construtor com injeção do serviço de mensagens.
    /// </summary>
    public MessagesController(ILabMessageService messageService)
    {
        _messageService = messageService;
    }

    /// <summary>
    /// Retorna mensagens recentes, opcionalmente filtradas por canal.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<MessageDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecentAsync([FromQuery] Guid? channelId, [FromQuery] int take = 50, CancellationToken cancellationToken = default)
    {
        var messages = await _messageService.GetRecentAsync(channelId, take, cancellationToken);
        return Ok(messages);
    }

    /// <summary>
    /// Retorna detalhes completos de uma mensagem específica.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var message = await _messageService.GetByIdAsync(id, cancellationToken);
        return message is null ? NotFound() : Ok(message);
    }

    /// <summary>
    /// Ingestão de nova mensagem laboratorial.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateMessageRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var message = await _messageService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = message.Id }, message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza o status de processamento de uma mensagem.
    /// </summary>
    [HttpPost("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatusAsync(Guid id, [FromBody] UpdateMessageStatusRequest request, CancellationToken cancellationToken)
    {
        if (request is null || !request.IsValid())
        {
            ModelState.AddModelError(nameof(request.ErrorMessage), "Informe a mensagem de erro ao registrar falha.");
            return ValidationProblem(ModelState);
        }

        var updated = await _messageService.UpdateStatusAsync(
            id,
            request.Succeeded ? null : request.ErrorMessage,
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }
}



