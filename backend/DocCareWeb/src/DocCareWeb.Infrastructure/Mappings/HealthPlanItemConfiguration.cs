using DocCareWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocCareWeb.Infrastructure.Mappings
{
    public class HealthPlanItemConfiguration : IEntityTypeConfiguration<HealthPlanItem>
    {
        public void Configure(EntityTypeBuilder<HealthPlanItem> builder)
        {
            builder.ToTable("HealthPlanItems");

            builder.HasKey(hpi => hpi.Id);
            builder.Property(hpi => hpi.HealthPlanId).IsRequired();
            builder.Property(hpi => hpi.ServiceId).IsRequired();
            builder.Property(hpi => hpi.Price).HasColumnType("decimal(10,2)").IsRequired();

            builder.HasOne(hpi => hpi.Service)
                   .WithMany(s => s.HealthPlanItems) 
                   .HasForeignKey(hpi => hpi.ServiceId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(hpi => hpi.HealthPlan)
                   .WithMany(hp => hp.Items)
                   .HasForeignKey(hpi => hpi.HealthPlanId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}