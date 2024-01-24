using EMS.Subscription.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Subscription.API.Data.Mappings;

public class PlanUserConfiguration : IEntityTypeConfiguration<PlanUser>
{
    public void Configure(EntityTypeBuilder<PlanUser> builder)
    {
        builder.ToTable("PlanUsers");

        builder.HasKey(pu => pu.Id);

        builder.Property(pu => pu.PlanId)
            .IsRequired();

        builder.Property(pu => pu.ClientId)
            .IsRequired();

        builder.Property(pu => pu.UserName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(pu => pu.UserEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(pu => pu.UserCpf)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(pu => pu.IsActive)
            .IsRequired();

        builder.HasOne(pu => pu.Plan)
            .WithMany(p => p.PlanUsers)
            .HasForeignKey(pu => pu.PlanId);
    }
}