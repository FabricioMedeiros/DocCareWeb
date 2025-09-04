using DocCareWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocCareWeb.Infrastructure.Mappings
{
    public class HealthPlanConfiguration : IEntityTypeConfiguration<HealthPlan>
    {
        public void Configure(EntityTypeBuilder<HealthPlan> builder)
        {
            builder.HasKey(hp => hp.Id);
            builder.Property(hp => hp.Description).IsRequired().HasMaxLength(60);
            builder.Property(hp => hp.Cost).HasColumnType("decimal(18,2)");

            builder.HasMany(hp => hp.Patients)
                   .WithOne(p => p.HealthPlan)
                   .HasForeignKey(p => p.HealthPlanId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}