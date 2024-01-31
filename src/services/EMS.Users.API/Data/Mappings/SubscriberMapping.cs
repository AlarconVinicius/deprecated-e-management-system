using EMS.Core.DomainObjects;
using EMS.Users.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Users.API.Data.Mappings;

public class SubscriberMapping : IEntityTypeConfiguration<Subscriber>
{
    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        builder.ToTable("Subscribers");

        builder.HasKey(c => c.Id);

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
               .WithOne(c => c.Subscriber)
               .HasForeignKey<Address>(a => a.UserId);

        builder.HasMany(s => s.Workers)
            .WithOne(w => w.Subscriber)
            .HasForeignKey(w => w.SubscriberId);

        builder.HasMany(s => s.Clients)
            .WithOne(c => c.Subscriber)
            .HasForeignKey(c => c.SubscriberId);
    }
}