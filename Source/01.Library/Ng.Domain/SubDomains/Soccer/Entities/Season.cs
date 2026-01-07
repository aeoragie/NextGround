using Ng.Domain.Enums;
using Ng.Domain.Primitives;
using Ng.Domain.Soccer.Entities;
using Ng.Domain.Soccer.ValueObjects;

namespace Ng.Domain.Soccer.Aggregates;

/// <summary>
/// 축구 시즌 애그리게이트 루트
/// </summary>
public class Season : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsCurrent { get; private set; }
    public SeasonType SeasonType { get; private set; } = SeasonType.InSeason;

    public Guid LeagueId { get; private set; }
    public League League { get; private set; } = null!;

    private readonly List<Match> mMatches = new();
    public IReadOnlyCollection<Match> Matches => mMatches.AsReadOnly();

    private readonly List<StandingEntry> mStandings = new();
    public IReadOnlyCollection<StandingEntry> Standings => mStandings.AsReadOnly();

    public SeasonStatistics? Statistics { get; private set; }

    private Season() { }

    public Season(string name, DateTime startDate, DateTime endDate, Guid leagueId) : base()
    {
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        LeagueId = leagueId;
    }

    public void UpdateDates(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public void SetAsCurrent()
    {
        IsCurrent = true;
    }

    public void SetAsNotCurrent()
    {
        IsCurrent = false;
    }

    public void ChangeSeasonType(SeasonType seasonType)
    {
        SeasonType = seasonType;
    }

    public void AddMatch(Match match)
    {
        mMatches.Add(match);
    }

    public void UpdateStandings(IEnumerable<StandingEntry> standings)
    {
        mStandings.Clear();
        mStandings.AddRange(standings);
    }

    public void UpdateStatistics(SeasonStatistics statistics)
    {
        Statistics = statistics;
    }
}
