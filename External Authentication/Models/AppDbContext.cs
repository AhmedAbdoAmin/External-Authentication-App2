using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace External_Authentication.Models
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolesHasPermission> RolesHasPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>()
                .HasIndex(p => p.Title)
                .IsUnique();

            modelBuilder.Entity<RolesHasPermission>()
             .HasKey(ub => new { ub.Permission_Id, ub.RoleId });

            modelBuilder.Entity<RolesHasPermission>()
                .HasOne(ub => ub.ManagerUserRole)
                .WithMany(au => au.RolesHasPermissions)
                .HasForeignKey(ub => ub.RoleId);

            modelBuilder.Entity<RolesHasPermission>()
               .HasOne(ub => ub.Permission)
               .WithMany(au => au.RolesHasPermissions)
               .HasForeignKey(ub => ub.Permission_Id);

            base.OnModelCreating(modelBuilder);

        }
    }
}
