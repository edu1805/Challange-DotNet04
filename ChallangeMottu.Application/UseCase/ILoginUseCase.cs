namespace ChallangeMottu.Application.UseCase;

public interface ILoginUseCase
{
    Task<LoginResponse> ExecuteAsync(LoginRequest request);
}