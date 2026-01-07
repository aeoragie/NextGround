namespace Ng.Domain.SubDomains.Soccer.Values;

/// <summary>
/// 팀 통계 값 객체
/// </summary>
public class TeamStatistics
{
    public string Season { get; private set; } = string.Empty;
    public string? CompetitionName { get; private set; }

    public int MatchesPlayed { get; private set; }
    public int Wins { get; private set; }
    public int Draws { get; private set; }
    public int Losses { get; private set; }

    public int GoalsScored { get; private set; }
    public int GoalsConceded { get; private set; }
    public int GoalDifference => GoalsScored - GoalsConceded;
    public int Points { get; private set; }

    public decimal WinRate => MatchesPlayed > 0 ? (decimal)Wins / MatchesPlayed * 100 : 0;
    public decimal AverageGoalsScored => MatchesPlayed > 0 ? (decimal)GoalsScored / MatchesPlayed : 0;
    public decimal AverageGoalsConceded => MatchesPlayed > 0 ? (decimal)GoalsConceded / MatchesPlayed : 0;

    public int CleanSheets { get; private set; }
    public decimal CleanSheetPercentage => MatchesPlayed > 0 ? (decimal)CleanSheets / MatchesPlayed * 100 : 0;

    public decimal? AveragePossession { get; private set; }
    public decimal? AverageShots { get; private set; }
    public decimal? AverageShotsOnTarget { get; private set; }
    public decimal? AverageCorners { get; private set; }
    public decimal? AverageFouls { get; private set; }

    public int YellowCards { get; private set; }
    public int RedCards { get; private set; }

    public int HomeWins { get; private set; }
    public int HomeDraws { get; private set; }
    public int HomeLosses { get; private set; }

    public int AwayWins { get; private set; }
    public int AwayDraws { get; private set; }
    public int AwayLosses { get; private set; }

    private TeamStatistics() { }

    public TeamStatistics(string season)
    {
        Season = season;
    }

    public void SetCompetition(string competitionName) => CompetitionName = competitionName;

    public void UpdateRecord(int wins, int draws, int losses, int goalsScored, int goalsConceded, int points)
    {
        Wins = wins;
        Draws = draws;
        Losses = losses;
        MatchesPlayed = wins + draws + losses;
        GoalsScored = goalsScored;
        GoalsConceded = goalsConceded;
        Points = points;
    }

    public void UpdateHomeRecord(int wins, int draws, int losses)
    {
        HomeWins = wins;
        HomeDraws = draws;
        HomeLosses = losses;
    }

    public void UpdateAwayRecord(int wins, int draws, int losses)
    {
        AwayWins = wins;
        AwayDraws = draws;
        AwayLosses = losses;
    }

    public void UpdateCards(int yellowCards, int redCards)
    {
        YellowCards = yellowCards;
        RedCards = redCards;
    }

    public void UpdateCleanSheets(int cleanSheets) => CleanSheets = cleanSheets;

    public void UpdateAverageStats(
        decimal? possession,
        decimal? shots,
        decimal? shotsOnTarget,
        decimal? corners,
        decimal? fouls)
    {
        AveragePossession = possession;
        AverageShots = shots;
        AverageShotsOnTarget = shotsOnTarget;
        AverageCorners = corners;
        AverageFouls = fouls;
    }
}
