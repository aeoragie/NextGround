using Ng.Domain.Primitives;
using Ng.Domain.Soccer.Aggregates;

namespace Ng.Domain.Soccer.Entities;

/// <summary>
/// 순위표 항목 엔티티
/// </summary>
public class StandingEntry : Entity
{
    public Guid SeasonId { get; private set; }
    public Season Season { get; private set; } = null!;

    public Guid TeamId { get; private set; }
    public Team Team { get; private set; } = null!;

    public int Position { get; private set; }

    public int Played { get; private set; }
    public int Won { get; private set; }
    public int Drawn { get; private set; }
    public int Lost { get; private set; }

    public int GoalsFor { get; private set; }
    public int GoalsAgainst { get; private set; }
    public int GoalDifference => GoalsFor - GoalsAgainst;
    public int Points { get; private set; }

    public int HomeWon { get; private set; }
    public int HomeDrawn { get; private set; }
    public int HomeLost { get; private set; }
    public int HomeGoalsFor { get; private set; }
    public int HomeGoalsAgainst { get; private set; }

    public int AwayWon { get; private set; }
    public int AwayDrawn { get; private set; }
    public int AwayLost { get; private set; }
    public int AwayGoalsFor { get; private set; }
    public int AwayGoalsAgainst { get; private set; }

    public string? Form { get; private set; }
    public DateTime LastUpdated { get; private set; }

    private StandingEntry() { }

    public StandingEntry(Guid seasonId, Guid teamId, int position)
    {
        SeasonId = seasonId;
        TeamId = teamId;
        Position = position;
        LastUpdated = DateTime.UtcNow;
    }

    public void UpdatePosition(int position)
    {
        Position = position;
        LastUpdated = DateTime.UtcNow;
    }

    public void UpdateRecord(int won, int drawn, int lost, int goalsFor, int goalsAgainst)
    {
        Won = won;
        Drawn = drawn;
        Lost = lost;
        Played = won + drawn + lost;
        GoalsFor = goalsFor;
        GoalsAgainst = goalsAgainst;
        Points = (won * 3) + drawn;
        LastUpdated = DateTime.UtcNow;
    }

    public void UpdateHomeRecord(int won, int drawn, int lost, int goalsFor, int goalsAgainst)
    {
        HomeWon = won;
        HomeDrawn = drawn;
        HomeLost = lost;
        HomeGoalsFor = goalsFor;
        HomeGoalsAgainst = goalsAgainst;
        LastUpdated = DateTime.UtcNow;
    }

    public void UpdateAwayRecord(int won, int drawn, int lost, int goalsFor, int goalsAgainst)
    {
        AwayWon = won;
        AwayDrawn = drawn;
        AwayLost = lost;
        AwayGoalsFor = goalsFor;
        AwayGoalsAgainst = goalsAgainst;
        LastUpdated = DateTime.UtcNow;
    }

    public void UpdateForm(string form)
    {
        Form = form;
        LastUpdated = DateTime.UtcNow;
    }
}
