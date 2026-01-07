using Ng.Domain.Enums;
using Ng.Domain.Primitives;
using Ng.Domain.Soccer.Entities;
using Ng.Domain.Soccer.Enums;
using Ng.Domain.Soccer.ValueObjects;

namespace Ng.Domain.Soccer.Aggregates;

/// <summary>
/// 축구 경기 애그리게이트 루트
/// </summary>
public class Match : AggregateRoot
{
    public Guid HomeTeamId { get; private set; }
    public Team HomeTeam { get; private set; } = null!;

    public Guid AwayTeamId { get; private set; }
    public Team AwayTeam { get; private set; } = null!;

    public Guid SeasonId { get; private set; }
    public Season Season { get; private set; } = null!;

    public int? Round { get; private set; }
    public DateTime MatchDate { get; private set; }

    public string? VenueName { get; private set; }
    public string? VenueCity { get; private set; }

    public int? HomeScore { get; private set; }
    public int? AwayScore { get; private set; }
    public int? HomeHalfTimeScore { get; private set; }
    public int? AwayHalfTimeScore { get; private set; }
    public int? HomeExtraTimeScore { get; private set; }
    public int? AwayExtraTimeScore { get; private set; }
    public int? HomePenaltyScore { get; private set; }
    public int? AwayPenaltyScore { get; private set; }

    public MatchStatus Status { get; private set; } = MatchStatus.Scheduled;
    public Ng.Domain.Enums.MatchType MatchType { get; private set; } = Ng.Domain.Enums.MatchType.Friendly;

    public int? Attendance { get; private set; }
    public string? RefereeName { get; private set; }
    public WeatherCondition? WeatherCondition { get; private set; }
    public int? Temperature { get; private set; }

    private readonly List<MatchLineup> mLineups = new();
    public IReadOnlyCollection<MatchLineup> Lineups => mLineups.AsReadOnly();

    private readonly List<MatchEvent> mEvents = new();
    public IReadOnlyCollection<MatchEvent> Events => mEvents.AsReadOnly();

    public MatchStatistics? Statistics { get; private set; }

    private readonly List<MatchOfficial> mOfficials = new();
    public IReadOnlyCollection<MatchOfficial> Officials => mOfficials.AsReadOnly();

    private Match() { }

    public Match(
        Guid homeTeamId,
        Guid awayTeamId,
        Guid seasonId,
        DateTime matchDate,
        Ng.Domain.Enums.MatchType matchType = Ng.Domain.Enums.MatchType.League) : base()
    {
        HomeTeamId = homeTeamId;
        AwayTeamId = awayTeamId;
        SeasonId = seasonId;
        MatchDate = matchDate;
        MatchType = matchType;
    }

    public void UpdateSchedule(DateTime matchDate, int? round = null)
    {
        MatchDate = matchDate;
        Round = round;
    }

    public void UpdateVenue(string? venueName, string? venueCity)
    {
        VenueName = venueName;
        VenueCity = venueCity;
    }

    //public void StartMatch()
    //{
    //    Status = MatchStatus.InProgress;
    //}

    public void UpdateScore(int homeScore, int awayScore)
    {
        HomeScore = homeScore;
        AwayScore = awayScore;
    }

    public void UpdateHalfTimeScore(int homeScore, int awayScore)
    {
        HomeHalfTimeScore = homeScore;
        AwayHalfTimeScore = awayScore;
    }

    public void UpdateExtraTimeScore(int homeScore, int awayScore)
    {
        HomeExtraTimeScore = homeScore;
        AwayExtraTimeScore = awayScore;
    }

    public void UpdatePenaltyScore(int homeScore, int awayScore)
    {
        HomePenaltyScore = homeScore;
        AwayPenaltyScore = awayScore;
    }

    //public void CompleteMatch()
    //{
    //    Status = MatchStatus.Completed;
    //}

    public void PostponeMatch()
    {
        Status = MatchStatus.Postponed;
    }

    public void CancelMatch()
    {
        Status = MatchStatus.Cancelled;
    }

    public void UpdateAttendance(int attendance)
    {
        Attendance = attendance;
    }

    public void UpdateWeather(WeatherCondition condition, int? temperature)
    {
        WeatherCondition = condition;
        Temperature = temperature;
    }

    public void AddLineup(MatchLineup lineup)
    {
        mLineups.Add(lineup);
    }

    public void AddEvent(MatchEvent matchEvent)
    {
        mEvents.Add(matchEvent);
    }

    public void UpdateStatistics(MatchStatistics statistics)
    {
        Statistics = statistics;
    }

    public void AddOfficial(MatchOfficial official)
    {
        mOfficials.Add(official);
    }
}
