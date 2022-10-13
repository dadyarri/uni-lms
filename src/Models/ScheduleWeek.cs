namespace src.Models;

/// <summary>
/// Model, which representing schedule weeks
/// </summary>
public class ScheduleWeek : BaseModel {
  /// <summary>
  /// Name of week
  /// </summary>
  public required string Name { get; set; }
}