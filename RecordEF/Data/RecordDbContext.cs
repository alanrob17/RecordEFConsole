using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using RecordEF.Models;

namespace RecordEF.Data;

public partial class RecordDbContext : DbContext
{
    public RecordDbContext()
    {
    }

    public RecordDbContext(DbContextOptions<RecordDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Disc> Discs { get; set; }

    public virtual DbSet<FreeDb> FreeDbs { get; set; }

    public virtual DbSet<Record> Records { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }
            
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["RecordDBContext"].ConnectionString;

        optionsBuilder.UseSqlServer(constr);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.ToTable("Artist");

            entity.Property(e => e.Biography)
                .UseCollation("Latin1_General_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Disc>(entity =>
        {
            entity.ToTable("Disc");

            entity.Property(e => e.FreeDbId)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Record).WithMany(p => p.DiscsNavigation)
                .HasForeignKey(d => d.RecordId)
                .HasConstraintName("FK_Disc_Record");
        });

        modelBuilder.Entity<FreeDb>(entity =>
        {
            entity.ToTable("FreeDB");

            entity.Property(e => e.Artist)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FreeDbId).HasMaxLength(50);
            entity.Property(e => e.Genre).HasMaxLength(200);
            entity.Property(e => e.OtherFreeDbId).HasMaxLength(200);
            entity.Property(e => e.Record)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Review).HasColumnType("ntext");
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.ToTable("Record");

            entity.Property(e => e.Bought).HasColumnType("datetime");
            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.CoverName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Field)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Label)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Media)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Pressing)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Rating)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Review).HasColumnType("text");

            entity.HasOne(d => d.Artist).WithMany(p => p.Records)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("FK_Record_Artist");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.PitchforkId);

            entity.ToTable("Review");

            entity.Property(e => e.ArtistId).HasDefaultValueSql("((0))");
            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Published).HasColumnType("datetime");
            entity.Property(e => e.RecordId).HasDefaultValueSql("((0))");
            entity.Property(e => e.RecordName).HasMaxLength(200);
            entity.Property(e => e.Review1).HasColumnName("Review");
            entity.Property(e => e.ReviewId).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.ToTable("Track");

            entity.Property(e => e.Extended)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Disc).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.DiscId)
                .HasConstraintName("FK_Track_Disc");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
