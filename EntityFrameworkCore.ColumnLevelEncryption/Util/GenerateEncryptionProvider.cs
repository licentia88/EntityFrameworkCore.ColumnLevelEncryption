using System.Security.Cryptography;
using System.Text;
using EntityFrameworkCore.ColumnLevelEncryption.Interfaces;

namespace EntityFrameworkCore.ColumnLevelEncryption.Util;

/// <summary>
/// Provides encryption and decryption services using AES algorithm.
/// </summary>
public class GenerateEncryptionProvider : IEncryptionProvider
{
    private readonly string _key;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateEncryptionProvider"/> class with the specified encryption key.
    /// </summary>
    /// <param name="key">The encryption key to use for encrypting and decrypting data.</param>
    /// <exception cref="ArgumentNullException">Thrown if the encryption key is null or empty.</exception>
    public GenerateEncryptionProvider(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key), "Please initialize your encryption key.");

        _key = key;
    }

    /// <summary>
    /// Encrypts the specified data using AES algorithm.
    /// </summary>
    /// <param name="dataToEncrypt">The data to encrypt.</param>
    /// <returns>The encrypted data as a base64 encoded string.</returns>
    public string Encrypt(string dataToEncrypt)
    {
        if (string.IsNullOrEmpty(dataToEncrypt))
            return string.Empty;

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_key);
        aes.IV = new byte[16]; // Initialization vector with zeros

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using var streamWriter = new StreamWriter(cryptoStream);

        streamWriter.Write(dataToEncrypt);
        streamWriter.Flush();
        cryptoStream.FlushFinalBlock();

        return Convert.ToBase64String(memoryStream.ToArray());
    }

    /// <summary>
    /// Decrypts the specified encrypted data using AES algorithm.
    /// </summary>
    /// <param name="dataToDecrypt">The data to decrypt.</param>
    /// <returns>The decrypted data as a string.</returns>
    public string Decrypt(string dataToDecrypt)
    {
        if (string.IsNullOrEmpty(dataToDecrypt))
            return string.Empty;

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_key);
        aes.IV = new byte[16]; // Initialization vector with zeros

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        var buffer = Convert.FromBase64String(dataToDecrypt);
        using var memoryStream = new MemoryStream(buffer);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);

        return streamReader.ReadToEnd();
    }
}