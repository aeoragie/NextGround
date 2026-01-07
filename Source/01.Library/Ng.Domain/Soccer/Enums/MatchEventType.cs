namespace Ng.Domain.Soccer.Enums;

/// <summary>
/// 경기 이벤트 유형
/// </summary>
public enum MatchEventType
{
    /// <summary>
    /// 골
    /// </summary>
    Goal = 1,

    /// <summary>
    /// 자책골
    /// </summary>
    OwnGoal = 2,

    /// <summary>
    /// 페널티 골
    /// </summary>
    PenaltyGoal = 3,

    /// <summary>
    /// 페널티 실축
    /// </summary>
    PenaltyMissed = 4,

    /// <summary>
    /// 경고
    /// </summary>
    YellowCard = 10,

    /// <summary>
    /// 퇴장
    /// </summary>
    RedCard = 11,

    /// <summary>
    /// 선수 교체
    /// </summary>
    Substitution = 20,

    /// <summary>
    /// 부상
    /// </summary>
    Injury = 21,

    /// <summary>
    /// VAR 판정
    /// </summary>
    VAR = 30,

    /// <summary>
    /// 오프사이드
    /// </summary>
    Offside = 31,

    /// <summary>
    /// 파울
    /// </summary>
    Foul = 32,

    /// <summary>
    /// 코너킥
    /// </summary>
    Corner = 40,

    /// <summary>
    /// 프리킥
    /// </summary>
    FreeKick = 41,

    /// <summary>
    /// 스로인
    /// </summary>
    ThrowIn = 42,

    /// <summary>
    /// 골킥
    /// </summary>
    GoalKick = 43,

    /// <summary>
    /// 어시스트
    /// </summary>
    Assist = 50,

    /// <summary>
    /// 슈팅
    /// </summary>
    Shot = 51,

    /// <summary>
    /// 유효 슈팅
    /// </summary>
    ShotOnTarget = 52,

    /// <summary>
    /// 슈팅 블락
    /// </summary>
    BlockedShot = 53,

    /// <summary>
    /// 세이브
    /// </summary>
    Save = 54,

    /// <summary>
    /// 전반 종료
    /// </summary>
    HalfTime = 100,

    /// <summary>
    /// 정규 시간 종료
    /// </summary>
    FullTime = 101,

    /// <summary>
    /// 연장전 시작
    /// </summary>
    ExtraTimeStart = 102,

    /// <summary>
    /// 연장전 종료
    /// </summary>
    ExtraTimeEnd = 103,

    /// <summary>
    /// 승부차기 시작
    /// </summary>
    PenaltyShootoutStart = 104,

    /// <summary>
    /// 승부차기 종료
    /// </summary>
    PenaltyShootoutEnd = 105
}
