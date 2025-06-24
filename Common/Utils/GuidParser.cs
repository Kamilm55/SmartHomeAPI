using Smart_Home_IoT_Device_Management_API.Common.Exceptions;

namespace Smart_Home_IoT_Device_Management_API.Common.Utils;


public static class GuidParser
{
    public static Guid Parse(string id, string entityName)
    {
        if (!Guid.TryParse(id, out var guid))
            throw new InvalidGuidException(id,entityName );

        return guid;
    }

    public static bool TryParse(string id, out Guid guid)
    {
        return Guid.TryParse(id, out guid);
    }
}