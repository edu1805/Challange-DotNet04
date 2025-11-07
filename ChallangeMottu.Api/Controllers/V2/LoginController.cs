using Asp.Versioning;
using ChallangeMottu.Application;
using ChallangeMottu.Application.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChallangeMottu.Api.Controllers;

[Route("api/v{version:apiVersion}/login")]
[ApiController]
[ApiVersion("2.0")]
[Produces("application/json")]
[SwaggerTag("Gerenciamento de Login")]
[AllowAnonymous]
public class LoginController : ControllerBase
{
    private readonly ILoginUseCase _loginUseCase;

    public LoginController(ILoginUseCase loginUseCase)
    {
        _loginUseCase = loginUseCase;
    }

    /// <summary>
    /// Realiza login e retorna token JWT.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     POST /api/v2/login
    ///     {
    ///         "email": "usuario@email.com",
    ///         "senha": "senha123"
    ///     }
    /// 
    /// </remarks>

    [HttpPost]
    [SwaggerOperation(Summary = "Login Service", Description = "Get Token")]
    [SwaggerResponse(StatusCodes.Status200OK, "Token returned")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server Error")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _loginUseCase.ExecuteAsync(request);

        if (!response.Sucesso)
            return Unauthorized(new { message = response.Mensagem });

        return Ok(response);
    }
}