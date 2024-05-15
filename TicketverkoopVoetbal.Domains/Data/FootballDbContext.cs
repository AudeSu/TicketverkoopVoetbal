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

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Seizoen> Seizoens { get; set; }

    public virtual DbSet<Stadion> Stadions { get; set; }

    public virtual DbSet<Stoeltje> Stoeltjes { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Zone> Zones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQL19_VIVES; Database=footballsql; Trusted_Connection=True; TrustServerCertificate=True; MultipleActiveResultSets=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Abonnement>(entity =>
        {
            entity.ToTable("Abonnement");

            entity.Property(e => e.AbonnementId).HasColumnName("AbonnementID");
            entity.Property(e => e.ClubId).HasColumnName("ClubID");
            entity.Property(e => e.GebruikerId)
                .HasMaxLength(450)
                .HasColumnName("GebruikerID");
            entity.Property(e => e.SeizoenId).HasColumnName("SeizoenID");
            entity.Property(e => e.StoeltjeId).HasColumnName("StoeltjeID");

            entity.HasOne(d => d.Club).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Abonnement_Club");

            entity.HasOne(d => d.Gebruiker).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.GebruikerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Abonnement_AspNetUsers");

            entity.HasOne(d => d.Seizoen).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.SeizoenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Abonnement_Seizoen");

            entity.HasOne(d => d.Stoeltje).WithMany(p => p.Abonnements)
                .HasForeignKey(d => d.StoeltjeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Abonnement_Stoeltje");
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

            entity.Property(e => e.Discriminator).HasDefaultValueSql("(N'')");
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

        modelBuilder.Entity<Club>(entity =>
        {
            entity.ToTable("Club");

            entity.Property(e => e.ClubId).HasColumnName("ClubID");
            entity.Property(e => e.LogoPath)
                .HasMaxLength(50)
                .IsFixedLength();
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
            entity.Property(e => e.Datum).HasColumnType("date");
            entity.Property(e => e.SeizoenId).HasColumnName("SeizoenID");
            entity.Property(e => e.StadionId).HasColumnName("StadionID");
            entity.Property(e => e.ThuisploegId).HasColumnName("ThuisploegID");
            entity.Property(e => e.UitploegId).HasColumnName("UitploegID");

            entity.HasOne(d => d.Seizoen).WithMany(p => p.Matches)
                .HasForeignKey(d => d.SeizoenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match_Seizoen");

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

        modelBuilder.Entity<Seizoen>(entity =>
        {
            entity.ToTable("Seizoen");

            entity.Property(e => e.SeizoenId).HasColumnName("SeizoenID");
            entity.Property(e => e.Einddatum).HasColumnType("date");
            entity.Property(e => e.Startdatum).HasColumnType("date");
        });

        modelBuilder.Entity<Stadion>(entity =>
        {
            entity.ToTable("Stadion");

            entity.Property(e => e.StadionId).HasColumnName("StadionID");
            entity.Property(e => e.Adres).HasMaxLength(50);
            entity.Property(e => e.Naam).HasMaxLength(50);
            entity.Property(e => e.Stad).HasMaxLength(50);
        });

        modelBuilder.Entity<Stoeltje>(entity =>
        {
            entity.ToTable("Stoeltje");

            entity.Property(e => e.StoeltjeId).HasColumnName("StoeltjeID");
            entity.Property(e => e.ClubId).HasColumnName("ClubID");
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.StadionId).HasColumnName("StadionID");
            entity.Property(e => e.ZoneId).HasColumnName("ZoneID");

            entity.HasOne(d => d.Club).WithMany(p => p.Stoeltjes)
                .HasForeignKey(d => d.ClubId)
                .HasConstraintName("FK_Stoeltje_Club");

            entity.HasOne(d => d.Match).WithMany(p => p.Stoeltjes)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK_Stoeltje_Match");

            entity.HasOne(d => d.Zone).WithMany(p => p.Stoeltjes)
                .HasForeignKey(d => d.ZoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stoeltje_Zone");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.GebruikersId)
                .HasMaxLength(450)
                .HasColumnName("GebruikersID");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.StoeltjeId).HasColumnName("StoeltjeID");
            entity.Property(e => e.ZoneId).HasColumnName("ZoneID");

            entity.HasOne(d => d.Gebruikers).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.GebruikersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_AspNetUsers");

            entity.HasOne(d => d.Match).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Match");

            entity.HasOne(d => d.Stoeltje).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.StoeltjeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Stoeltje");

            entity.HasOne(d => d.Zone).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ZoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Zone1");
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.ToTable("Zone");

            entity.Property(e => e.ZoneId).HasColumnName("ZoneID");
            entity.Property(e => e.Naam)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrijsAbonnement).HasColumnType("money");
            entity.Property(e => e.PrijsTicket).HasColumnType("money");
            entity.Property(e => e.StadionId).HasColumnName("StadionID");

            entity.HasOne(d => d.Stadion).WithMany(p => p.Zones)
                .HasForeignKey(d => d.StadionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Zone_Stadion");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
