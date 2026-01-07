namespace Ng.Domain.SubDomains.Soccer.Values;

/// <summary>
/// 시즌 통계 값 객체
/// </summary>
public class SeasonStatistics
{
    public Guid SeasonId { get; private set; }

    public int TotalMatches { get; private set; }
    public int CompletedMatches { get; private set; }
    public int RemainingMatches => TotalMatches - CompletedMatches;

    public int TotalGoals { get; private set; }
    public decimal AverageGoalsPerMatch => CompletedMatches > 0 ? (decimal)TotalGoals / CompletedMatches : 0;

    public int HomeWins { get; private set; }
    public int Draws { get; private set; }
    public int AwayWins { get; private set; }

    public decimal HomeWinPercentage => CompletedMatches > 0 ? (decimal)HomeWins / CompletedMatches * 100 : 0;
    public decimal DrawPercentage => CompletedMatches > 0 ? (decimal)Draws / CompletedMatches * 100 : 0;
    public decimal AwayWinPercentage => CompletedMatches > 0 ? (decimal)AwayWins / CompletedMatches * 100 : 0;

    public int? HighestScoringMatch { get; private set; }
    public int? BiggestVictoryMargin { get; private set; }

    public int TotalYellowCards { get; private set; }
    public int TotalRedCards { get; private set; }

    public int? AverageAttendance { get; private set; }
    public int? HighestAttendance { get; private set; }
    public int? LowestAttendance { get; private set; }

    public int CleanSheets { get; private set; }
    public int PenaltyGoals { get; private set; }
    public int OwnGoals { get; private set; }
    public int HatTricks { get; private set; }

    public string? TopScorer { get; private set; }
    public int? TopScorerGoals { get; private set; }
    public string? TopAssistProvider { get; private set; }
    public int? TopAssistProviderAssists { get; private set; }
    public string? MostCleanSheetsTeam { get; private set; }
    public int? MostCleanSheets { get; private set; }

    private SeasonStatistics() { }

    public SeasonStatistics(Guid seasonId)
    {
        SeasonId = seasonId;
    }

    public void UpdateMatchCounts(int totalMatches, int completedMatches)
    {
        TotalMatches = totalMatches;
        CompletedMatches = completedMatches;
    }

    public void UpdateGoals(int totalGoals) => TotalGoals = totalGoals;

    public void UpdateResultDistribution(int homeWins, int draws, int awayWins)
    {
        HomeWins = homeWins;
        Draws = draws;
        AwayWins = awayWins;
    }

    public void UpdateMatchRecords(int? highestScoringMatch, int? biggestVictoryMargin)
    {
        HighestScoringMatch = highestScoringMatch;
        BiggestVictoryMargin = biggestVictoryMargin;
    }

    public void UpdateCards(int totalYellowCards, int totalRedCards)
    {
        TotalYellowCards = totalYellowCards;
        TotalRedCards = totalRedCards;
    }

    public void UpdateAttendance(int? average, int? highest, int? lowest)
    {
        AverageAttendance = average;
        HighestAttendance = highest;
        LowestAttendance = lowest;
    }

    public void UpdateSpecialGoals(int cleanSheets, int penaltyGoals, int ownGoals, int hatTricks)
    {
        CleanSheets = cleanSheets;
        PenaltyGoals = penaltyGoals;
        OwnGoals = ownGoals;
        HatTricks = hatTricks;
    }

    public void UpdateTopScorer(string name, int goals)
    {
        TopScorer = name;
        TopScorerGoals = goals;
    }

    public void UpdateTopAssistProvider(string name, int assists)
    {
        TopAssistProvider = name;
        TopAssistProviderAssists = assists;
    }

    public void UpdateMostCleanSheets(string teamName, int cleanSheets)
    {
        MostCleanSheetsTeam = teamName;
        MostCleanSheets = cleanSheets;
    }
}
