using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public interface IEntityInterceptor<T> where T : class
{
    void Apply(EntityEntry<T> entry, bool hardDelete = false);
    void SaveOrUpdate(EntityEntry<T> entry);
    void Save(EntityEntry<T> entry);
    void Update(EntityEntry<T> entry);
    void Delete(EntityEntry<T> entry, bool hardDelete = false);
}
