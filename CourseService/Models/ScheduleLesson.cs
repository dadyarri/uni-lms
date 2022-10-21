namespace src.Models;

/// <summary>
/// Model, which representing lesson of schedule
/// </summary>
public class ScheduleLesson : BaseModel {
  /// <summary>
  /// The group for which the lesson will be held
  /// </summary>
  public required Group Group { get; set; }

  /// <summary>
  /// The subgroup for which the lesson will be held
  /// </summary>
  public int? Subgroup { get; set; }

  /// <summary>
  /// The time at which the lesson will take place
  /// </summary>
  public required ScheduleTime Time { get; set; }

  /// <summary>
  /// The subject on which the lesson will take place
  /// </summary>
  public required Course Course { get; set; }

  /// <summary>
  /// The teacher who will conduct the lesson
  /// </summary>
  public required User Tutor { get; set; }

  /// <summary>
  /// The audience in which the lesson will be held
  /// </summary>
  public required ScheduleClassroom Classroom { get; set; }

  /// <summary>
  /// Week of shedule of lesson
  /// </summary>
  public List<ScheduleWeek>? Weeks { get; set; }

  /// <summary>
  /// Type of lesson
  /// </summary>
  public required LessonType Type { get; set; }
}