using System;
using System.Collections.Generic;
using Infrastructure.Models;
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

    public virtual DbSet<Routine> Routines { get; set; }

    public virtual DbSet<Routinesexercise> Routinesexercises { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<Shiftsclient> Shiftsclients { get; set; }

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
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Firstname)
                .HasMaxLength(20)
                .HasColumnName("firstname");
            entity.Property(e => e.Iduser).HasColumnName("iduser");
            entity.Property(e => e.Lastname)
                .HasMaxLength(20)
                .HasColumnName("lastname");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(15)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Startdatemembership)
                .HasColumnType("datetime")
                .HasColumnName("startdatemembership");
            entity.Property(e => e.Statusmembership)
                .HasMaxLength(45)
                .HasColumnName("statusmembership");
            entity.Property(e => e.Typememberships)
                .HasMaxLength(20)
                .HasDefaultValueSql("'standar'")
                .HasColumnName("typememberships");

            entity.HasOne(d => d.IduserNavigation).WithMany(p => p.Clients)
                .HasForeignKey(d => d.Iduser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("iduser_fk_client");

            entity.HasOne(d => d.TypemembershipsNavigation).WithMany(p => p.Clients)
                .HasForeignKey(d => d.Typememberships)
                .OnDelete(DeleteBehavior.ClientSetNull)
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
                .HasMaxLength(20)
                .HasColumnName("adress");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
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

        modelBuilder.Entity<Routine>(entity =>
        {
            entity.HasKey(e => new { e.Correlativenumber, e.Dniclient, e.Dnitrainer })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("routines");

            entity.HasIndex(e => e.Dniclient, "dniclient_fk_routines_idx");

            entity.HasIndex(e => e.Dnitrainer, "dnitrainer_fk_routines_idx");

            entity.Property(e => e.Correlativenumber)
                .ValueGeneratedOnAdd()
                .HasColumnName("correlativenumber");
            entity.Property(e => e.Dniclient)
                .HasMaxLength(8)
                .HasColumnName("dniclient");
            entity.Property(e => e.Dnitrainer)
                .HasMaxLength(8)
                .HasColumnName("dnitrainer");
            entity.Property(e => e.Description)
                .HasMaxLength(45)
                .HasColumnName("description");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.State)
                .HasMaxLength(15)
                .HasColumnName("state");
            entity.Property(e => e.Weight).HasColumnName("weight");

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
            entity.HasKey(e => new { e.Correlativenumber, e.Dniclient, e.Dnitrainer, e.Idexercise })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0 });

            entity.ToTable("routinesexercises");

            entity.HasIndex(e => e.Dniclient, "dniclient_fk_re_idx");

            entity.HasIndex(e => e.Dnitrainer, "dnitrainer_fk_re_idx");

            entity.HasIndex(e => e.Idexercise, "idexercise_fk_re_idx");

            entity.Property(e => e.Correlativenumber).HasColumnName("correlativenumber");
            entity.Property(e => e.Dniclient)
                .HasMaxLength(45)
                .HasColumnName("dniclient");
            entity.Property(e => e.Dnitrainer)
                .HasMaxLength(45)
                .HasColumnName("dnitrainer");
            entity.Property(e => e.Idexercise).HasColumnName("idexercise");
            entity.Property(e => e.Breaktime).HasColumnName("breaktime");
            entity.Property(e => e.Serie).HasColumnName("serie");

            entity.HasOne(d => d.IdexerciseNavigation).WithMany(p => p.Routinesexercises)
                .HasForeignKey(d => d.Idexercise)
                .HasConstraintName("idexercise_fk_re");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Idshift).HasName("PRIMARY");

            entity.ToTable("shifts");

            entity.HasIndex(e => e.Dnitrainer, "dnitrainer_idx");

            entity.HasIndex(e => e.Idlocation, "idlocation_idx");

            entity.Property(e => e.Idshift).HasColumnName("idshift");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Dnitrainer)
                .HasMaxLength(8)
                .HasColumnName("dnitrainer");
            entity.Property(e => e.Idlocation).HasColumnName("idlocation");
            entity.Property(e => e.Peoplelimit)
                .HasDefaultValueSql("'30'")
                .HasColumnName("peoplelimit");

            entity.HasOne(d => d.DnitrainerNavigation).WithMany(p => p.Shifts)
                .HasForeignKey(d => d.Dnitrainer)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("dnitrainer");

            entity.HasOne(d => d.IdlocationNavigation).WithMany(p => p.Shifts)
                .HasForeignKey(d => d.Idlocation)
                .HasConstraintName("idlocation");
        });

        modelBuilder.Entity<Shiftsclient>(entity =>
        {
            entity.HasKey(e => new { e.Dniclient, e.Idshift, e.Date })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("shiftsclients");

            entity.HasIndex(e => e.Idshift, "idshift_idx");

            entity.Property(e => e.Dniclient)
                .HasMaxLength(8)
                .HasColumnName("dniclient");
            entity.Property(e => e.Idshift).HasColumnName("idshift");
            entity.Property(e => e.Date).HasColumnName("date");

            entity.HasOne(d => d.DniclientNavigation).WithMany(p => p.Shiftsclients)
                .HasForeignKey(d => d.Dniclient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dniclient_fk");

            entity.HasOne(d => d.IdshiftNavigation).WithMany(p => p.Shiftsclients)
                .HasForeignKey(d => d.Idshift)
                .HasConstraintName("idshift_fk");
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
