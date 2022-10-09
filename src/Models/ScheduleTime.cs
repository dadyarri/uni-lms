namespace src.Models;

/// <summary>
/// Model, which representing schedule time
/// </summary>
public class ScheduleTime : BaseModel {
  /// <summary>
  /// Time of the start of lesson
  /// </summary>
  public required TimeSpan TimeStart;

  /// <summary>
  /// Time of the end of lesson
  /// </summary>
  public required TimeSpan TimeEnd;
}