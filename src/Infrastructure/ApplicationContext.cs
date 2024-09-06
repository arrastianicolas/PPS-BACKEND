using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<NutritionalPlan> NutritionalPlans { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<RoutineExercise> RoutineExercises { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<ShiftClient> ShiftClients { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Exercise> Exercises { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
 : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Client configuration
            modelBuilder.Entity<Client>()
                .HasOne(c => c.Membership)
                .WithMany()
                .HasForeignKey(c => c.MembershipId);

            // NutritionalPlan configuration - Composite Primary Key
            modelBuilder.Entity<NutritionalPlan>()
                .HasKey(np => new { np.TrainerId, np.ClientId, np.CorrelativeNumber });

            modelBuilder.Entity<NutritionalPlan>()
                .HasOne(np => np.Trainer)
                .WithMany()
                .HasForeignKey(np => np.TrainerId);

            modelBuilder.Entity<NutritionalPlan>()
                .HasOne(np => np.Client)
                .WithMany()
                .HasForeignKey(np => np.ClientId);

            // Routine configuration - Composite Primary Key
            modelBuilder.Entity<Routine>()
                .HasKey(r => new { r.TrainerId, r.ClientId, r.CorrelativeNumber });

            modelBuilder.Entity<Routine>()
                .HasOne(r => r.Trainer)
                .WithMany()
                .HasForeignKey(r => r.TrainerId);

            modelBuilder.Entity<Routine>()
                .HasOne(r => r.Client)
                .WithMany()
                .HasForeignKey(r => r.ClientId);

            // RoutineExercise configuration - Composite Primary Key
            modelBuilder.Entity<RoutineExercise>()
                .HasKey(re => new { re.ExerciseId, re.TrainerId, re.ClientId, re.CorrelativeNumber });

            modelBuilder.Entity<RoutineExercise>()
                .HasOne(re => re.Exercise)
                .WithMany()
                .HasForeignKey(re => re.ExerciseId);

            modelBuilder.Entity<RoutineExercise>()
                .HasOne(re => re.Routine)
                .WithMany()
                .HasForeignKey(re => new { re.TrainerId, re.ClientId, re.CorrelativeNumber });

            // Shift configuration - Composite Primary Key
            modelBuilder.Entity<Shift>()
                .HasKey(s => new { s.LocationId, s.Hour, s.Day });

            modelBuilder.Entity<Shift>()
                .HasOne(s => s.Trainer)
                .WithMany()
                .HasForeignKey(s => s.TrainerId);

            modelBuilder.Entity<Shift>()
                .HasOne(s => s.Location)
                .WithMany()
                .HasForeignKey(s => s.LocationId);

            // ShiftClient configuration - Composite Primary Key
            modelBuilder.Entity<ShiftClient>()
                .HasKey(sc => new { sc.ClientId, sc.LocationId, sc.Hour, sc.Day });

            modelBuilder.Entity<ShiftClient>()
                .HasOne(sc => sc.Client)
                .WithMany()
                .HasForeignKey(sc => sc.ClientId);

            modelBuilder.Entity<ShiftClient>()
                .HasOne(sc => sc.Shift)
                .WithMany()
                .HasForeignKey(sc => new { sc.LocationId, sc.Hour, sc.Day });
        }
    }
}