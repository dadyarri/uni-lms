namespace src.Exceptions; 

/// <summary>
/// Exception, which raised, when someone tries to login as created, but not registered user
/// </summary>
public class PasswordWasNotSetException: Exception {
  /// <inheritdoc />
  public PasswordWasNotSetException() {}

  /// <inheritdoc />
  public PasswordWasNotSetException(string message): base(message) {}
}