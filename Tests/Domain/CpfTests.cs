using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace CadastroCliente.Tests.Domain;

public class CpfTests
{
    [Theory]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")]
    public void Create_ComCpfValido_DeveNormalizarDigitos(string input)
    {
        // Arrange & Act
        var cpf = Cpf.Create(input);

        // Assert
        cpf.Value.Should().Be("52998224725");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ComCpfVazio_DeveFalhar(string? input)
    {
        // Arrange & Act
        var act = () => Cpf.Create(input);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*obrigatório*");
    }

    [Theory]
    [InlineData("11111111111")]
    [InlineData("12345678900")]
    [InlineData("123")]
    public void Create_ComCpfInvalido_DeveFalhar(string input)
    {
        // Arrange & Act
        var act = () => Cpf.Create(input);

        // Assert
        act.Should().Throw<DomainException>();
    }
}
