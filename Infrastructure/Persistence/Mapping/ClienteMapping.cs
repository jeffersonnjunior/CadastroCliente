using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Mapping;

public sealed class ClienteMapping : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        var cpfConverter = new ValueConverter<Cpf, string>(
            v => v.Value,
            v => Cpf.Create(v));

        var emailConverter = new ValueConverter<EmailAddress, string>(
            v => v.Value,
            v => EmailAddress.Create(v));

        builder.ToTable("clientes");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.CriadoEmUtc)
            .IsRequired();

        builder.Property(x => x.Cpf)
            .HasConversion(cpfConverter)
            .HasColumnName("cpf")
            .HasMaxLength(11)
            .IsRequired();

        builder.HasIndex(x => x.Cpf).IsUnique();

        builder.Property(x => x.Email)
            .HasConversion(emailConverter)
            .HasColumnName("email")
            .HasMaxLength(256)
            .IsRequired();
    }
}
