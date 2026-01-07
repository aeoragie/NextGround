namespace Ng.Domain.Soccer.Enums;

/// <summary>
/// 카드 유형
/// </summary>
public enum CardType
{
    /// <summary>
    /// 경고 (옐로카드)
    /// </summary>
    YellowCard = 1,

    /// <summary>
    /// 퇴장 (레드카드)
    /// </summary>
    RedCard = 2,

    /// <summary>
    /// 경고 누적 퇴장 (두 번째 옐로카드)
    /// </summary>
    SecondYellowCard = 3
}
