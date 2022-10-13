namespace src.Models;

/// <summary>
/// Model, which representing classroom of schedule
/// </summary>
public class ScheduleClassroom : BaseModel {
  /// <summary>
  /// Classroom of lesson
  /// </summary>
  public required string Classroom { get; set; }

  /// <summary>
  /// Building where classrom is located
  /// </summary>
  public int? Building { get; set; }
}