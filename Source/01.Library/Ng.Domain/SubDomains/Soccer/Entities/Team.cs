using Ng.Domain.Enums;
using Ng.Domain.Primitives;
using Ng.Domain.Soccer.Entities;
using Ng.Domain.Soccer.ValueObjects;

namespace Ng.Domain.Soccer.Aggregates;

/// <summary>
/// 축구 팀 애그리게이트 루트
/// </summary>
public class Team : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string? FullName { get; private set; }
    public string? Abbreviation { get; private set; }
    public int? FoundedYear { get; private set; }

    public string? StadiumName { get; private set; }
    public int? StadiumCapacity { get; private set; }

    public string? LogoUrl { get; private set; }
    public string? PrimaryColor { get; private set; }
    public string? SecondaryColor { get; private set; }
    public string? WebsiteUrl { get; private set; }

    public TeamStatus Status { get; private set; } = TeamStatus.Active;

    public Guid? LeagueId { get; private set; }
    public League? League { get; private set; }

    public string? City { get; private set; }
    public string CountryCode { get; private set; } = string.Empty;

    private readonly List<Player> mPlayers = new();
    public IReadOnlyCollection<Player> Players => mPlayers.AsReadOnly();

    private readonly List<Match> mHomeMatches = new();
    public IReadOnlyCollection<Match> HomeMatches => mHomeMatches.AsReadOnly();

    private readonly List<Match> mAwayMatches = new();
    public IReadOnlyCollection<Match> AwayMatches => mAwayMatches.AsReadOnly();

    private readonly List<CoachingStaffMember> mCoachingStaff = new();
    public IReadOnlyCollection<CoachingStaffMember> CoachingStaff => mCoachingStaff.AsReadOnly();

    private readonly List<TeamStatistics> mStatistics = new();
    public IReadOnlyCollection<TeamStatistics> Statistics => mStatistics.AsReadOnly();

    private Team() { }

    public Team(string name, string countryCode) : base()
    {
        Name = name;
        CountryCode = countryCode;
    }

    public void UpdateBasicInfo(string name, string? fullName, string? abbreviation)
    {
        Name = name;
        FullName = fullName;
        Abbreviation = abbreviation;
    }

    public void UpdateStadiumInfo(string? stadiumName, int? capacity)
    {
        StadiumName = stadiumName;
        StadiumCapacity = capacity;
    }

    public void UpdateBranding(string? logoUrl, string? primaryColor, string? secondaryColor, string? websiteUrl)
    {
        LogoUrl = logoUrl;
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
        WebsiteUrl = websiteUrl;
    }

    public void UpdateLocation(string? city, string countryCode)
    {
        City = city;
        CountryCode = countryCode;
    }

    public void AssignToLeague(Guid leagueId)
    {
        LeagueId = leagueId;
    }

    public void ChangeStatus(TeamStatus status)
    {
        Status = status;
    }

    public void AddPlayer(Player player)
    {
        mPlayers.Add(player);
    }

    public void RemovePlayer(Player player)
    {
        mPlayers.Remove(player);
    }

    public void AddCoachingStaff(CoachingStaffMember staff)
    {
        mCoachingStaff.Add(staff);
    }

    public void RemoveCoachingStaff(CoachingStaffMember staff)
    {
        mCoachingStaff.Remove(staff);
    }
}
