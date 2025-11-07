using ChallangeMottu.Domain.Interfaces;
using FluentValidation;

namespace ChallangeMottu.Application.Validators;

public class CreateUsuarioDtoValidator : AbstractValidator<CreateUsuarioDto>
{
    public CreateUsuarioDtoValidator(IMotoRepository motoRepository)
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome não pode exceder 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("O e-mail é obrigatório.")
            .EmailAddress()
            .WithMessage("O e-mail deve ser válido.");
        
        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória")
            .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres")
            .MaximumLength(100).WithMessage("Senha deve ter no máximo 100 caracteres");

        RuleFor(x => x.MotoId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("O MotoId informado é inválido.")
            .MustAsync(async (id, cancellation) =>
            {
                if (id == null) return true; // não é obrigatório
                var moto = await motoRepository.GetByIdAsync(id.Value);
                return moto != null;
            })
            .WithMessage("O MotoId informado não existe.");
    }
}