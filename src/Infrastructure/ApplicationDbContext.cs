using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Infrastructure;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<Nutritionalplan> Nutritionalplans { get; set; }

    public virtual DbSet<Routine> Routines { get; set; }

    public virtual DbSet<Routinesexercise> Routinesexercises { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<Shiftclient> Shiftclients { get; set; }

    public virtual DbSet<Trainer> Trainers { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Dniclient).HasName("PRIMARY");

            entity.ToTable("clients");

            entity.HasIndex(e => e.Iduser, "iduser_fk_idx");

            entity.HasIndex(e => e.Typememberships, "typememberships_idx");

            entity.Property(e => e.Dniclient)
                .HasMaxLength(8)
                .HasColumnName("dniclient");
            entity.Property(e => e.Actualdatemembership)
                .HasColumnType("datetime")
                .HasColumnName("actualdatemembership");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Firstname)
                .HasMaxLength(20)
                .HasColumnName("firstname");
            entity.Property(e => e.Genre)
                .HasMaxLength(45)
                .HasDefaultValueSql("'Sin especificar'")
                .HasColumnName("genre");
            entity.Property(e => e.Iduser).HasColumnName("iduser");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("isactive");
            entity.Property(e => e.Lastname)
                .HasMaxLength(20)
                .HasColumnName("lastname");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(15)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Startdatemembership)
                .HasColumnType("datetime")
                .HasColumnName("startdatemembership");
            entity.Property(e => e.Typememberships)
                .HasMaxLength(20)
                .HasColumnName("typememberships");

            entity.HasOne(d => d.IduserNavigation).WithMany(p => p.Clients)
                .HasForeignKey(d => d.Iduser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("iduser_fk_client");

            entity.HasOne(d => d.TypemembershipsNavigation).WithMany(p => p.Clients)
                .HasForeignKey(d => d.Typememberships)
                .HasConstraintName("typememberships");
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.Idexercise).HasName("PRIMARY");

            entity.ToTable("exercises");

            entity.Property(e => e.Idexercise).HasColumnName("idexercise");
            entity.Property(e => e.Musclegroup)
                .HasMaxLength(45)
                .HasColumnName("musclegroup");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Idlocation).HasName("PRIMARY");

            entity.ToTable("locations");

            entity.Property(e => e.Idlocation).HasColumnName("idlocation");
            entity.Property(e => e.Adress)
                .HasMaxLength(120)
                .HasColumnName("adress");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("isactive");
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.Type).HasName("PRIMARY");

            entity.ToTable("memberships");

            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .HasColumnName("description");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<Nutritionalplan>(entity =>
        {
            entity.HasKey(e => e.Idnutritionalplan).HasName("PRIMARY");

            entity.ToTable("nutritionalplan");

            entity.HasIndex(e => e.Dniclient, "dniclient_fk_np_idx");

            entity.HasIndex(e => e.Dnitrainer, "dnitrainer_fk_np_idx");

            entity.Property(e => e.Idnutritionalplan).HasColumnName("idnutritionalplan");
            entity.Property(e => e.Breakfast)
                .HasMaxLength(200)
                .HasColumnName("breakfast");
            entity.Property(e => e.Brunch)
                .HasMaxLength(200)
                .HasColumnName("brunch");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.Dinner)
                .HasMaxLength(200)
                .HasColumnName("dinner");
            entity.Property(e => e.Dniclient)
                .HasMaxLength(8)
                .HasColumnName("dniclient");
            entity.Property(e => e.Dnitrainer)
                .HasMaxLength(8)
                .HasColumnName("dnitrainer");
            entity.Property(e => e.Height)
                .HasMaxLength(6)
                .HasColumnName("height");
            entity.Property(e => e.Lunch)
                .HasMaxLength(200)
                .HasColumnName("lunch");
            entity.Property(e => e.Snack)
                .HasMaxLength(200)
                .HasColumnName("snack");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .HasColumnName("status");
            entity.Property(e => e.Weight)
                .HasMaxLength(6)
                .HasColumnName("weight");

            entity.HasOne(d => d.DniclientNavigation).WithMany(p => p.Nutritionalplans)
                .HasForeignKey(d => d.Dniclient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dniclient_fk_np");

            entity.HasOne(d => d.DnitrainerNavigation).WithMany(p => p.Nutritionalplans)
                .HasForeignKey(d => d.Dnitrainer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dnitrainer_fk_np");
        });

        modelBuilder.Entity<Routine>(entity =>
        {
            entity.HasKey(e => e.Idroutine).HasName("PRIMARY");

            entity.ToTable("routines");

            entity.HasIndex(e => e.Dniclient, "dniclient_fk_routines_idx");

            entity.HasIndex(e => e.Dnitrainer, "dnitrainer_fk_routines_idx");

            entity.Property(e => e.Idroutine).HasColumnName("idroutine");
            entity.Property(e => e.Days).HasColumnName("days");
            entity.Property(e => e.Description)
                .HasMaxLength(45)
                .HasColumnName("description");
            entity.Property(e => e.Dniclient)
                .HasMaxLength(8)
                .HasColumnName("dniclient");
            entity.Property(e => e.Dnitrainer)
                .HasMaxLength(8)
                .HasColumnName("dnitrainer");
            entity.Property(e => e.Height)
                .HasMaxLength(6)
                .HasColumnName("height");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .HasColumnName("status");
            entity.Property(e => e.Weight)
                .HasMaxLength(6)
                .HasColumnName("weight");

            entity.HasOne(d => d.DniclientNavigation).WithMany(p => p.Routines)
                .HasForeignKey(d => d.Dniclient)
                .HasConstraintName("dniclient_fk_routines");

            entity.HasOne(d => d.DnitrainerNavigation).WithMany(p => p.Routines)
                .HasForeignKey(d => d.Dnitrainer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dnitrainer_fk_routines");
        });

        modelBuilder.Entity<Routinesexercise>(entity =>
        {
            entity.HasKey(e => new { e.Idroutine, e.Idexercise, e.Day })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("routinesexercise");

            entity.HasIndex(e => e.Idexercise, "exercise_fk_re_idx");

            entity.Property(e => e.Idroutine).HasColumnName("idroutine");
            entity.Property(e => e.Idexercise).HasColumnName("idexercise");
            entity.Property(e => e.Day)
                .HasMaxLength(12)
                .HasColumnName("day");
            entity.Property(e => e.Breaktime)
                .HasColumnType("time")
                .HasColumnName("breaktime");
            entity.Property(e => e.Series)
                .HasMaxLength(5)
                .HasColumnName("series");

            entity.HasOne(d => d.IdexerciseNavigation).WithMany(p => p.Routinesexercises)
                .HasForeignKey(d => d.Idexercise)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("exercise_fk_re");

            entity.HasOne(d => d.IdroutineNavigation).WithMany(p => p.Routinesexercises)
                .HasForeignKey(d => d.Idroutine)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("idroutine_fk_re");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Idshift).HasName("PRIMARY");

            entity.ToTable("shift");

            entity.HasIndex(e => e.Dnitrainer, "dnitrainer_fk_shift_idx");

            entity.HasIndex(e => e.Idlocation, "idlocation_fk_shift_idx");

            entity.Property(e => e.Idshift).HasColumnName("idshift");
            entity.Property(e => e.Actualpeople).HasColumnName("actualpeople");
            entity.Property(e => e.Dateday)
                .HasMaxLength(12)
                .HasColumnName("dateday");
            entity.Property(e => e.Dnitrainer)
                .HasMaxLength(8)
                .HasColumnName("dnitrainer");
            entity.Property(e => e.Hour)
                .HasColumnType("time")
                .HasColumnName("hour");
            entity.Property(e => e.Idlocation).HasColumnName("idlocation");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("isActive");
            entity.Property(e => e.Peoplelimit)
                .HasDefaultValueSql("'30'")
                .HasColumnName("peoplelimit");

            entity.HasOne(d => d.DnitrainerNavigation).WithMany(p => p.Shifts)
                .HasForeignKey(d => d.Dnitrainer)
                .HasConstraintName("dnitrainer_fk_shift");

            entity.HasOne(d => d.IdlocationNavigation).WithMany(p => p.Shifts)
                .HasForeignKey(d => d.Idlocation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("idlocation_fk_shift");
        });

        modelBuilder.Entity<Shiftclient>(entity =>
        {
            entity.HasKey(e => e.Idshift).HasName("PRIMARY");

            entity.ToTable("shiftclient");

            entity.HasIndex(e => e.Dniclient, "dniclient_fk_shiftclient_idx");

            entity.Property(e => e.Idshift)
                .ValueGeneratedNever()
                .HasColumnName("idshift");
            entity.Property(e => e.Dniclient)
                .HasMaxLength(8)
                .HasColumnName("dniclient");

            entity.HasOne(d => d.DniclientNavigation).WithMany(p => p.Shiftclients)
                .HasForeignKey(d => d.Dniclient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dniclient_fk_shiftclient");

            entity.HasOne(d => d.IdshiftNavigation).WithOne(p => p.Shiftclient)
                .HasForeignKey<Shiftclient>(d => d.Idshift)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("idshift_fk_shiftclient");
        });

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.HasKey(e => e.Dnitrainer).HasName("PRIMARY");

            entity.ToTable("trainers");

            entity.HasIndex(e => e.Iduser, "iduser_fk_idx");

            entity.Property(e => e.Dnitrainer)
                .HasMaxLength(8)
                .HasColumnName("dnitrainer");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Firstname)
                .HasMaxLength(20)
                .HasColumnName("firstname");
            entity.Property(e => e.Iduser).HasColumnName("iduser");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("isactive");
            entity.Property(e => e.Lastname)
                .HasMaxLength(20)
                .HasColumnName("lastname");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(15)
                .HasColumnName("phonenumber");

            entity.HasOne(d => d.IduserNavigation).WithMany(p => p.Trainers)
                .HasForeignKey(d => d.Iduser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("iduser_fk_trainer");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");
            entity.Property(e => e.Type)
                .HasMaxLength(45)
                .HasColumnName("type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
