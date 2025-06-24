namespace Smart_Home_IoT_Device_Management_API.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string key,Guid id)
        : base($"{key} not found with id:{id.ToString()}")
    {
        
    }
    
    public NotFoundException(string msg)
        : base(msg)
    {
        
    }
}