using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace StudentGroup_Repository.Models;

public partial class Prn231Su23StudentGroupDbContext : DbContext
{

    public Prn231Su23StudentGroupDbContext()
    {
        
    }

    public Prn231Su23StudentGroupDbContext(DbContextOptions<Prn231Su23StudentGroupDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentGroup> StudentGroups { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];

        return strConn;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student__3214EC0781944B92");

            entity.ToTable("Student");

            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FullName).HasMaxLength(250);

            entity.HasOne(d => d.Group).WithMany(p => p.Students)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK__Student__GroupId__3B75D760");
        });

        modelBuilder.Entity<StudentGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StudentG__3214EC07C1F90F42");

            entity.ToTable("StudentGroup");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.GroupName).HasMaxLength(250);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC07E242E969");

            entity.ToTable("UserRole");

            entity.HasIndex(e => e.Username, "UQ__UserRole__536C85E43CA08C1E").IsUnique();

            entity.Property(e => e.Passphrase)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UserRole1).HasColumnName("UserRole");
            entity.Property(e => e.Username).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
