namespace P01_HospitalDatabase.Data
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using P01_HospitalDatabase.Data.Models;

    public class HospitalContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<PatientMedicament> PatientsMedicaments { get; set; }

        public HospitalContext()
        {

        }

        public HospitalContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-VNP1D7N\SQLEXPRESS;Database=Hospital;Integrated Security = true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigurePatientEntity(modelBuilder);

            ConfigureVisitationEntity(modelBuilder);

            ConfigureDiagnoseEntity(modelBuilder);

            ConfigureMedicamentEntity(modelBuilder);

            ConfigurePatientMedicamentEntity(modelBuilder);

            ConfigureDoctorEntity(modelBuilder);
        }

        private void ConfigureDoctorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Doctor>()
                .HasKey(d => d.DoctorId);

            modelBuilder
                .Entity<Doctor>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsUnicode();

            modelBuilder
              .Entity<Doctor>()
              .Property(p => p.Specialty)
              .HasMaxLength(100)
              .IsUnicode();

            modelBuilder
                .Entity<Doctor>()
                .HasMany(d => d.Visitations)
                .WithOne(d => d.Doctor);
        }

        private void ConfigurePatientMedicamentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PatientMedicament>()
                .HasKey(pm => new
                {
                    pm.MedicamentId,
                    pm.PatientId
                });

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(pm => pm.Medicament)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(p => p.MedicamentId);

            modelBuilder
               .Entity<PatientMedicament>()
               .HasOne(pm => pm.Patient)
               .WithMany(p => p.Prescriptions)
               .HasForeignKey(p => p.PatientId);
        }

        private void ConfigureMedicamentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
              .Entity<Medicament>()
              .HasKey(m => m.MedicamentId);

            modelBuilder
                .Entity<Medicament>()
                .Property(d => d.Name)
                .HasMaxLength(50)
                .IsUnicode();
        }

        private void ConfigureDiagnoseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Diagnose>()
                .HasKey(d => d.DiagnoseId);

            modelBuilder
                .Entity<Diagnose>()
                .Property(d => d.Name)
                .HasMaxLength(250)
                .IsUnicode();

            modelBuilder
                .Entity<Diagnose>().
                Property(v => v.Comments)
                .HasMaxLength(250)
                .IsUnicode();
        }

        private void ConfigureVisitationEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Visitation>()
                .HasKey(v => v.VisitationId);

            modelBuilder
                .Entity<Visitation>().
                Property(v => v.Comments)
                .HasMaxLength(250)
                .IsUnicode();

            modelBuilder
                .Entity<Visitation>()
                .HasOne(p => p.Doctor)
                .WithMany(d => d.Visitations)
                .HasForeignKey(d => d.DoctorId);
        }

        private void ConfigurePatientEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
              .Entity<Patient>()
              .HasKey(p => p.PatientId);

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
                .Entity<Patient>()
                 .Property(p => p.LastName)
                .HasMaxLength(50)
            .IsUnicode();

            modelBuilder
               .Entity<Patient>()
               .Property(p => p.Address)
               .HasMaxLength(250);

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.Email)
                .HasMaxLength(80);

            modelBuilder
                .Entity<Patient>()
                .HasMany(p => p.Visitations)
                .WithOne(p => p.Patient);

            modelBuilder.
                Entity<Patient>()
               .HasMany(p => p.Diagnoses)
               .WithOne(p => p.Patient);
        }
    }
}
