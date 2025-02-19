# EntityFrameworkCore Column-Level Encryption

## Overview
This library provides a simple way to encrypt and decrypt column-level data in Entity Framework Core using an encryption provider.

## Let's Connect!
I appreciate every star ⭐ that my projects receive, and your support means a lot to me! If you find my projects useful or enjoyable, please consider giving them a star.


## Features
- Automatic encryption and decryption of string properties marked with `@EncryptColumn`.
- Uses AES encryption for secure data storage.
- Easy integration with `DbContext` using `ModelBuilder` extensions.

## Installation
To use this package, add the required dependencies to your .NET project:

You can install this template using [NuGet](https://www.nuget.org/packages/https://www.nuget.org/packages/EntityFrameworkCore.ColumnLevelEncryption):

```sh
// Install EF Core and required packages
 dotnet add package EntityFrameworkCore.ColumnLevelEncryption
```

## Usage

### 1. Define the Encryption Provider & Configure the `DbContext`

Modify your `DbContext` to use the encryption provider:

```csharp
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore.ColumnLevelEncryption.Util;

public class BetContext : DbContext
{
    private readonly GenerateEncryptionProvider _provider;

    public BetContext(DbContextOptions<BetContext> options) : base(options)
    {
        _provider = new GenerateEncryptionProvider("WOXcoTgvWTh+PXaYCAfiEQ==");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseEncryption(_provider);
        base.OnModelCreating(modelBuilder);
    }
}
```

### 2. Apply Encryption to Entity Properties

Add the `@EncryptColumn` attribute to properties that should be encrypted:

```csharp
using EntityFrameworkCore.ColumnLevelEncryption.Attribute;

public class User
{
    public int Id { get; set; }
    
    [EncryptColumn]
    public string U_PASSWORD { get; set; }
}
```



## How It Works
- When Entity Framework Core interacts with the database, properties marked with `@EncryptColumn` are automatically encrypted before saving and decrypted when retrieved.
- The `ModelBuilderExtensions` class scans all entity types and applies the encryption logic to properties with the `@EncryptColumn` attribute.

## Security Considerations
- The encryption key should be securely stored, not hardcoded in the application.
- Consider using a key management service (e.g., Azure Key Vault, AWS KMS) to securely handle encryption keys.

## License
This project is licensed under the MIT License.

