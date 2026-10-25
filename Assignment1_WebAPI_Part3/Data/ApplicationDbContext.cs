using Microsoft.EntityFrameworkCore;

namespace Assignment1_WebAPI_Part3.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      => optionsBuilder.UseSqlServer("Server = localhost\\SAGDA ; Database = Assignment1-Part3; Trusted_Connection = true; TrustServerCertificate = true;");

        public DbSet<Entities.Doctor> Doctors { get; set; }
        public DbSet<Entities.Patient> Patients { get; set; }
        public DbSet<Entities.Appointment> Appointments { get; set; }
    }
}
