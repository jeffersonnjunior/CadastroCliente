using System.Net.Mail;
using Domain.Exceptions;

namespace Domain.ValueObjects;

public sealed class EmailAddress : IEquatable<EmailAddress>
{
    public string Value { get; }

    private EmailAddress(string value) => Value = value;

    public static EmailAddress Create(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            throw new DomainException("E-mail é obrigatório.");

        var normalized = raw.Trim().ToLowerInvariant();

        try
        {
            var parsed = new MailAddress(normalized);
            if (!parsed.Address.Equals(normalized, StringComparison.OrdinalIgnoreCase))
                throw new DomainException("E-mail inválido.");
        }
        catch (FormatException)
        {
            throw new DomainException("E-mail inválido.");
        }

        if (!normalized.Contains('@') || normalized.StartsWith('@') || normalized.EndsWith('@'))
            throw new DomainException("E-mail inválido.");

        return new EmailAddress(normalized);
    }

    public bool Equals(EmailAddress? other) =>
        other is not null && Value == other.Value;

    public override bool Equals(object? obj) =>
        obj is EmailAddress other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}
