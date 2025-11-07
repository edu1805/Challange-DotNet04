namespace ChallangeMottu.Application;

public class LoginResponse
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public string? Token { get; set; }
    public Guid? UsuarioId { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
}