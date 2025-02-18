namespace EntityFrameworkCore.ColumnLevelEncryption.Attribute;
 
 /// <summary>
 /// An attribute to specify that a property should be encrypted at the column level in the database.
 /// </summary>
 [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
 public sealed class EncryptColumnAttribute : System.Attribute
 {
 }