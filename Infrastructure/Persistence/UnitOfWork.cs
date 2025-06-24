namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly SmartHomeContext _context;

    public UnitOfWork(SmartHomeContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
