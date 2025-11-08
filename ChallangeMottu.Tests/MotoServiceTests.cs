using ChallangeMottu.Application;
using ChallangeMottu.Application.Validators;
using ChallangeMottu.Domain.Interfaces;
using Moq;

namespace ChallangeMottu.Tests;

public class MotoServiceTests
{
    [Fact]
    public async Task CriarMoto_DeveLancarExcecao_SePlacaForVazia()
    {
        // Arrange
        var dto = new CreateMotoDto
        {
            Placa = "", // inválida
            Posicao = "A1",
            Status = "Pronta"
        };

        var motoRepositoryMock = new Mock<IMotoRepository>();
        var validator = new CreateMotoDtoValidator(motoRepositoryMock.Object);

        // Act
        var resultado = await validator.ValidateAsync(dto);

        // Assert
        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == "Placa");
    }
}