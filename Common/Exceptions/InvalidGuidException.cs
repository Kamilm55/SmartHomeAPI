namespace Smart_Home_IoT_Device_Management_API.Common.Exceptions;

public class InvalidGuidException : Exception
{
    public InvalidGuidException(string value, string entityName)
        : base($"The value '{value}' is not a valid GUID. Therefore not found {entityName} with id '{value}'")
    {
    }
}