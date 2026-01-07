using Ng.Domain.Common;
using Ng.Domain.SubDomains.Soccer.Enums;

namespace Ng.Domain.SubDomains.Soccer.Entities;

/// <summary>
/// 경기 이벤트 엔티티
/// </summary>
public class MatchEvent : Entity
{
    public Guid MatchId { get; private set; }
    public Match Match { get; private set; } = null!;

    public MatchEventType EventType { get; private set; }
    public int Minute { get; private set; }
    public int? ExtraMinute { get; private set; }
    public MatchPeriod Period { get; private set; }

    public Guid TeamId { get; private set; }
    public Team Team { get; private set; } = null!;

    public Guid? PlayerId { get; private set; }
    public Player? Player { get; private set; }

    public Guid? RelatedPlayerId { get; private set; }
    public Player? RelatedPlayer { get; private set; }

    public CardType? CardType { get; private set; }
    public string? Description { get; private set; }
    public string? VideoUrl { get; private set; }

    private MatchEvent() { }

    public MatchEvent(
        Guid matchId,
        MatchEventType eventType,
        int minute,
        MatchPeriod period,
        Guid teamId,
        Guid? playerId = null)
    {
        MatchId = matchId;
        EventType = eventType;
        Minute = minute;
        Period = period;
        TeamId = teamId;
        PlayerId = playerId;
    }

    public void SetExtraMinute(int extraMinute) => ExtraMinute = extraMinute;
    public void SetRelatedPlayer(Guid playerId) => RelatedPlayerId = playerId;
    public void SetCardType(CardType cardType) => CardType = cardType;
    public void SetDescription(string description) => Description = description;
    public void SetVideoUrl(string videoUrl) => VideoUrl = videoUrl;
}
