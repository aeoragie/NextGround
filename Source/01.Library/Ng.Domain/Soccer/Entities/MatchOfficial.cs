using Ng.Domain.Primitives;
using Ng.Domain.Soccer.Aggregates;

namespace Ng.Domain.Soccer.Entities;

/// <summary>
/// 경기 심판 엔티티
/// </summary>
public class MatchOfficial : Entity
{
    public Guid MatchId { get; private set; }
    public Match Match { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public string? Nationality { get; private set; }
    public string? LicenseLevel { get; private set; }

    private MatchOfficial() { }

    public MatchOfficial(Guid matchId, string name, string role)
    {
        MatchId = matchId;
        Name = name;
        Role = role;
    }

    public void UpdateInfo(string name, string role)
    {
        Name = name;
        Role = role;
    }

    public void SetNationality(string nationality) => Nationality = nationality;
    public void SetLicenseLevel(string licenseLevel) => LicenseLevel = licenseLevel;
}
