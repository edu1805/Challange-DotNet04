using System.Security.Cryptography;
using System.Text;

namespace ChallangeMottu.Domain;

public class Usuario
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; private set; }
    public string Email { get; private set; }
    private string SenhaHash { get; set; }
    private string SenhaSalt { get; set; }
    public Guid? MotoId { get; private set; }
    public Moto? Moto { get; private set; }

    private Usuario() { }

    public Usuario(string nome, string email, string senha, Guid? motoId = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório.");
        if (string.IsNullOrWhiteSpace(senha))
            throw new ArgumentException("Senha é obrigatório.");

        Nome = nome;
        Email = email;
        CreatePasswordHash(senha);
        MotoId = motoId;
    }

    public void AtribuirMoto(Moto? moto)
    {
        if (moto != null && moto.Id == Guid.Empty)
            throw new ArgumentException("Moto inválida.");

        Moto = moto;
        MotoId = moto?.Id;
    }
    
    private void CreatePasswordHash(string senha) 
    {
        using var hmac = new HMACSHA512();
        SenhaSalt = Convert.ToBase64String(hmac.Key); // Chave única (salt) 
        SenhaHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(senha)));
    }
    
    public bool VerifyPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        using var hmac = new HMACSHA512(Convert.FromBase64String(SenhaSalt));
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        var computedHashString = Convert.ToBase64String(computedHash);

        return computedHashString == SenhaHash;
    }
    
    public void AtualizarDados(string nome, string email, Guid? motoId)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório.");

        Nome = nome;
        Email = email;
        MotoId = motoId;
    }
}
