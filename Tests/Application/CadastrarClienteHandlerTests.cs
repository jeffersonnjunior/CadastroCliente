using Application.Interfaces;
using Application.Clientes.CadastrarCliente;
using Application.Exceptions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace CadastroCliente.Tests.Application;

public class CadastrarClienteHandlerTests
{
    private readonly IClienteRepository _clientes = Substitute.For<IClienteRepository>();
    private readonly IOutboxWriter _outbox = Substitute.For<IOutboxWriter>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private CadastrarClienteHandler CreateSut() =>
        new(_clientes, _outbox, _unitOfWork);

    [Fact]
    public async Task HandleAsync_ComDadosValidos_DevePersistirEEnfileirarOutbox()
    {
        // Arrange
        _clientes.ExistsByCpfAsync(Arg.Any<Cpf>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var sut = CreateSut();
        var command = new CadastrarClienteCommand(
            "Maria Silva",
            "52998224725",
            "maria@email.com");

        // Act
        var result = await sut.HandleAsync(command);

        // Assert
        result.Nome.Should().Be("Maria Silva");
        result.Cpf.Should().Be("52998224725");
        await _clientes.Received(1).AddAsync(Arg.Any<Cliente>(), Arg.Any<CancellationToken>());
        await _outbox.Received(1).EnqueueAsync(
            "ClienteCadastrado",
            Arg.Any<string>(),
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ComCpfDuplicado_DeveLancarConflito()
    {
        // Arrange
        _clientes.ExistsByCpfAsync(Arg.Any<Cpf>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var sut = CreateSut();
        var command = new CadastrarClienteCommand(
            "Maria Silva",
            "52998224725",
            "maria@email.com");

        // Act
        var act = async () => await sut.HandleAsync(command);

        // Assert
        await act.Should().ThrowAsync<CpfDuplicadoException>();
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ComCpfInvalido_DeveLancarDomainException()
    {
        // Arrange
        var sut = CreateSut();
        var command = new CadastrarClienteCommand(
            "Maria Silva",
            "11111111111",
            "maria@email.com");

        // Act
        var act = async () => await sut.HandleAsync(command);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
    }
}
