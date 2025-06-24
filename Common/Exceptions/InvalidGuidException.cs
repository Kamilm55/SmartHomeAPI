namespace Smart_Home_IoT_Device_Management_API.Common.Exceptions;

public class InvalidGuidException : Exception
{
    public InvalidGuidException(string value)
        : base($"The value '{value}' is not a valid GUID. Therefore not found entity with id '{value}'")
    {
    }
}