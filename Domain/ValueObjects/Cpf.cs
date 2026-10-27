using System.Text.RegularExpressions;
using Domain.Exceptions;

namespace Domain.ValueObjects;

public sealed partial class Cpf : IEquatable<Cpf>
{
    public string Value { get; }

    private Cpf(string value) => Value = value;

    public static Cpf Create(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            throw new DomainException("CPF é obrigatório.");

        var digits = DigitsOnly().Replace(raw, string.Empty);

        if (digits.Length != 11)
            throw new DomainException("CPF deve conter 11 dígitos.");

        if (digits.Distinct().Count() == 1)
            throw new DomainException("CPF inválido.");

        if (!HasValidCheckDigits(digits))
            throw new DomainException("CPF inválido.");

        return new Cpf(digits);
    }

    private static bool HasValidCheckDigits(string digits)
    {
        var numbers = digits.Select(c => c - '0').ToArray();

        var sum1 = 0;
        for (var i = 0; i < 9; i++)
            sum1 += numbers[i] * (10 - i);

        var d1 = sum1 % 11;
        d1 = d1 < 2 ? 0 : 11 - d1;
        if (numbers[9] != d1)
            return false;

        var sum2 = 0;
        for (var i = 0; i < 10; i++)
            sum2 += numbers[i] * (11 - i);

        var d2 = sum2 % 11;
        d2 = d2 < 2 ? 0 : 11 - d2;
        return numbers[10] == d2;
    }

    public bool Equals(Cpf? other) => other is not null && Value == other.Value;

    public override bool Equals(object? obj) => obj is Cpf other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;

    [GeneratedRegex(@"\D")]
    private static partial Regex DigitsOnly();
}
