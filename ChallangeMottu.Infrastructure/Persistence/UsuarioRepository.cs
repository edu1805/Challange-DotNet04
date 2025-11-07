using ChallangeMottu.Domain;
using ChallangeMottu.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChallangeMottu.Infrastructure.Persistence;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ChallangeMottuContext _context;

    public UsuarioRepository(ChallangeMottuContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObterPorIdAsync(Guid id)
    {
        return await _context.Usuario
            .Include(u => u.Moto) 
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario?> BuscarPorEmailAsync(string email)
    {
        return await _context.Usuario
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<Usuario>> ListarTodosAsync()
    {
        return await _context.Usuario
            .Include(u => u.Moto)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Usuario usuario)
    {
        await _context.Usuario.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        _context.Usuario.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task DeletarAsync(Usuario usuario)
    {
        _context.Usuario.Remove(usuario);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Usuario?> ObterPorMotoIdAsync(Guid motoId)
    {
        return await _context.Usuario
            .Include(u => u.Moto)
            .FirstOrDefaultAsync(u => u.MotoId == motoId);
    }
    
}