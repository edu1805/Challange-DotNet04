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
[ApiVersion("2.0")]
[SwaggerTag("Controller responsável por gerenciar usuários.")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Lista todos os usuários.
    /// </summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Listar usuários", Description = "Retorna todos os usuários cadastrados.")]
    [SwaggerResponse(200, "Lista de usuários retornada com sucesso", typeof(IEnumerable<UsuarioDto>))]
    [SwaggerResponse(500, "Erro interno no servidor")]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
    {
        var usuarios = await _usuarioService.ListarTodosAsync();
        return Ok(usuarios);
    }

    /// <summary>
    /// Obtém um usuário por ID.
    /// </summary>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Buscar usuário por ID", Description = "Retorna um usuário específico pelo ID.")]
    [SwaggerResponse(200, "Usuário encontrado", typeof(UsuarioDto))]
    [SwaggerResponse(404, "Usuário não encontrado")]
    public async Task<ActionResult<UsuarioDto>> GetById(Guid id)
    {
        var usuario = await _usuarioService.ObterPorIdAsync(id);
        if (usuario == null) return NotFound();

        return Ok(usuario);
    }

    /// <summary>
    /// Cria um novo usuário.
    /// </summary>
    [HttpPost("criar")]
    [SwaggerOperation(Summary = "Criar usuário", Description = "Cria um novo usuário com os dados informados.")]
    [SwaggerResponse(201, "Usuário criado com sucesso", typeof(UsuarioDto))]
    [SwaggerResponse(400, "Dados inválidos")]
    public async Task<ActionResult<UsuarioDto>> Create(
        [FromBody] CreateUsuarioDto dto,
        [FromServices] CreateUsuarioDtoValidator validator)
    {
        var result = await validator.ValidateAsync(dto);

        if (!result.IsValid)
        {
            var modelState = new ModelStateDictionary();
            foreach (var failure in result.Errors)
                modelState.AddModelError(failure.PropertyName, failure.ErrorMessage);

            return ValidationProblem(modelState);
        }

        var usuario = await _usuarioService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
    }

    /// <summary>
    /// Atualiza um usuário existente.
    /// </summary>
    [HttpPut("editar/{id}")]
    [SwaggerOperation(Summary = "Atualizar usuário", Description = "Atualiza os dados de um usuário existente.")]
    [SwaggerResponse(204, "Usuário atualizado com sucesso")]
    [SwaggerResponse(404, "Usuário não encontrado")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateUsuarioDto dto)
    {
        var atualizado = await _usuarioService.AtualizarAsync(id, dto);
        if (atualizado == null) return NotFound();
        
        return NoContent();
    }

    /// <summary>
    /// Deleta um usuário.
    /// </summary>
    [HttpDelete("delete/{id}")]
    [SwaggerOperation(Summary = "Deletar usuário", Description = "Remove um usuário do sistema.")]
    [SwaggerResponse(204, "Usuário deletado com sucesso")]
    [SwaggerResponse(404, "Usuário não encontrado")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deletado = await _usuarioService.DeletarAsync(id);
        if (!deletado) return NotFound();

        return NoContent();
    }
}