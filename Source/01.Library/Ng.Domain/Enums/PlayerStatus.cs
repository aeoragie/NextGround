namespace Ng.Domain.Enums;

/// <summary>
/// 선수 상태
/// </summary>
public enum PlayerStatus
{
    Active,      // 활동 중
    Injured,     // 부상
    OnLoan,      // 임대 중
    Suspended,   // 출장 정지
    Retired,     // 은퇴
    FreeAgent    // FA (자유계약)
}
