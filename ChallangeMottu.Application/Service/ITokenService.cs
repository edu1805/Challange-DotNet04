

using ChallangeMottu.Domain;

namespace ChallangeMottu.Application.Service;

public interface ITokenService
{
    string GenerateToken(Usuario user);
}