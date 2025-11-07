namespace ChallangeMottu.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorIdAsync(Guid id);
    Task<Usuario?> BuscarPorEmailAsync(string email);
    Task<IEnumerable<Usuario>> ListarTodosAsync();
    Task AdicionarAsync(Usuario usuario);
    Task AtualizarAsync(Usuario usuario);
    Task DeletarAsync(Usuario usuario);
    Task<Usuario?> ObterPorMotoIdAsync(Guid motoId);
}