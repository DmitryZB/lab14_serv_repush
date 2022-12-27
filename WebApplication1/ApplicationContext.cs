using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

public partial class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<TaxiDepot> TaxiDepots { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<TaxiGroup> TaxiGroups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source = /Users/dimon/Desktop/PROGRAMMING/C#/lab14/WebApplication1/DataBase/AssembliesDB.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaxiGroup>(entity =>
        {
            entity.HasIndex(e => e.TaxiDepotId, "IX_Parts_AssemblyId"); // ПОМЕНЯТЬ //

            entity.HasIndex(e => e.CarId, "IX_Parts_DetailId");

            entity.HasOne(d => d.TaxiDepot).WithMany(p => p.TaxiGroups).HasForeignKey(d => d.TaxiDepotId);

            //entity.HasOne(d => d.Car).WithMany(p => p.TaxiGroups).HasForeignKey(d => d.CarId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
