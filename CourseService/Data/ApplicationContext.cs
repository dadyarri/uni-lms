using Microsoft.EntityFrameworkCore;

using src.Models;


namespace src.Data;

/// <summary>
/// Context of Data Base
/// </summary>
public class ApplicationContext : DbContext {
  /// <summary>
  /// Constractor of context
  /// </summary>
  /// <param name="options">Parms of context</param>
  public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

  /// <summary>
  /// Collection of users
  /// </summary>
  public DbSet<User> Users => Set<User>();

  /// <summary>
  /// Collection of code of registration
  /// </summary>
  public DbSet<RegisterCode> RegisterCodes => Set<RegisterCode>();

  /// <summary>
  /// Collection roles
  /// </summary>
  public DbSet<Role> Roles => Set<Role>();

  /// <summary>
  /// Collections of groups
  /// </summary>
  public DbSet<Group> Groups => Set<Group>();

  /// <summary>
  /// Configuring models before creating in database
  /// </summary>
  /// <param name="builder">The builder being used to construct the model for this context. Databases (and other extensions) typically define extension methods on this object that allow you to configure aspects of the model that are specific to a given database.</param>
  protected override void OnModelCreating(ModelBuilder builder) {
    builder.Entity<User>(
      entity => {
        entity.HasIndex(e => e.Email).IsUnique();
      }
    );
    builder.Entity<RegisterCode>(
      entity => {
        entity.HasIndex(e => e.Code).IsUnique();
      }
    );
  }
}