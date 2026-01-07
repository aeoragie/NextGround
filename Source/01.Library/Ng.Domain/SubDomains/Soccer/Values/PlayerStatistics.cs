namespace Ng.Domain.SubDomains.Soccer.Values;

/// <summary>
/// 선수 통계 값 객체
/// </summary>
public class PlayerStatistics
{
    public string Season { get; private set; } = string.Empty;
    public string? CompetitionName { get; private set; }

    public int Appearances { get; private set; }
    public int Starts { get; private set; }
    public int SubstituteAppearances { get; private set; }
    public int MinutesPlayed { get; private set; }

    public int Goals { get; private set; }
    public int Assists { get; private set; }
    public int YellowCards { get; private set; }
    public int RedCards { get; private set; }
    public int SecondYellowCards { get; private set; }

    public int PenaltyGoals { get; private set; }
    public int PenaltiesMissed { get; private set; }
    public int OwnGoals { get; private set; }

    public int Shots { get; private set; }
    public int ShotsOnTarget { get; private set; }
    public decimal? PassAccuracy { get; private set; }
    public int? KeyPasses { get; private set; }

    public int? TacklesWon { get; private set; }
    public int? Interceptions { get; private set; }
    public int? Clearances { get; private set; }

    public decimal? AverageRating { get; private set; }
    public int? ManOfTheMatchAwards { get; private set; }

    private PlayerStatistics() { }

    public PlayerStatistics(string season)
    {
        Season = season;
    }

    public void SetCompetition(string competitionName) => CompetitionName = competitionName;

    public void UpdateAppearances(int appearances, int starts, int substituteAppearances, int minutesPlayed)
    {
        Appearances = appearances;
        Starts = starts;
        SubstituteAppearances = substituteAppearances;
        MinutesPlayed = minutesPlayed;
    }

    public void UpdateGoalContributions(int goals, int assists, int penaltyGoals, int penaltiesMissed, int ownGoals)
    {
        Goals = goals;
        Assists = assists;
        PenaltyGoals = penaltyGoals;
        PenaltiesMissed = penaltiesMissed;
        OwnGoals = ownGoals;
    }

    public void UpdateCards(int yellowCards, int redCards, int secondYellowCards)
    {
        YellowCards = yellowCards;
        RedCards = redCards;
        SecondYellowCards = secondYellowCards;
    }

    public void UpdateShootingStats(int shots, int shotsOnTarget)
    {
        Shots = shots;
        ShotsOnTarget = shotsOnTarget;
    }

    public void UpdatePassingStats(decimal? passAccuracy, int? keyPasses)
    {
        PassAccuracy = passAccuracy;
        KeyPasses = keyPasses;
    }

    public void UpdateDefensiveStats(int? tacklesWon, int? interceptions, int? clearances)
    {
        TacklesWon = tacklesWon;
        Interceptions = interceptions;
        Clearances = clearances;
    }

    public void UpdateRating(decimal? averageRating, int? manOfTheMatchAwards)
    {
        AverageRating = averageRating;
        ManOfTheMatchAwards = manOfTheMatchAwards;
    }
}
