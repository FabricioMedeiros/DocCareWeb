using DocCareWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocCareWeb.Infrastructure.Mappings
{
    public class AppointmentItemConfiguration : IEntityTypeConfiguration<AppointmentItem>
    {
        public void Configure(EntityTypeBuilder<AppointmentItem> builder)
        {
            builder.ToTable("AppointmentItems");

            builder.HasKey(ai => ai.Id);
            builder.Property(ai => ai.AppointmentId).IsRequired();
            builder.Property(ai => ai.ServiceId).IsRequired();
            builder.Property(ai => ai.SuggestedPrice).HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(ai => ai.Price).HasColumnType("decimal(10,2)").IsRequired();

            builder.HasOne(ai => ai.Appointment)
                   .WithMany(a => a.Items)
                   .HasForeignKey(ai => ai.AppointmentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ai => ai.Service)
                   .WithMany() 
                   .HasForeignKey(ai => ai.ServiceId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
