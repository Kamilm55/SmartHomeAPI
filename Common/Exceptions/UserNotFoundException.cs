namespace Smart_Home_IoT_Device_Management_API.Common.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string msg)
        : base(msg)
    {
        
    }
}