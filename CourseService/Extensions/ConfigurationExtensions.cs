using src.Exceptions;


namespace src.Extensions;

/// <summary>
/// Set of extensions for configuration manager
/// </summary>
public static class ConfigurationExtensions {
  /// <summary>
  /// Validator of configuration: Checks existing of required properties
  /// </summary>
  /// <param name="configuration">Manager of configuration</param>
  /// <exception cref="InvalidOperationException">Throws, if any of required sections in configuration are missing</exception>
  /// <exception cref="MissingConfigurationValueException">Throws, if any of the required values in configuration are missing</exception>
  public static void ValidateConfiguration(this ConfigurationManager configuration) {
    var emailAddress =
      configuration.GetRequiredSection("MailSettings").GetValue<string>("Address");

    var emailToken = configuration.GetRequiredSection("MailSettings").GetValue<string>("Token");

    var emailHost = configuration.GetRequiredSection("MailSettings").GetValue<string>("Host");

    if (emailAddress == null) {
      throw new MissingConfigurationValueException(
        "MailSettings.Address configuration value is required"
      );
    }

    if (emailToken == null) {
      throw new MissingConfigurationValueException(
        "MailSettings.Token configuration value is required"
      );
    }

    if (emailHost == null) {
      throw new MissingConfigurationValueException(
        "MailSettings.Host configuration value is required"
      );
    }
  }
}