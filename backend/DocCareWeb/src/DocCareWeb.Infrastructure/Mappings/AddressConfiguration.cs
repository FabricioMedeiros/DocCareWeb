using DocCareWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocCareWeb.Infrastructure.Mappings
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.PatientId).IsRequired();
            builder.Property(a => a.Street).IsRequired().HasMaxLength(50);
            builder.Property(a => a.Number).HasMaxLength(10);
            builder.Property(a => a.Complement).HasMaxLength(20);
            builder.Property(a => a.Neighborhood).IsRequired().HasMaxLength(50);
            builder.Property(a => a.City).IsRequired().HasMaxLength(50);
            builder.Property(a => a.State).IsRequired().HasMaxLength(2);
            builder.Property(a => a.ZipCode).IsRequired().HasMaxLength(8);
        }
    }
}