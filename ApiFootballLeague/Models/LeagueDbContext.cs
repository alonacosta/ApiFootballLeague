using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiFootballLeague.Models;

public partial class LeagueDbContext : DbContext
{
    public LeagueDbContext()
    {
    }

    public LeagueDbContext(DbContextOptions<LeagueDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Function> Functions { get; set; }

    public virtual DbSet<Incident> Incidents { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Round> Rounds { get; set; }

    public virtual DbSet<StaffMember> StaffMembers { get; set; }

    public virtual DbSet<Sysdiagram> Sysdiagrams { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=League_db.mssql.somee.com;Initial Catalog=League_db;User ID=alionamytkevych_SQLLogin_1;Password=Silvaecosta74@;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            //entity.HasNoKey();
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasMaxLength(450);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.RoleId).HasMaxLength(450);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            //entity.HasNoKey();

            //entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            //entity.Property(e => e.Id).HasMaxLength(450);
            entity.Property(e => e.LastName).HasMaxLength(50);
            //entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            //entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            //entity.Property(e => e.UserName).HasMaxLength(256);
            entity.Property(e => e.ImageId);
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.LoginProvider).HasMaxLength(450);
            entity.Property(e => e.ProviderKey).HasMaxLength(450);
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<AspNetUserRole>(entity =>
        {
            //entity.HasNoKey();
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.Property(e => e.RoleId).HasMaxLength(450);
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne<AspNetUser>()
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .IsRequired();

            entity.HasOne<IdentityRole>()
                  .WithMany()
                  .HasForeignKey(e => e.RoleId)
                  .IsRequired();

            entity.HasOne<AspNetRole>(ur => ur.Role)
                 .WithMany()
                 .HasForeignKey(ur => ur.RoleId)
                 .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.LoginProvider).HasMaxLength(450);
            entity.Property(e => e.Name).HasMaxLength(450);
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Function>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.NamePosition).HasMaxLength(450);
        });

        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.OccurenceName).HasMaxLength(50);
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(m => m.Round) 
                .WithMany(r => r.Matches)        
                .HasForeignKey(m => m.RoundId) 
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id); 

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();            

            entity.HasOne(e => e.Club) 
                .WithMany(c => c.Players)
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict); 

            entity.HasOne(e => e.Position) 
                .WithMany()
                .HasForeignKey(e => e.PositionId)
                .OnDelete(DeleteBehavior.Restrict);
           
            //entity.HasNoKey();

            //entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id); 

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();
        });

        modelBuilder.Entity<Round>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<StaffMember>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<Sysdiagram>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("sysdiagrams");

            entity.Property(e => e.Definition).HasColumnName("definition");
            entity.Property(e => e.DiagramId).HasColumnName("diagram_id");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
            entity.Property(e => e.PrincipalId).HasColumnName("principal_id");
            entity.Property(e => e.Version).HasColumnName("version");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
