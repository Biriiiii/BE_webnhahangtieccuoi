using BE_webnhahangtieccuoi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BE_webnhahangtieccuoi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Hall> Halls => Set<Hall>();
    public DbSet<MenuGroup> MenuGroups => Set<MenuGroup>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<MenuPackage> MenuPackages => Set<MenuPackage>();
    public DbSet<MenuPackageItem> MenuPackageItems => Set<MenuPackageItem>();
    public DbSet<GalleryAlbum> GalleryAlbums => Set<GalleryAlbum>();
    public DbSet<GalleryItem> GalleryItems => Set<GalleryItem>();
    public DbSet<Testimonial> Testimonials => Set<Testimonial>();
    public DbSet<News> News => Set<News>();
    public DbSet<Banner> Banners => Set<Banner>();
    public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<LeadMenuSelection> LeadMenuSelections => Set<LeadMenuSelection>();
    public DbSet<LeadHistory> LeadHistories => Set<LeadHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ==== Unique keys ====
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<ServiceCategory>().HasIndex(s => s.Slug).IsUnique();
        modelBuilder.Entity<Service>().HasIndex(s => s.Slug).IsUnique();
        modelBuilder.Entity<News>().HasIndex(n => n.Slug).IsUnique();
        modelBuilder.Entity<SiteSetting>().HasIndex(s => s.Key).IsUnique();

        // ==== Decimal precision ====
        modelBuilder.Entity<Service>().Property(s => s.BasePrice).HasColumnType("decimal(18,0)");
        modelBuilder.Entity<Hall>().Property(h => h.AreaSqm).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<MenuItem>().Property(m => m.UnitPrice).HasColumnType("decimal(18,0)");
        modelBuilder.Entity<MenuPackage>().Property(m => m.PricePerTray).HasColumnType("decimal(18,0)");
        modelBuilder.Entity<LeadMenuSelection>().Property(m => m.UnitPrice).HasColumnType("decimal(18,0)");
        modelBuilder.Entity<LeadMenuSelection>().Property(m => m.LineTotal).HasColumnType("decimal(18,0)");
        modelBuilder.Entity<Lead>().Property(l => l.EstimatedTotal).HasColumnType("decimal(18,0)");

        // ==== Relationships: tránh multiple cascade paths (SQL Server) ====
        modelBuilder.Entity<Lead>()
            .HasOne(l => l.ServiceCategory)
            .WithMany(sc => sc.Leads)
            .HasForeignKey(l => l.ServiceCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Lead>()
            .HasOne(l => l.Service)
            .WithMany()
            .HasForeignKey(l => l.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Lead>()
            .HasOne(l => l.AssignedStaff)
            .WithMany(u => u.AssignedLeads)
            .HasForeignKey(l => l.AssignedStaffId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LeadMenuSelection>()
            .HasOne(s => s.Lead)
            .WithMany(l => l.MenuSelections)
            .HasForeignKey(s => s.LeadId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LeadMenuSelection>()
            .HasOne(s => s.MenuItem)
            .WithMany(mi => mi.LeadMenuSelections)
            .HasForeignKey(s => s.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LeadMenuSelection>()
            .HasOne(s => s.MenuPackage)
            .WithMany(mp => mp.LeadMenuSelections)
            .HasForeignKey(s => s.MenuPackageId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LeadHistory>()
            .HasOne(h => h.Lead)
            .WithMany(l => l.Histories)
            .HasForeignKey(h => h.LeadId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LeadHistory>()
            .HasOne(h => h.ChangedByUser)
            .WithMany()
            .HasForeignKey(h => h.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MenuPackageItem>()
            .HasOne(pi => pi.MenuPackage)
            .WithMany(p => p.MenuPackageItems)
            .HasForeignKey(pi => pi.MenuPackageId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MenuPackageItem>()
            .HasOne(pi => pi.MenuItem)
            .WithMany(mi => mi.MenuPackageItems)
            .HasForeignKey(pi => pi.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GalleryItem>()
            .HasOne(gi => gi.GalleryAlbum)
            .WithMany(ga => ga.Items)
            .HasForeignKey(gi => gi.GalleryAlbumId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
