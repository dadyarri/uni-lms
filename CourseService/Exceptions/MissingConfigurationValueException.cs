namespace src.Exceptions; 

public class MissingConfigurationValueException : Exception {
  public MissingConfigurationValueException() {}
  public MissingConfigurationValueException(string message): base(message) {}
}