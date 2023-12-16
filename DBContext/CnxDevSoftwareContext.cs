using System;
using System.Collections.Generic;
using cnxdevsoftware.Models;
using Microsoft.EntityFrameworkCore;

namespace cnxdevsoftware.Context;

public partial class CnxDevSoftwareContext : DbContext
{
    public CnxDevSoftwareContext()
    {
    }

    public CnxDevSoftwareContext(DbContextOptions<CnxDevSoftwareContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ItemMaster> ItemMasters { get; set; }
    public virtual DbSet<AccessToken> AccessTokens { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemMaster>(entity =>
        {
            entity.HasKey(e => e.ItemCode);

            entity.ToTable("ItemMaster");

            entity.Property(e => e.ItemCode)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.ItemImage).HasColumnType("image");
            entity.Property(e => e.ItemName).HasMaxLength(100);
        });

        modelBuilder.Entity<AccessToken>(entity =>
        {
            entity.ToTable("AccessToken");

            entity.Property(e => e.AccessToken1).HasColumnName("access_token");
            entity.Property(e => e.ExpiresIn).HasColumnName("expires_in");
            entity.Property(e => e.LastUpdate).HasColumnName("last_update");
            entity.Property(e => e.TokenType)
                .HasMaxLength(100)
                .HasColumnName("token_type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
