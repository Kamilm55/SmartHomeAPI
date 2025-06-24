namespace Smart_Home_IoT_Device_Management_API.Common.Exceptions;

public class InvalidUsernameOrPasswordException
{
    
}
public class InvalidEmailOrPasswordException : Exception
{
    public InvalidEmailOrPasswordException(string msg)
        : base(msg)
    {
        
    }
}