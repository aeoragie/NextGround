namespace Ng.Domain.SubDomains.Soccer.Enums;

/// <summary>
/// 부상 심각도
/// </summary>
public enum InjurySeverity
{
    /// <summary>
    /// 경미 (1-7일)
    /// </summary>
    Minor = 1,

    /// <summary>
    /// 보통 (1-4주)
    /// </summary>
    Moderate = 2,

    /// <summary>
    /// 심각 (1-3개월)
    /// </summary>
    Major = 3,

    /// <summary>
    /// 매우 심각 (3개월 이상)
    /// </summary>
    Severe = 4,

    /// <summary>
    /// 선수 생활 종료
    /// </summary>
    CareerEnding = 5
}
