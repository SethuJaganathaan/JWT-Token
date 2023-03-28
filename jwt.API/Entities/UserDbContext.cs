using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace jwt.API.Entities;

public partial class UserDbContext : DbContext
{
    public UserDbContext()
    {
    }

    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();

            var connectionString = configuration.GetConnectionString("constring");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Mailid).HasName("PK__users__F5CE7C80C51CE2F3");

            entity.ToTable("users");

            entity.Property(e => e.Mailid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("mailid");
            entity.Property(e => e.Passwords)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("passwords");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
