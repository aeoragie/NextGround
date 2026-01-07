namespace Ng.Domain.Soccer.Enums;

/// <summary>
/// 경기 시간대
/// </summary>
public enum MatchPeriod
{
    /// <summary>
    /// 경기 전
    /// </summary>
    PreMatch = 0,

    /// <summary>
    /// 전반전
    /// </summary>
    FirstHalf = 1,

    /// <summary>
    /// 하프타임
    /// </summary>
    HalfTime = 2,

    /// <summary>
    /// 후반전
    /// </summary>
    SecondHalf = 3,

    /// <summary>
    /// 정규 시간 종료
    /// </summary>
    FullTime = 4,

    /// <summary>
    /// 연장 전반전
    /// </summary>
    ExtraTimeFirstHalf = 5,

    /// <summary>
    /// 연장 하프타임
    /// </summary>
    ExtraTimeHalfTime = 6,

    /// <summary>
    /// 연장 후반전
    /// </summary>
    ExtraTimeSecondHalf = 7,

    /// <summary>
    /// 승부차기
    /// </summary>
    PenaltyShootout = 8,

    /// <summary>
    /// 경기 후
    /// </summary>
    PostMatch = 9
}
