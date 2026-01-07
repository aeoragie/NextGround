namespace Ng.Domain.Enums;

/// <summary>
/// 시즌 유형
/// </summary>
public enum SeasonType
{
    None = 0,

    OffSeason = 1,      // 비시즌 (휴식 및 이적 시장)
    PreSeason = 2,      // 프리시즌 (훈련 및 친선 경기)
    InSeason = 3,       // 정규 시즌 (본 경기)
    PostSeason = 4,     // 포스트시즌 (승강 플레이오프, 챔피언 결정전 등)

    Max
}
