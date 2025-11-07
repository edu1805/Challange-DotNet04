using Asp.Versioning;
using AutoMapper;
using ChallangeMottu.Application;
using ChallangeMottu.Application.UseCase;
using ChallangeMottu.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChallangeMottu.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[SwaggerTag("Controller responsável por gerenciar a localização atual das motos no pátio.")]
public class LocalizacaoAtualController : ControllerBase
{
    private readonly ILocalizacaoAtualService _service;
    private readonly IMapper _mapper;

    public LocalizacaoAtualController(ILocalizacaoAtualService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("all")]
    [SwaggerOperation(Summary = "Lista todas as localizações registradas das motos.")]
    [SwaggerResponse(200, "Retorna a lista de localizações.", typeof(IEnumerable<LocalizacaoAtualDto>))]
    public async Task<ActionResult<IEnumerable<LocalizacaoAtualDto>>> ListarTodas()
    {
        var localizacoes = await _service.ListarTodasAsync();
        var dto = _mapper.Map<IEnumerable<LocalizacaoAtualDto>>(localizacoes);
        return Ok(dto);
    }

    [HttpGet("moto/{motoId:guid}")]
    [SwaggerOperation(Summary = "Obtém a localização atual de uma moto pelo ID.")]
    [SwaggerResponse(200, "Retorna a localização da moto.", typeof(LocalizacaoAtualDto))]
    [SwaggerResponse(404, "Localização para a moto não encontrada.")]
    [Authorize]
    public async Task<ActionResult<LocalizacaoAtualDto>> ObterPorMotoId(Guid motoId)
    {
        var localizacao = await _service.ObterPorMotoIdAsync(motoId);
        if (localizacao == null)
            return NotFound($"Localização para a moto {motoId} não encontrada.");

        var dto = _mapper.Map<LocalizacaoAtualDto>(localizacao);
        return Ok(dto);
    }

    [HttpPost("create")]
    [SwaggerOperation(Summary = "Cria uma nova localização atual para uma moto.")]
    [SwaggerResponse(201, "Localização criada com sucesso.", typeof(LocalizacaoAtualDto))]
    [SwaggerResponse(400, "Dados inválidos para criação.")]
    [SwaggerResponse(500, "Erro interno no servidor")]
    [Authorize]
    public async Task<ActionResult> Criar([FromBody] CriarLocalizacaoAtualDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entidade = _mapper.Map<LocalizacaoAtual>(dto);
        
        var existente = await _service.ObterPorMotoIdAsync(dto.MotoId);
        if (existente != null)
            return Conflict($"Já existe uma localização registrada para a moto {dto.MotoId}.");

        await _service.AdicionarAsync(entidade);

        var retornoDto = _mapper.Map<LocalizacaoAtualDto>(entidade);
        return CreatedAtAction(nameof(ObterPorMotoId), new { motoId = retornoDto.MotoId }, retornoDto);
    }

    [HttpPut("moto_edit/{motoId:guid}")]
    [SwaggerOperation(Summary = "Atualiza a localização de uma moto existente.")]
    [SwaggerResponse(204, "Atualização concluída com sucesso.")]
    [SwaggerResponse(404, "Localização da moto não encontrada.")]
    [Authorize]
    public async Task<ActionResult> Atualizar(Guid motoId, [FromBody] AtualizarLocalizacaoAtualDto dto)
    {
        var existente = await _service.ObterPorMotoIdAsync(motoId);
        if (existente == null)
            return NotFound($"Localização da moto {motoId} não encontrada.");

        _mapper.Map(dto, existente);
        await _service.AtualizarAsync(existente);

        return NoContent();
    }

    [HttpDelete("moto_delete/{motoId:guid}")]
    [SwaggerOperation(Summary = "Deleta o registro de localização de uma moto.")]
    [SwaggerResponse(204, "Localização deletada com sucesso.")]
    [SwaggerResponse(404, "Localização da moto não encontrada.")]
    [Authorize]
    public async Task<ActionResult> Deletar(Guid motoId)
    {
        var existente = await _service.ObterPorMotoIdAsync(motoId);
        if (existente == null)
            return NotFound($"Localização da moto {motoId} não encontrada.");

        await _service.DeletarAsync(motoId);
        return NoContent();
    }
}        