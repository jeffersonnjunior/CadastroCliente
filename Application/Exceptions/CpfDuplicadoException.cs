namespace Application.Exceptions;

public sealed class CpfDuplicadoException : Exception
{
    public string Cpf { get; }

    public CpfDuplicadoException(string cpf)
        : base($"Já existe cliente cadastrado com o CPF {cpf}.")
    {
        Cpf = cpf;
    }
}
