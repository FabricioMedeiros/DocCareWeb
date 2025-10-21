using DocCareWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocCareWeb.Infrastructure.Mappings
{
    public class HealthPlanConfiguration : IEntityTypeConfiguration<HealthPlan>
    {
        public void Configure(EntityTypeBuilder<HealthPlan> builder)
        {
            builder.ToTable("HealthPlans");

            builder.HasKey(hp => hp.Id);
            builder.Property(hp => hp.Name).IsRequired().HasMaxLength(60);

            builder.HasMany(hp => hp.Items)
                   .WithOne(item => item.HealthPlan)
                   .HasForeignKey(item => item.HealthPlanId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}