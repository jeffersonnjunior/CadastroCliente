using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace CadastroCliente.Tests.Domain;

public class EmailAddressTests
{
    [Fact]
    public void Create_ComEmailValido_DeveNormalizar()
    {
        // Arrange & Act
        var email = EmailAddress.Create("  Maria.Silva@Email.COM ");

        // Assert
        email.Value.Should().Be("maria.silva@email.com");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("sem-arroba")]
    [InlineData("@semlocal.com")]
    public void Create_ComEmailInvalido_DeveFalhar(string? input)
    {
        // Arrange & Act
        var act = () => EmailAddress.Create(input);

        // Assert
        act.Should().Throw<DomainException>();
    }
}
