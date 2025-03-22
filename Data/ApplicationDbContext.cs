using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyWebApp.Models;
using MyWebApp.Data.Interceptors;
using System.ComponentModel.DataAnnotations; 


namespace MyWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        private readonly UserInterceptor _userInterceptor = new();
        private readonly PostInterceptor _postInterceptor = new();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) {}

        public DbSet<Post> Posts { get; set; }

        public bool HardDeleteMode { get; set; } = false;
        public bool validate { get; set; } = true;

        public override int SaveChanges()
        {
            ApplyInterceptors(ChangeTracker);
            ApplyTimestamps();
            if (HardDeleteMode == false)
            {
                HandleSoftDelete();
            }
            return base.SaveChanges();
        }

        private void ApplyInterceptors(ChangeTracker changeTracker, bool hardDelete = false)
        {
            foreach (var entry in changeTracker.Entries<User>())
            {
                _userInterceptor.Apply(entry, hardDelete);
            }

            foreach (var entry in changeTracker.Entries<Post>())
            {
                _postInterceptor.Apply(entry, hardDelete);
            }
        }

        private void ApplyTimestamps()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Modified && entry.Properties.Any(p => p.Metadata.Name == "UpdatedAt"))
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }
        }

        private void HandleSoftDelete()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Deleted && entry.Properties.Any(p => p.Metadata.Name == "IsDeleted"))
                {
                    entry.Property("IsDeleted").CurrentValue = true;
                    entry.Property("DeletedAt").CurrentValue = DateTime.UtcNow;
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void HardDelete<T>(T entity) where T : class
        {
            HardDeleteMode = true;
            Remove(entity);
            base.SaveChanges();
            HardDeleteMode = false;
        }

    }
}
