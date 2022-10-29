namespace src.Extensions;

/// <summary>
/// Set of extensions for configuration manager
/// </summary>
public static class ConfigurationExtensions {
  /// <summary>
  /// Validator of configuration: Checks existing of required properties
  /// </summary>
  /// <param name="configuration">Manager of configuration (see <see cref="ConfigurationManager"/>)</param>
  /// <exception cref="InvalidOperationException">Throws, if any of required sections in configuration are missing</exception>
  public static void ValidateConfiguration(this ConfigurationManager configuration) {

    var sections = new [] { "MailSettings:Address", "MailSettings:Token", "MailSettings:Host", "Security:Token" };

    foreach (var section in sections) {
      var _ = configuration.GetRequiredSection(section).Value;
    }
  }
}