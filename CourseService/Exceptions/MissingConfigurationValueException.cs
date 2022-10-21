namespace src.Exceptions; 

/// <summary>
/// Exception, which raised, when configuration lacks of some values
/// </summary>
public class MissingConfigurationValueException : Exception {
  /// <inheritdoc />
  public MissingConfigurationValueException() {}

  /// <inheritdoc />
  public MissingConfigurationValueException(string message): base(message) {}
}