using System.Security.Cryptography;

namespace EntityFrameworkCore.ColumnLevelEncryption.Util;

/// <summary>
/// Provides functionality to generate a random base64 encoded string.
/// </summary>
public class Base64Generator
{
    /// <summary>
    /// Generates a random base64 encoded string of the specified byte length.
    /// </summary>
    /// <param name="byteLength">The length of the byte array to generate. Default is 16.</param>
    /// <returns>A base64 encoded string representing the random bytes.</returns>
    public static string GenerateBase64String(int byteLength = 16)
    {
        byte[] randomBytes = new byte[byteLength];
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}