using Asp.Versioning;
using ChallangeMottu.Application;
using ChallangeMottu.Application.UseCase;
using ChallangeMottu.Application.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace ChallangeMottu.Api.Controllers;


[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[SwaggerTag("Controller responsável por gerenciar motos.")]
[ApiVersion("1.0")]
public class MotoController : ControllerBase
{
    private readonly IMotoService _motoService;

    public MotoController(IMotoService motoService)
    {
        _motoService = motoService;
    }

    /// <summary>
    /// Lista todas as motos ou filtra por status.
    /// </summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Listar motos", Description = "Retorna todas as motos ou apenas as que têm o status informado.")]
    [SwaggerResponse(200, "Lista de motos retornada com sucesso", typeof(IEnumerable<MotoDto>))]
    [SwaggerResponse(500, "Erro interno no servidor")]
    public async Task<ActionResult<IEnumerable<MotoDto>>> GetAll([FromQuery] string? status = null)
    {
        var motos = await _motoService.ListarMotosAsync(status);
        return Ok(motos);
    }

    /// <summary>
    /// Obtém uma moto por ID.
    /// </summary>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Buscar moto por ID", Description = "Retorna uma moto específica pelo ID.")]
    [SwaggerResponse(200, "Moto encontrada", typeof(MotoDto))]
    [SwaggerResponse(404, "Moto não encontrada")]
    public async Task<ActionResult<MotoDto>> GetById(Guid id)
    {
        var moto = await _motoService.BuscarPorIdAsync(id);
        if (moto == null) return NotFound();

        return Ok(moto);
    }

    /// <summary>
    /// Cria uma nova moto.
    /// </summary>
    [HttpPost("criar")]
    [SwaggerOperation(Summary = "Criar moto", Description = "Cria uma nova moto com os dados informados.")]
    [SwaggerResponse(201, "Moto criada com sucesso", typeof(MotoDto))]
    [SwaggerResponse(400, "Dados inválidos")]
    public async Task<ActionResult<MotoDto>> Create(
        [FromBody] CreateMotoDto dto,
        [FromServices] CreateMotoDtoValidator validator)
    {
        var result = await validator.ValidateAsync(dto);

        if (!result.IsValid)
        {
            var modelState = new ModelStateDictionary();
            foreach (var failure in result.Errors)
                modelState.AddModelError(failure.PropertyName, failure.ErrorMessage);

            return ValidationProblem(modelState);
        }

        var moto = await _motoService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = moto.Id }, moto);
    }

    /// <summary>
    /// Atualiza uma moto existente.
    /// </summary>
    [HttpPut("editar/{id}")]
    [SwaggerOperation(Summary = "Atualizar moto", Description = "Atualiza os dados de uma moto existente.")]
    [SwaggerResponse(204, "Moto atualizada com sucesso")]
    [SwaggerResponse(404, "Moto não encontrada")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateMotoDto dto)
    {
        var atualizado = await _motoService.AtualizarAsync(id, dto);
        if (!atualizado) return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Deleta uma moto.
    /// </summary>
    [HttpDelete("delete/{id}")]
    [SwaggerOperation(Summary = "Deletar moto", Description = "Remove uma moto do sistema.")]
    [SwaggerResponse(204, "Moto deletada com sucesso")]
    [SwaggerResponse(404, "Moto não encontrada")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deletado = await _motoService.DeletarAsync(id);
        if (!deletado) return NotFound();

        return NoContent();
    }
}