using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StripsDL.Models;

public partial class StripsDbContext : DbContext
{
    public StripsDbContext()
    {
    }

    public StripsDbContext(DbContextOptions<StripsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auteur> Auteurs { get; set; }

    public virtual DbSet<Reeksen> Reeksens { get; set; }

    public virtual DbSet<Strip> Strips { get; set; }

    public virtual DbSet<Uitgeverijen> Uitgeverijens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=MSI\\SQLEXPRESS01;Initial Catalog=StripsDb;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Auteurs__3214EC07235E6F70");

            entity.HasIndex(e => e.Email, "UQ__Auteurs__A9D105340A3BF6DD").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Naam).HasMaxLength(100);
        });

        modelBuilder.Entity<Reeksen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reeksen__3214EC07DE03ADF5");

            entity.ToTable("Reeksen");

            entity.HasIndex(e => e.Naam, "UQ__Reeksen__7375E70FB621842E").IsUnique();

            entity.Property(e => e.Naam).HasMaxLength(100);
        });

        modelBuilder.Entity<Strip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Strips__3214EC073C90DBFD");

            entity.Property(e => e.Titel).HasMaxLength(255);

            entity.HasOne(d => d.Reeks).WithMany(p => p.Strips)
                .HasForeignKey(d => d.ReeksId)
                .HasConstraintName("FK__Strips__ReeksId__403A8C7D");

            entity.HasOne(d => d.Uitgeverij).WithMany(p => p.Strips)
                .HasForeignKey(d => d.UitgeverijId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Strips__Uitgever__3F466844");

            entity.HasMany(d => d.Auteurs).WithMany(p => p.Strips)
                .UsingEntity<Dictionary<string, object>>(
                    "StripAuteur",
                    r => r.HasOne<Auteur>().WithMany()
                        .HasForeignKey("AuteurId")
                        .HasConstraintName("FK__StripAute__Auteu__440B1D61"),
                    l => l.HasOne<Strip>().WithMany()
                        .HasForeignKey("StripId")
                        .HasConstraintName("FK__StripAute__Strip__4316F928"),
                    j =>
                    {
                        j.HasKey("StripId", "AuteurId").HasName("PK__StripAut__3561B4D522281DB3");
                        j.ToTable("StripAuteurs");
                    });
        });

        modelBuilder.Entity<Uitgeverijen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Uitgever__3214EC0776164898");

            entity.ToTable("Uitgeverijen");

            entity.Property(e => e.Adres).HasMaxLength(255);
            entity.Property(e => e.Naam).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
