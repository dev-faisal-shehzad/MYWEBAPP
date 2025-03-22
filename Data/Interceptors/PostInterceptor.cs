using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyWebApp.Models;

namespace MyWebApp.Data.Interceptors
{
    public class PostInterceptor
    {
        public void Apply(EntityEntry<Post> entry, bool hardDelete = false)
        {
            var post = entry.Entity;
            switch (entry.State)
            {
                case EntityState.Added:
                    SaveOrUpdate(post);
                    Save(post);
                    break;
                case EntityState.Modified:
                    SaveOrUpdate(post);
                    Update(post);
                    break;
                case EntityState.Deleted:
                    Delete(post, hardDelete);
                    break;
            }
        }

        public void SaveOrUpdate(Post post)
        {
            post.Name = post.Name?.Trim();
        }
         public void Save(Post entry) {}
        public void Update(Post entry) {}
        public void Delete(Post entry, bool hardDelete = false) {}
    }
}
