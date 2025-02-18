using EntityFrameworkCore.ColumnLevelEncryption.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EntityFrameworkCore.ColumnLevelEncryption.Converter;

/// <summary>
/// A value converter that encrypts and decrypts string properties using the specified encryption provider.
/// </summary>
public sealed class EncryptionConverter : ValueConverter<string, string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EncryptionConverter"/> class.
    /// </summary>
    /// <param name="encryptionProvider">The encryption provider used to encrypt and decrypt data.</param>
    /// <param name="mappingHints">Optional mapping hints for the converter.</param>
    public EncryptionConverter(IEncryptionProvider encryptionProvider, ConverterMappingHints? mappingHints = null) 
        : base(
            data => encryptionProvider.Encrypt(data), 
            encryptedData => encryptionProvider.Decrypt(encryptedData), 
            mappingHints)
    {
    }
}