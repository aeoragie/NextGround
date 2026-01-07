using Ng.Domain.Common;
using Ng.Domain.SubDomains.Soccer.Enums;

namespace Ng.Domain.SubDomains.Soccer.Entities;

/// <summary>
/// 경기 라인업 엔티티
/// </summary>
public class MatchLineup : Entity
{
    public Guid MatchId { get; private set; }
    public Match Match { get; private set; } = null!;

    public Guid PlayerId { get; private set; }
    public Player Player { get; private set; } = null!;

    public Guid TeamId { get; private set; }
    public Team Team { get; private set; } = null!;

    public int JerseyNumber { get; private set; }
    public SoccerPosition Position { get; private set; }
    public bool IsStarting { get; private set; }
    public bool IsCaptain { get; private set; }

    public int? MinutesPlayed { get; private set; }
    public int? SubstitutionMinute { get; private set; }
    public Guid? SubstitutedPlayerId { get; private set; }
    public decimal? Rating { get; private set; }

    private MatchLineup() { }

    public MatchLineup(
        Guid matchId,
        Guid playerId,
        Guid teamId,
        int jerseyNumber,
        SoccerPosition position,
        bool isStarting)
    {
        MatchId = matchId;
        PlayerId = playerId;
        TeamId = teamId;
        JerseyNumber = jerseyNumber;
        Position = position;
        IsStarting = isStarting;
    }

    public void SetAsCaptain() => IsCaptain = true;

    public void RecordSubstitution(int minute, Guid? substitutedPlayerId = null)
    {
        SubstitutionMinute = minute;
        SubstitutedPlayerId = substitutedPlayerId;
    }

    public void UpdateMinutesPlayed(int minutes) => MinutesPlayed = minutes;

    public void SetRating(decimal rating) => Rating = rating;
}
