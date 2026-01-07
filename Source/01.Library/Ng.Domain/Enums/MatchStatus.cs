namespace Ng.Domain.Enums;

/// <summary>
/// 경기 상태
/// </summary>
public enum MatchStatus
{
    Scheduled = 1,
    InProgress = 2,
    Live = 3,
    Completed = 4,
    Postponed = 5,
    Cancelled = 6,
    Forfeit = 7,
    Walkover = 8,
    Suspended = 9
}
