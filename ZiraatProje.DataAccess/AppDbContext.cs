using Microsoft.EntityFrameworkCore;

namespace ZiraatProje.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<ShiftType> ShiftTypes { get; set; } = null!;
        public DbSet<Shift> Shifts { get; set; } = null!;
        public DbSet<Leave> Leaves { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<ProjectAllocation> ProjectAllocations { get; set; } = null!;
        public DbSet<ProjectMonthlyCost> ProjectMonthlyCosts { get; set; } = null!;
        public DbSet<MonthlyReleaseShift> MonthlyReleaseShifts { get; set; } = null!;
        public DbSet<CustomShift> CustomShifts { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public DbSet<ChatGroup> ChatGroups { get; set; } = null!;

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Targeting local MSSQLLocalDB database ZiraatProjeDb
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=ZiraatProjeDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration for Teams
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TeamName).IsRequired().HasMaxLength(100);
            });

            // Configuration for Users
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Surname).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Team).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(50);
                entity.Property(e => e.IsAdmin).IsRequired();

                entity.Ignore(e => e.PhoneFormatted);
                entity.Ignore(e => e.DisplayTeamName);
                entity.Ignore(e => e.HasTeam);
                entity.Ignore(e => e.SubTitleDotTeam);
            });

            // Configuration for ShiftTypes
            modelBuilder.Entity<ShiftType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ShiftName).IsRequired().HasMaxLength(100);
            });

            // Configuration for Shifts
            modelBuilder.Entity<Shift>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ShiftDate).IsRequired();
                entity.Property(e => e.JiraTicketNo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ExternalLink).HasMaxLength(500);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Shifts)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ShiftType)
                      .WithMany(s => s.Shifts)
                      .HasForeignKey(e => e.ShiftTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuration for Leaves
            modelBuilder.Entity<Leave>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
                entity.Property(e => e.Year).IsRequired();
                entity.Property(e => e.Quarter).IsRequired();

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Leaves)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuration for Projects
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProjectName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.ProjectStatus).IsRequired().HasMaxLength(30);
                entity.Property(e => e.Team).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TotalManDayBudget).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
            });

            // Configuration for ProjectAllocations
            modelBuilder.Entity<ProjectAllocation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AllocatedManDay).HasColumnType("decimal(18,2)").IsRequired();

                entity.HasOne(e => e.Project)
                      .WithMany(p => p.ProjectAllocations)
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.ProjectAllocations)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuration for MonthlyReleaseShifts
            modelBuilder.Entity<MonthlyReleaseShift>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MonthName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ReleaseDate).IsRequired();
                entity.Property(e => e.AssignedUsers).IsRequired();
                entity.Property(e => e.JiraTicketNo).HasMaxLength(100);
                entity.Property(e => e.CreatedByUserName).HasMaxLength(200);
                entity.Property(e => e.UpdatedByUserName).HasMaxLength(200);
            });

            // Configuration for CustomShifts
            modelBuilder.Entity<CustomShift>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Topic).IsRequired().HasMaxLength(200);
                entity.Property(e => e.AssignedUsers).IsRequired();
                entity.Property(e => e.ShiftDate).IsRequired();
                entity.Property(e => e.CreatedByUserName).HasMaxLength(200);
                entity.Property(e => e.UpdatedByUserName).HasMaxLength(200);
            });

            // Configuration for ChatMessages
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MessageText).IsRequired();
                entity.Property(e => e.TargetType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TargetTeam).HasMaxLength(50);
                entity.Property(e => e.SentAt).IsRequired();

                entity.HasOne(e => e.SenderUser)
                    .WithMany()
                    .HasForeignKey(e => e.SenderUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ReceiverUser)
                    .WithMany()
                    .HasForeignKey(e => e.ReceiverUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
