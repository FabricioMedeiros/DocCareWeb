using DocCareWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocCareWeb.Infrastructure.Mappings
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(d => d.Crm)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(d => d.CellPhone)
                   .IsRequired()
                   .HasMaxLength(11);

            builder.Property(d => d.Email)
                   .IsRequired()
                   .HasMaxLength(60);

            builder.HasOne(d => d.Specialty)
                   .WithMany()
                   .HasForeignKey(d => d.SpecialtyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}