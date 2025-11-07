using ChallangeMottu.Application.Service;
using ChallangeMottu.Domain.Interfaces;

namespace ChallangeMottu.Application.UseCase;

public class LoginUseCase : ILoginUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;

    public LoginUseCase(IUsuarioRepository usuarioRepository, ITokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> ExecuteAsync(LoginRequest request)
    {
        var usuario = await _usuarioRepository.BuscarPorEmailAsync(request.Email);

        if (usuario is null || !usuario.VerifyPassword(request.Senha))
        {
            return new LoginResponse
            {
                Sucesso = false,
                Mensagem = "Credenciais inválidas"
            };
        }

        var token = _tokenService.GenerateToken(usuario);

        return new LoginResponse
        {
            Sucesso = true,
            Mensagem = "Login realizado com sucesso",
            Token = token,
            UsuarioId = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email
        };
    }
}