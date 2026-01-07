namespace Ng.Domain.Soccer.ValueObjects;

/// <summary>
/// 경기 통계 값 객체
/// </summary>
public class MatchStatistics
{
    public Guid MatchId { get; private set; }

    public decimal HomePossession { get; private set; }
    public decimal AwayPossession => 100 - HomePossession;

    public int HomeShots { get; private set; }
    public int AwayShots { get; private set; }
    public int HomeShotsOnTarget { get; private set; }
    public int AwayShotsOnTarget { get; private set; }

    public int HomeCorners { get; private set; }
    public int AwayCorners { get; private set; }

    public int HomeFouls { get; private set; }
    public int AwayFouls { get; private set; }

    public int HomeOffsides { get; private set; }
    public int AwayOffsides { get; private set; }

    public int HomeYellowCards { get; private set; }
    public int AwayYellowCards { get; private set; }
    public int HomeRedCards { get; private set; }
    public int AwayRedCards { get; private set; }

    public int? HomePasses { get; private set; }
    public int? AwayPasses { get; private set; }
    public decimal? HomePassAccuracy { get; private set; }
    public decimal? AwayPassAccuracy { get; private set; }

    public int? HomeTackles { get; private set; }
    public int? AwayTackles { get; private set; }
    public int? HomeInterceptions { get; private set; }
    public int? AwayInterceptions { get; private set; }

    public int? HomeSaves { get; private set; }
    public int? AwaySaves { get; private set; }
    public int? HomeCrosses { get; private set; }
    public int? AwayCrosses { get; private set; }
    public int? HomeDribbles { get; private set; }
    public int? AwayDribbles { get; private set; }

    private MatchStatistics() { }

    public MatchStatistics(Guid matchId)
    {
        MatchId = matchId;
    }

    public void UpdatePossession(decimal homePossession) => HomePossession = homePossession;

    public void UpdateShots(int homeShots, int awayShots, int homeShotsOnTarget, int awayShotsOnTarget)
    {
        HomeShots = homeShots;
        AwayShots = awayShots;
        HomeShotsOnTarget = homeShotsOnTarget;
        AwayShotsOnTarget = awayShotsOnTarget;
    }

    public void UpdateCorners(int homeCorners, int awayCorners)
    {
        HomeCorners = homeCorners;
        AwayCorners = awayCorners;
    }

    public void UpdateFouls(int homeFouls, int awayFouls)
    {
        HomeFouls = homeFouls;
        AwayFouls = awayFouls;
    }

    public void UpdateOffsides(int homeOffsides, int awayOffsides)
    {
        HomeOffsides = homeOffsides;
        AwayOffsides = awayOffsides;
    }

    public void UpdateCards(int homeYellowCards, int awayYellowCards, int homeRedCards, int awayRedCards)
    {
        HomeYellowCards = homeYellowCards;
        AwayYellowCards = awayYellowCards;
        HomeRedCards = homeRedCards;
        AwayRedCards = awayRedCards;
    }

    public void UpdatePassing(int? homePasses, int? awayPasses, decimal? homePassAccuracy, decimal? awayPassAccuracy)
    {
        HomePasses = homePasses;
        AwayPasses = awayPasses;
        HomePassAccuracy = homePassAccuracy;
        AwayPassAccuracy = awayPassAccuracy;
    }

    public void UpdateDefensiveStats(int? homeTackles, int? awayTackles, int? homeInterceptions, int? awayInterceptions)
    {
        HomeTackles = homeTackles;
        AwayTackles = awayTackles;
        HomeInterceptions = homeInterceptions;
        AwayInterceptions = awayInterceptions;
    }

    public void UpdateOtherStats(int? homeSaves, int? awaySaves, int? homeCrosses, int? awayCrosses, int? homeDribbles, int? awayDribbles)
    {
        HomeSaves = homeSaves;
        AwaySaves = awaySaves;
        HomeCrosses = homeCrosses;
        AwayCrosses = awayCrosses;
        HomeDribbles = homeDribbles;
        AwayDribbles = awayDribbles;
    }
}
