using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyWebApp.Models;

namespace MyWebApp.Data.Interceptors
{
    public class UserInterceptor
    {

        public void Apply(EntityEntry<User> entry, bool hardDelete = false)
        {
            var user = entry.Entity;
            switch (entry.State)
            {
                case EntityState.Added:
                    SaveOrUpdate(user);
                    Save(user);
                    break;
                case EntityState.Modified:
                    SaveOrUpdate(user);
                    Update(user);
                    break;
                case EntityState.Deleted:
                    Delete(user, hardDelete);
                    break;
            }
        }


        public void SaveOrUpdate(User user)
        {
            user.UserName ??= $"{user.FirstName.ToLower()}_{user.LastName.ToLower()}_{user.Id}";
        }
        public void Save(User user) {}
        public void Update(User user) {}
        public void Delete(User user, bool hardDelete = false) {}
    }
}
