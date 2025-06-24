namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Security;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string hashedPassword, string inputPassword);
}