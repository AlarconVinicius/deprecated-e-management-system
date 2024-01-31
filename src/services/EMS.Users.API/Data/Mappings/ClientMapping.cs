using EMS.Core.DomainObjects;
using EMS.Users.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Users.API.Data.Mappings;

public class ClientMapping : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.SubscriberId)
               .IsRequired();

        builder.Property(c => c.Name)
               .IsRequired()
               .HasColumnType("varchar(200)");

        builder.OwnsOne(c => c.Cpf, tf =>
        {
            tf.Property(c => c.Number)
              .IsRequired()
              .HasMaxLength(Cpf.MaxCpfLength)
              .HasColumnName("Cpf")
              .HasColumnType($"varchar({Cpf.MaxCpfLength})");
        });

        builder.OwnsOne(c => c.Email, tf =>
        {
            tf.Property(c => c.Address)
              .IsRequired()
              .HasColumnName("Email")
              .HasColumnType($"varchar({Email.AddressMaxLength})");
        });

        builder.HasOne(c => c.Address)
               .WithOne(c => c.Client)
               .HasForeignKey<Address>(a => a.UserId);

        builder.HasOne(c => c.Subscriber)
            .WithMany(s => s.Clients)
            .HasForeignKey(c => c.SubscriberId);
    }
}