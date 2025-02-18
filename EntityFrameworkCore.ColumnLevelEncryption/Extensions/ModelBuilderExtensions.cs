using System.Reflection;
using EntityFrameworkCore.ColumnLevelEncryption.Attribute;
using EntityFrameworkCore.ColumnLevelEncryption.Converter;
using EntityFrameworkCore.ColumnLevelEncryption.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.ColumnLevelEncryption.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ModelBuilder"/> to enable column-level encryption.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Enables encryption for properties marked with the <see cref="EncryptColumnAttribute"/>.
    /// </summary>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> instance to configure.</param>
    /// <param name="encryptionProvider">The encryption provider to use for encrypting and decrypting data.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="modelBuilder"/> or <paramref name="encryptionProvider"/> is null.</exception>
    public static void UseEncryption(this ModelBuilder modelBuilder, IEncryptionProvider encryptionProvider)
    {
        if (modelBuilder is null)
            throw new ArgumentNullException(nameof(modelBuilder), "The ModelBuilder instance cannot be null.");
        
        if (encryptionProvider is null)
            throw new ArgumentNullException(nameof(encryptionProvider), "The encryption provider cannot be null.");

        var encryptionConverter = new EncryptionConverter(encryptionProvider);

        // Process all entity types in the model
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ProcessEntityType(entityType, encryptionConverter);
        }
    }

    /// <summary>
    /// Processes an entity type to apply encryption to its properties.
    /// </summary>
    /// <param name="entityType">The entity type to process.</param>
    /// <param name="encryptionConverter">The encryption converter to apply.</param>
    private static void ProcessEntityType(IMutableEntityType entityType, EncryptionConverter encryptionConverter)
    {
        foreach (var property in entityType.GetProperties())
        {
            if (ShouldEncryptProperty(property))
            {
                property.SetValueConverter(encryptionConverter);
            }
        }
    }

    /// <summary>
    /// Determines whether a property should be encrypted.
    /// </summary>
    /// <param name="property">The property to check.</param>
    /// <returns>True if the property should be encrypted; otherwise, false.</returns>
    private static bool ShouldEncryptProperty(IMutableProperty property)
    {
        return CanEncrypt(property) && HasEncryptColumnAttribute(property);
    }

    /// <summary>
    /// Determines whether a property can be encrypted.
    /// </summary>
    /// <param name="property">The property to check.</param>
    /// <returns>True if the property can be encrypted; otherwise, false.</returns>
    private static bool CanEncrypt(IMutableProperty property)
    {
        // Ensure the property is not a discriminator, has a valid PropertyInfo, and is of type string.
        return property.Name != "Discriminator" && 
               property.PropertyInfo != null && 
               property.ClrType == typeof(string);
    }

    /// <summary>
    /// Determines whether a property is marked with the <see cref="EncryptColumnAttribute"/>.
    /// </summary>
    /// <param name="property">The property to check.</param>
    /// <returns>True if the property has the <see cref="EncryptColumnAttribute"/>; otherwise, false.</returns>
    private static bool HasEncryptColumnAttribute(IMutableProperty property)
    {
        return property.PropertyInfo?.GetCustomAttribute<EncryptColumnAttribute>(false) != null;
    }
}