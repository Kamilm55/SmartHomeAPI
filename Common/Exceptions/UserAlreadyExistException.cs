namespace Smart_Home_IoT_Device_Management_API.Common.Exceptions;

public class UserAlreadyExistException : Exception
{
    public UserAlreadyExistException(string key,string value)
        : base($"The User is already exist {key}: {value}")
    {
    }
}