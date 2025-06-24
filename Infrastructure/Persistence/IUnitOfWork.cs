namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;

public interface IUnitOfWork
{
    // Task<T> SaveChangesAsyncAndReturnLatestEntity<T>(T entity);

    Task<int> SaveChangesAsync();
}