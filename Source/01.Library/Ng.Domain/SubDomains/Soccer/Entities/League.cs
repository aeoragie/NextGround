using Ng.Domain.Primitives;
using Ng.Domain.Soccer.Entities;

namespace Ng.Domain.Soccer.Aggregates;

/// <summary>
/// 축구 리그 애그리게이트 루트
/// </summary>
public class League : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string? Abbreviation { get; private set; }
    public string CountryCode { get; private set; } = string.Empty;
    public int Level { get; private set; }
    public string? LogoUrl { get; private set; }
    public int? FoundedYear { get; private set; }
    public bool IsActive { get; private set; } = true;

    private readonly List<Season> mSeasons = new();
    public IReadOnlyCollection<Season> Seasons => mSeasons.AsReadOnly();

    private readonly List<Team> mTeams = new();
    public IReadOnlyCollection<Team> Teams => mTeams.AsReadOnly();

    private League() { }

    public League(string name, string countryCode, int level) : base()
    {
        Name = name;
        CountryCode = countryCode;
        Level = level;
    }

    public void UpdateInfo(string name, string? abbreviation, string? logoUrl)
    {
        Name = name;
        Abbreviation = abbreviation;
        LogoUrl = logoUrl;
    }

    public void ChangeLevel(int level)
    {
        Level = level;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void AddSeason(Season season)
    {
        mSeasons.Add(season);
    }

    public void AddTeam(Team team)
    {
        mTeams.Add(team);
    }

    public void RemoveTeam(Team team)
    {
        mTeams.Remove(team);
    }
}
