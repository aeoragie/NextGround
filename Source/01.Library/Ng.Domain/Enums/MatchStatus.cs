namespace Ng.Domain.Enums;

/// <summary>
/// 경기 상태
/// </summary>
public enum MatchStatus
{
    None = 0,

    Scheduled = 1,      // 예정
    FirstHalf = 2,      // 전반전
    HalfTime = 3,       // 하프타임
    SecondHalf = 4,     // 후반전
    Finished = 5,       // 종료
    Postponed = 6,      // 연기
    Cancelled = 7,      // 취소

    Max
}
