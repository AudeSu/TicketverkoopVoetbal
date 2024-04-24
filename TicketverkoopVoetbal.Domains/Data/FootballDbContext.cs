using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketverkoopVoetbal.Domains.Entities;

namespace TicketverkoopVoetbal.Domains.Data;

public partial class FootballDbContext : DbContext
{
    public FootballDbContext()
    {
    }

    public FootballDbContext(DbContextOptions<FootballDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Abonnement> Abonnements { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Bestelling> Bestellings { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Stadion> Stadions { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Zitplaat> Zitplaats { get; set; }

    public virtual DbSet<Zone> Zones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQL19_VIVES; Database=footballsql; Trusted_Connection=True; TrustServerCertificate=True; MultipleActiveResultSets=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Abonnement>(entity =>
        {
            entity.ToTable("Abonnement");

            entity.Property(e => e.AbonnementId)
                .ValueGeneratedNever()
                .HasColumnName("AbonnementID");
            entity.Property(e => e.ClubId).HasColumnName("ClubID");
            entity.Property(e => e.GebruikerId)
                .HasMaxLength(450)
                .HasColumnName("GebruikerID");
            entity.Property(e => e.ZitplaatsId).HasColumnName("ZitplaatsID");

            entity.HasOne(d => d.Club).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Abonnement_Club");

            entity.HasOne(d => d.Gebruiker).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.GebruikerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Abonnement_AspNetUsers");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Bestelling>(entity =>
        {
            entity.ToTable("Bestelling");

            entity.Property(e => e.BestellingId).HasColumnName("BestellingID");
            entity.Property(e => e.Datum).HasColumnType("datetime");
            entity.Property(e => e.GebruikerId)
                .HasMaxLength(450)
                .HasColumnName("GebruikerID");

            entity.HasOne(d => d.Gebruiker).WithMany(p => p.Bestellings)
                .HasForeignKey(d => d.GebruikerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bestelling_AspNetUsers");
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.ToTable("Club");

            entity.Property(e => e.ClubId).HasColumnName("ClubID");
            entity.Property(e => e.Naam)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.ThuisstadionId).HasColumnName("ThuisstadionID");

            entity.HasOne(d => d.Thuisstadion).WithMany(p => p.Clubs)
                .HasForeignKey(d => d.ThuisstadionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Club_Stadion");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.ToTable("Match");

            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.Datum).HasColumnType("datetime");
            entity.Property(e => e.StadionId).HasColumnName("StadionID");
            entity.Property(e => e.ThuisploegId).HasColumnName("ThuisploegID");
            entity.Property(e => e.UitploegId).HasColumnName("UitploegID");

            entity.HasOne(d => d.Stadion).WithMany(p => p.Matches)
                .HasForeignKey(d => d.StadionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match_Stadion");

            entity.HasOne(d => d.Thuisploeg).WithMany(p => p.MatchThuisploegs)
                .HasForeignKey(d => d.ThuisploegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match_Club");

            entity.HasOne(d => d.Uitploeg).WithMany(p => p.MatchUitploegs)
                .HasForeignKey(d => d.UitploegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match_Club1");
        });

        modelBuilder.Entity<Stadion>(entity =>
        {
            entity.ToTable("Stadion");

            entity.Property(e => e.StadionId).HasColumnName("StadionID");
            entity.Property(e => e.Adres).HasMaxLength(50);
            entity.Property(e => e.Naam).HasMaxLength(50);
            entity.Property(e => e.Stad).HasMaxLength(50);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.BestellingId).HasColumnName("BestellingID");
            entity.Property(e => e.GebruikersId)
                .HasMaxLength(450)
                .HasColumnName("GebruikersID");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.ZitplaatsId).HasColumnName("ZitplaatsID");

            entity.HasOne(d => d.Bestelling).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.BestellingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Bestelling");

            entity.HasOne(d => d.Gebruikers).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.GebruikersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_AspNetUsers");

            entity.HasOne(d => d.Match).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Match");

            entity.HasOne(d => d.Zitplaats).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ZitplaatsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Zitplaats");
        });

        modelBuilder.Entity<Zitplaat>(entity =>
        {
            entity.HasKey(e => e.ZitplaatsId);

            entity.Property(e => e.ZitplaatsId).HasColumnName("ZitplaatsID");
            entity.Property(e => e.StadionId).HasColumnName("StadionID");
            entity.Property(e => e.ZoneId).HasColumnName("ZoneID");

            entity.HasOne(d => d.Stadion).WithMany(p => p.Zitplaats)
                .HasForeignKey(d => d.StadionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Zitplaats_Stadion");

            entity.HasOne(d => d.Zone).WithMany(p => p.Zitplaats)
                .HasForeignKey(d => d.ZoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Zitplaats_Zone");
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.ToTable("Zone");

            entity.Property(e => e.ZoneId).HasColumnName("ZoneID");
            entity.Property(e => e.Naam)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Prijs).HasColumnType("money");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
