using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MangoCard.Models;


public class AppDbContext : IdentityDbContext<Company>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<WalletCard> WalletCards { get; set; }
    public DbSet<Store> Stores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Company Configuration
        modelBuilder.Entity<Company>()
            .Property(c => c.MonthlyFee)
            .HasColumnType("decimal(18,2)");

        // WalletCard Configuration
        modelBuilder.Entity<WalletCard>()
            .HasOne(wc => wc.Company)
            .WithMany(c => c.WalletCards)
            .HasForeignKey(wc => wc.CompanyId); // Fremdschlüssel vom Typ string

        // Store Configuration
        modelBuilder.Entity<Store>()
            .HasOne(s => s.Company)
            .WithMany(c => c.Stores)
            .HasForeignKey(s => s.CompanyId); // Fremdschlüssel vom Typ string

        modelBuilder.Entity<WalletCard>()
            .HasKey(sp => sp.WalletId); // Definiert PlanId als Primärschlüssel

    }
}
