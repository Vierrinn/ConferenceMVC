using System;
using System.Collections.Generic;
using ConferenceDomain.Model;
using Microsoft.EntityFrameworkCore;

//namespace ConferenceDomain.Model;
namespace ConferenceInfrastructure;
public partial class DbconferenceContext : DbContext
{
    public DbconferenceContext()
    {
    }

    public DbconferenceContext(DbContextOptions<DbconferenceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Conference> Conferences { get; set; }

    public virtual DbSet<Organizer> Organizers { get; set; }

    public virtual DbSet<SignUp> SignUps { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-B8OBOTM\\SQLEXPRESS; Database=DBConference; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conference>(entity =>
        {
            entity.ToTable("Conference");

            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.Info).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Organizer).WithMany(p => p.Conferences)
                .HasForeignKey(d => d.OrganizerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Conference_Organizer");

            entity.HasOne(d => d.Topic).WithMany(p => p.Conferences)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Conference_Topic");
        });

        modelBuilder.Entity<Organizer>(entity =>
        {
            entity.ToTable("Organizer");

            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        modelBuilder.Entity<SignUp>(entity =>
        {
            entity.ToTable("SignUp");

            entity.HasOne(d => d.Conference).WithMany(p => p.SignUps)
                .HasForeignKey(d => d.ConferenceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SignUp_Conference");

            entity.HasOne(d => d.User).WithMany(p => p.SignUps)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SignUp_User");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.ToTable("Topic");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
