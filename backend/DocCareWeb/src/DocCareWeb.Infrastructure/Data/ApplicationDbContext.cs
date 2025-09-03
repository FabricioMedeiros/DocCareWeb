using DocCareWeb.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocCareWeb.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthPlan> HealthPlans { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(60); 
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Street).IsRequired().HasMaxLength(50);
                entity.Property(a => a.Number).HasMaxLength(10);
                entity.Property(a => a.Complement).HasMaxLength(20);
                entity.Property(a => a.Neighborhood).IsRequired().HasMaxLength(50);
                entity.Property(a => a.City).IsRequired().HasMaxLength(50);
                entity.Property(a => a.State).IsRequired().HasMaxLength(2);
                entity.Property(a => a.ZipCode).IsRequired().HasMaxLength(8);
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.DoctorId).IsRequired();
                entity.Property(a => a.PatientId).IsRequired();
                entity.Property(a => a.HealthPlanId).IsRequired();
                entity.Property(a => a.Cost).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(a => a.AppointmentDate).IsRequired();
                entity.Property(a => a.StartTime).IsRequired();
                entity.Property(a => a.EndTime).IsRequired();
                entity.Property(p => p.CreatedAt).IsRequired();
                entity.Property(p => p.CreatedBy).IsRequired();
                entity.Property(a => a.Notes).HasMaxLength(500);

                entity.HasOne(a => a.Doctor )
                      .WithMany()
                      .HasForeignKey(a => a.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Patient)
                      .WithMany()
                      .HasForeignKey(a => a.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.HealthPlan)
                      .WithMany()
                      .HasForeignKey(a => a.HealthPlanId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Name).IsRequired().HasMaxLength(50);
                entity.Property(d => d.Crm).IsRequired().HasMaxLength(20);
                entity.Property(d => d.CellPhone).IsRequired().HasMaxLength(11);
                entity.Property(d => d.Email).IsRequired().HasMaxLength(60);
                entity.HasOne(d => d.Specialty)
                      .WithMany()
                      .HasForeignKey(d => d.SpecialtyId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(50);
                entity.Property(p => p.Cpf).HasMaxLength(11);
                entity.Property(p => p.Rg).HasMaxLength(20);
                entity.Property(p => p.Gender).IsRequired();
                entity.Property(p => p.BirthDate).IsRequired();
                entity.Property(p => p.Phone).HasMaxLength(10);
                entity.Property(p => p.CellPhone).HasMaxLength(11);
                entity.Property(p => p.Email).HasMaxLength(60);
                entity.Property(p => p.CreatedAt).IsRequired();
                entity.Property(p => p.CreatedBy).IsRequired();
                entity.Property(p => p.Notes).HasMaxLength(5000);
                entity.HasOne(p => p.Address)
                      .WithMany()
                      .HasForeignKey(p => p.AddressId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(p => p.HealthPlan)
                      .WithMany(hp => hp.Patients)
                      .HasForeignKey(p => p.HealthPlanId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<HealthPlan>(entity =>
            {
                entity.HasKey(hp => hp.Id);
                entity.Property(hp => hp.Description).IsRequired().HasMaxLength(60);
                entity.Property(hp => hp.Cost).HasColumnType("decimal(18,2)");
                entity.HasMany(hp => hp.Patients)
                      .WithOne(p => p.HealthPlan)
                      .HasForeignKey(p => p.HealthPlanId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Specialty>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Description).IsRequired().HasMaxLength(50);
            });            
        }
    }
}
