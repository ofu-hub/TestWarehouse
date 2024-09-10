using Microsoft.EntityFrameworkCore;
using TestWarehouse.Models;

namespace TestWarehouse;

public class DatabaseContext : DbContext
{
    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    /// <param name="options">Базовые настройки бд</param>
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    /// <summary>
    /// Коллекция коробок
    /// </summary>
    public DbSet<Box> Boxes { get; set; }
    
    /// <summary>
    /// Коллекция паллетов
    /// </summary>
    public DbSet<Pallet> Pallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Box>()
            .HasOne(b => b.Pallet)
            .WithMany(p => p.Boxes)
            .HasForeignKey(b => b.PalletId)
            .OnDelete(DeleteBehavior.Cascade); // Если удален паллет, то и его коробки удаляются

        base.OnModelCreating(modelBuilder);
    }
}