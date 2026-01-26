using Microsoft.EntityFrameworkCore;

namespace TeamPro1.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamRequest> TeamRequests { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<ProjectProgress> ProjectProgresses { get; set; }
        public DbSet<TeamMeeting> TeamMeetings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Team relationships with NoAction to avoid cascade cycles
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Student1)
                .WithMany()
                .HasForeignKey(t => t.Student1Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Student2)
                .WithMany()
                .HasForeignKey(t => t.Student2Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure TeamRequest relationships with NoAction to avoid cascade cycles
            modelBuilder.Entity<TeamRequest>()
                .HasOne(tr => tr.Sender)
                .WithMany()
                .HasForeignKey(tr => tr.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TeamRequest>()
                .HasOne(tr => tr.Receiver)
                .WithMany()
                .HasForeignKey(tr => tr.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure ProjectProgress relationships
            modelBuilder.Entity<ProjectProgress>()
                .HasOne(pp => pp.Team)
                .WithMany()
                .HasForeignKey(pp => pp.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectProgress>()
                .HasOne(pp => pp.AssignedFaculty)
                .WithMany()
                .HasForeignKey(pp => pp.AssignedFacultyId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure TeamMeeting relationships
            modelBuilder.Entity<TeamMeeting>()
                .HasOne(tm => tm.Team)
                .WithMany()
                .HasForeignKey(tm => tm.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


