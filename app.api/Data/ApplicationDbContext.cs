using Microsoft.EntityFrameworkCore;
using app.api.Models;

namespace app.api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações do User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
            });

            // Configurações do Schedule
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Doctor)
                    .WithMany(e => e.Schedules)
                    .HasForeignKey(e => e.DoctorId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => e.DoctorId).IsUnique();
            });

            // Configurações do Appointment
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Doctor)
                    .WithMany(e => e.AppointmentsAsDoctor)
                    .HasForeignKey(e => e.DoctorId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Patient)
                    .WithMany(e => e.AppointmentsAsPatient)
                    .HasForeignKey(e => e.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Índices para melhor performance
            modelBuilder.Entity<Appointment>()
                .HasIndex(e => new { e.DoctorId, e.AppointmentDate });

            modelBuilder.Entity<Appointment>()
                .HasIndex(e => new { e.PatientId, e.AppointmentDate });
        }
    }
} 