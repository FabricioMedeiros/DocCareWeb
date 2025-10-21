using DocCareWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocCareWeb.Infrastructure.Mappings
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.DoctorId).IsRequired();
            builder.Property(a => a.PatientId).IsRequired();
            builder.Property(a => a.HealthPlanId).IsRequired();
            builder.Property(a => a.AppointmentDate).IsRequired();
            builder.Property(a => a.StartTime).IsRequired();
            builder.Property(a => a.EndTime).IsRequired();
            builder.Property(a => a.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(a => a.CreatedAt).IsRequired();
            builder.Property(a => a.CreatedBy).IsRequired();
            builder.Property(a => a.Notes).HasMaxLength(500);

            builder.HasOne(a => a.Doctor)
                   .WithMany()
                   .HasForeignKey(d => d.DoctorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Patient)
                   .WithMany()
                   .HasForeignKey(p => p.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.HealthPlan)
                   .WithMany()
                   .HasForeignKey(hs => hs.HealthPlanId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Items)
                   .WithOne(s => s.Appointment)
       .            HasForeignKey(a => a.AppointmentId);

        }
    }
}