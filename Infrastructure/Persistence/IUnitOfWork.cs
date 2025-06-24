namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}