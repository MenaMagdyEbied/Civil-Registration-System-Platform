using Civil_Registration_System_Platform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Civil_Registration_System_Platform.Relation_Constraint_Configration
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureAll(this ModelBuilder modelBuilder)
        {
            // =========================
            // UserAccount
            // =========================
            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasIndex(u => u.NationalID).IsUnique();

                entity.HasIndex(e=>e.Email).IsUnique();
                entity.HasIndex(u => u.UserName).IsUnique();

                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");   

                // User -> Governorate
                entity.HasOne(u => u.Governorate)
                    .WithMany(g => g.UserAccounts)
                    .HasForeignKey(u => u.GovernorateId)
                    .OnDelete(DeleteBehavior.Restrict);

                // User -> Office (employee)
                entity.HasOne(u => u.Office)
                    .WithMany(o => o.UserAccounts)
                    .HasForeignKey(u => u.OfficeId)
                    .OnDelete(DeleteBehavior.Restrict);

                // User -> ManageOffice (manager)
                entity.HasOne(u => u.ManageOffice)
                    .WithMany(o => o.ManageUserAccounts)
                    .HasForeignKey(u => u.ManageOfficeId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // =========================
            // Governorate
            // =========================
            modelBuilder.Entity<Governorate>(entity =>
            {
                entity.HasIndex(g => g.Code).IsUnique();
            });

            // =========================
            // Office
            // =========================
            modelBuilder.Entity<Office>(entity =>
            {

                entity.HasOne(o => o.Governorate)
                    .WithMany(g => g.Offices)
                    .HasForeignKey(o => o.GovernorateId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // =========================
            // Application
            // =========================
            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasIndex(a => a.ApplicationNumber).IsUnique();

                entity.Property(a => a.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");


                // Application -> Office
                entity.HasOne(a => a.Office)
                    .WithMany(o => o.Applications)
                    .HasForeignKey(a => a.OfficeId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Application -> User (Applicant)
                entity.HasOne(a => a.UserAccount)
                    .WithMany(u => u.ApplicationsApply)
                    .HasForeignKey(a => a.UserAccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Application -> Reviewer
                entity.HasOne(a => a.ReviewedUserAccount)
                    .WithMany(u => u.ApplicationsReviewed)
                    .HasForeignKey(a => a.ReviewedById)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // =========================
            // ApplicationDocuments
            // =========================
            modelBuilder.Entity<ApplicationDocuments>(entity =>
            {

                entity.Property(d => d.UploadedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(d => d.Application)
                    .WithMany(a => a.ApplicationDocuments)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.Cascade); // ✅ يمسح مع الابلكيشن   بعد فتره محدده
            });

            // =========================
            // Appointment
            // =========================
            modelBuilder.Entity<Appointment>(entity =>
            {

                entity.HasOne(a => a.Application)
                    .WithMany(app => app.Appointments)
                    .HasForeignKey(a => a.ApplicationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.ScheduledBy)
                    .WithMany(u => u.Appointments)
                    .HasForeignKey(a => a.ScheduledById)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // =========================
            // TimelineEntry
            // =========================
            modelBuilder.Entity<TimelineEntry>(entity =>
            {
                entity.Property(t => t.Timestamp)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(t => t.Application)
                    .WithMany(a => a.TimelineEntries)
                    .HasForeignKey(t => t.ApplicationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.PerformedBy)
                    .WithMany(u => u.TimelineEntries)
                    .HasForeignKey(t => t.PerformedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
