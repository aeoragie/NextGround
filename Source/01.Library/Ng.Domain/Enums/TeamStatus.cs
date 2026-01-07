namespace Ng.Domain.Enums;

/// <summary>
/// 팀 상태
/// </summary>
public enum TeamStatus
{
    None = 0,

    Active = 1,         // 활동
    Inactive = 2,       // 휴식
    Disbanded = 3,      // 해산

    Max
}
