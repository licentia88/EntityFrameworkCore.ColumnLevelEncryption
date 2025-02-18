namespace EntityFrameworkCore.ColumnLevelEncryption.Interfaces;
    
    /// <summary>
    /// Interface for encryption providers that handle encryption and decryption of data.
    /// </summary>
    public interface IEncryptionProvider
    {
        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <returns>The encrypted data as a string.</returns>
        string Encrypt(string data);
    
        /// <summary>
        /// Decrypts the specified encrypted data.
        /// </summary>
        /// <param name="encyptedData">The data to decrypt.</param>
        /// <returns>The decrypted data as a string.</returns>
        string Decrypt(string encyptedData);
    }