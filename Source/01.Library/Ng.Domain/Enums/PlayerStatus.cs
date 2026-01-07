namespace Ng.Domain.Enums;

/// <summary>
/// 선수 상태
/// </summary>
public enum PlayerStatus
{
    None = 0,

    Active = 1,          // 활동 (가용 상태)
    Injured = 2,         // 부상
    Resting = 3,         // 휴식 (컨디션 조절)
    Suspended = 4,       // 징계 (출전 정지)
    Free = 5,            // 소속 없음 (자유 계약/입단 테스트 가능)
    Transferred = 6,     // 이적 (타 팀으로 이동)
    Retired = 7,         // 은퇴 (선수 생활 종료)

    Max
}
