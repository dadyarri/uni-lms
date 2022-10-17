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