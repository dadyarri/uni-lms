using src.Exceptions;


namespace src.Extensions;

/// <summary>
/// Set of extensions for <see cref="IConfiguration"/>
/// </summary>
public static class ConfigurationExtensions {
  /// <summary>
  /// Guaranteed fetching value from loaded configuration
  /// </summary>
  /// <param name="config">Configuration object, to load values from</param>
  /// <param name="section">Section of configuration to fetch value</param>
  /// <returns></returns>
  /// <exception cref="MissingConfigurationValueException">Throws if specified section was not found in loaded configuration</exception>
  public static string GuaranteedGetValue(this IConfiguration config, string section) {
    string? value;
    try {
      value = config.GetRequiredSection(section).Value;
    }
    catch (InvalidOperationException) {
      throw new MissingConfigurationValueException(
        $"{section} was not presented in loaded configuration"
      );
    }

    if (value == null) {
      throw new MissingConfigurationValueException(
        $"{section} was not presented in loaded configuration"
      );
    }

    return value;
  }
}