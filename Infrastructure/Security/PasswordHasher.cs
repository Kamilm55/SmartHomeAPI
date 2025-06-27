using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Smart_Home_IoT_Device_Management_API.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        // generate a 128-bit salt using a secure PRNG
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100_000,
            numBytesRequested: 32);

        // Combine salt + hash and convert to base64
        byte[] result = new byte[1 + salt.Length + hash.Length];
        result[0] = 0x01; // format marker
        Buffer.BlockCopy(salt, 0, result, 1, salt.Length);
        Buffer.BlockCopy(hash, 0, result, 1 + salt.Length, hash.Length);

        return Convert.ToBase64String(result);
    }

    public bool Verify( string inputPassword, string hashedPassword)
    {
        byte[] decoded = Convert.FromBase64String(hashedPassword);

        if (decoded[0] != 0x01)
            throw new FormatException("Invalid hashed password format");

        byte[] salt = new byte[16];
        Buffer.BlockCopy(decoded, 1, salt, 0, 16);

        byte[] expectedHash = new byte[32];
        Buffer.BlockCopy(decoded, 1 + 16, expectedHash, 0, 32);

        byte[] actualHash = KeyDerivation.Pbkdf2(
            password: inputPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100_000,
            numBytesRequested: 32);

        return CryptographicOperations.FixedTimeEquals(expectedHash, actualHash);
    }
}