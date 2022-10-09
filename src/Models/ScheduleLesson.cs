namespace src.Models;

/// <summary>
/// Model, which representing lesson of schedule
/// </summary>
public class ScheduleLesson : BaseModel {
  /// <summary>
  /// Group of lesson
  /// </summary>
  public required Group Group;

  /// <summary>
  /// Subgroup of lesson
  /// </summary>
  public int? Subgroup;

  /// <summary>
  /// Time of the lesson
  /// </summary>
  public required ScheduleTime Time;

  /// <summary>
  /// Course of the lesson
  /// </summary>
  public required Course Course;

  /// <summary>
  /// Tutor of the lesson
  /// </summary>
  public required User Tutor;

  /// <summary>
  /// Classrom of lesson
  /// </summary>
  public required ScheduleClassroom Classroom;

  /// <summary>
  /// Week of shedule of lesson
  /// </summary>
  public List<ScheduleWeek>? Weeks;
}