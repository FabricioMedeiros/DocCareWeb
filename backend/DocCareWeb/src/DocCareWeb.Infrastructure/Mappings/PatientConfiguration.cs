using DocCareWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocCareWeb.Infrastructure.Mappings
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Cpf).HasMaxLength(11);
            builder.Property(p => p.Rg).HasMaxLength(20);
            builder.Property(p => p.Gender).IsRequired();
            builder.Property(p => p.BirthDate).IsRequired();
            builder.Property(p => p.Phone).HasMaxLength(10);
            builder.Property(p => p.CellPhone).HasMaxLength(11);
            builder.Property(p => p.Email).HasMaxLength(60);
            builder.Property(p => p.CreatedAt).IsRequired();
            builder.Property(p => p.CreatedBy).IsRequired();
            builder.Property(p => p.Notes).HasMaxLength(5000);

            builder.HasOne(p => p.Address)
                   .WithMany()
                   .HasForeignKey(p => p.AddressId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.HealthPlan)
                   .WithMany(hp => hp.Patients)
                   .HasForeignKey(p => p.HealthPlanId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}