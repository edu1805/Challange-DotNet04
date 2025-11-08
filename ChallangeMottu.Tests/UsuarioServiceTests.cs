using AutoMapper;
using ChallangeMottu.Application;
using ChallangeMottu.Application.UseCase;
using ChallangeMottu.Application.Validators;
using ChallangeMottu.Domain;
using ChallangeMottu.Domain.Interfaces;
using Moq;

namespace ChallangeMottu.Tests;

public class UsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UsuarioService _usuarioService;

    public UsuarioServiceTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _mapperMock = new Mock<IMapper>();
        _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task CriarUsuario_DeveCriarComSucesso()
    {
        // Arrange
        var dto = new CreateUsuarioDto
        {
            Nome = "Edu Barriviera",
            Email = "edu@example.com",
            Senha = "123456",
            MotoId = null
        };

        var usuario = new Usuario(dto.Nome, dto.Email, dto.Senha, dto.MotoId);

        // Configura o mock do mapper para converter o DTO em entidade e vice-versa
        _mapperMock.Setup(m => m.Map<Usuario>(dto)).Returns(usuario);
        _mapperMock.Setup(m => m.Map<UsuarioDto>(usuario)).Returns(new UsuarioDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            MotoId = usuario.MotoId
        });

        // Configura o repositório para retornar null (sem moto associada)
        _usuarioRepositoryMock.Setup(r => r.ObterPorMotoIdAsync(It.IsAny<Guid>())).ReturnsAsync((Usuario?)null);
        _usuarioRepositoryMock.Setup(r => r.AdicionarAsync(usuario)).Returns(Task.CompletedTask);

        // Act
        var resultado = await _usuarioService.CriarAsync(dto);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(dto.Nome, resultado.Nome);
        Assert.Equal(dto.Email, resultado.Email);
        _usuarioRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Usuario>()), Times.Once);
    }

    [Fact]
    public async void CriarUsuario_DeveLancarErro_SeSenhaForMenorQue6Caracteres()
    {
        // Arrange
        var motoRepositoryMock = new Mock<IMotoRepository>();
        var validator = new CreateUsuarioDtoValidator(motoRepositoryMock.Object);

        var dto = new CreateUsuarioDto
        {
            Nome = "Eduardo",
            Email = "edu@example.com",
            Senha = "123" // inválida
        };

        // Act
        var resultado = await validator.ValidateAsync(dto);

        // Assert
        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == "Senha");
    }
}