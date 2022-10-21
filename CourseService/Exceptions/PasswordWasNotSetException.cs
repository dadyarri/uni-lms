namespace src.Exceptions; 

public class PasswordWasNotSetException: Exception {
  public PasswordWasNotSetException() {}

  public PasswordWasNotSetException(string message): base(message) {}
}