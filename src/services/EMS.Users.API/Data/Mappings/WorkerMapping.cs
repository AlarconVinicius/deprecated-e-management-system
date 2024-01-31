using EMS.Core.DomainObjects;
using EMS.Users.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Users.API.Data.Mappings;

public class WorkerMapping : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.ToTable("Workers");

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

        builder.Property(c => c.Salary)
            .IsRequired();

        builder.Property(c => c.Commission)
            .IsRequired();

        builder.Property(c => c.HardSkills)
            .IsRequired()
            .HasMaxLength(250);

        builder.HasOne(c => c.Address)
               .WithOne(c => c.Worker)
               .HasForeignKey<Address>(a => a.UserId);

        builder.HasOne(w => w.Subscriber)
            .WithMany(s => s.Workers)
            .HasForeignKey(w => w.SubscriberId);
    }
}