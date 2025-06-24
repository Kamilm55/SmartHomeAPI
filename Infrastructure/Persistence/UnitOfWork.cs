namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly SmartHomeContext _context;

    public UnitOfWork(SmartHomeContext context)
    {
        _context = context;
    }

    /*public async Task<T> SaveChangesAsyncAndReturnLatestEntity<T>(T entity)
    {
        // Save changes first
        await _context.SaveChangesAsync();
    
        // Reload the entity to get latest data from database
        await _context.Entry(entity).ReloadAsync();
    
        // Also reload related entities if they exist
        var entityEntry = _context.Entry(entity);
        foreach (var navigationEntry in entityEntry.Navigations.Where(n => n.IsLoaded))
        {
            await navigationEntry.LoadAsync();
        }
    
        return entity;
    }*/

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
