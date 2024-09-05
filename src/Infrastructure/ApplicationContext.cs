using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Exercise> Exercise { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<NutritionalPlan> NutritionalPlans { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<RoutineExercise> RoutinesExercises { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<ShiftClient> ShiftClients { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
         : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasDiscriminator(u => u.Type);


            modelBuilder.Entity<NutritionalPlan>()
        .HasKey(np => new { np.TrainerId, np.ClientId, np.CorrelativeNumber });

            modelBuilder.Entity<Shift>()
        .HasKey(s => new { s.LocationId, s.Hour, s.Day });

            modelBuilder.Entity<RoutineExercise>()
        .HasKey(re => new { re.ExerciseId, re.RoutineId });

            modelBuilder.Entity<ShiftClient>()
        .HasKey(sc => new { sc.ClientId, sc.ShiftId });

            modelBuilder.Entity<Routine>()
        .HasKey(r => new { r.TrainerId, r.ClientId, r.CorrelativeNumber });

            modelBuilder.Entity<Client>()
        .HasOne(b => b.Membership)
        .WithMany()  
        .HasForeignKey(b => b.MembershipId);
            modelBuilder.Entity<Client>()
        .HasOne(b => b.Membership)
        .WithMany()
        .HasForeignKey(b => b.MembershipId);
            modelBuilder.Entity<NutritionalPlan>()
       .HasOne(b => b.Trainer)
       .WithMany()
       .HasForeignKey(b => b.TrainerId);
            modelBuilder.Entity<NutritionalPlan>()
       .HasOne(b => b.Client)
       .WithMany()
       .HasForeignKey(b => b.ClientId);
            modelBuilder.Entity<ShiftClient>()
       .HasOne(b => b.Client)
       .WithMany()
       .HasForeignKey(b => b.ClientId);
            modelBuilder.Entity<ShiftClient>()
       .HasOne(b => b.Shift)
       .WithMany()
       .HasForeignKey(b => b.ShiftId);
            modelBuilder.Entity<Routine>()
       .HasOne(b => b.Trainer)
       .WithMany()
       .HasForeignKey(b => b.TrainerId);
            modelBuilder.Entity<Routine>()
       .HasOne(b => b.Client)
       .WithMany()
       .HasForeignKey(b => b.ClientId);
            modelBuilder.Entity<RoutineExercise>()
       .HasOne(b => b.Exercise)
       .WithMany()
       .HasForeignKey(b => b.ExerciseId);
            modelBuilder.Entity<RoutineExercise>()
       .HasOne(b => b.Routine)
       .WithMany()
       .HasForeignKey(b => b.RoutineId);







            base.OnModelCreating(modelBuilder);
        }
    }
}
