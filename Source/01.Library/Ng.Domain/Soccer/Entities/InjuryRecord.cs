using Ng.Domain.Primitives;
using Ng.Domain.Soccer.Aggregates;
using Ng.Domain.Soccer.Enums;

namespace Ng.Domain.Soccer.Entities;

/// <summary>
/// 부상 기록 엔티티
/// </summary>
public class InjuryRecord : Entity
{
    public Guid PlayerId { get; private set; }
    public Player Player { get; private set; } = null!;

    public InjuryType InjuryType { get; private set; }
    public InjurySeverity Severity { get; private set; }
    public string BodyPart { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public DateTime InjuryDate { get; private set; }
    public DateTime? ExpectedReturnDate { get; private set; }
    public DateTime? ActualReturnDate { get; private set; }
    public int? DaysOut { get; private set; }

    public Guid? MatchId { get; private set; }
    public Match? Match { get; private set; }

    public bool IsRecovered { get; private set; }

    private InjuryRecord() { }

    public InjuryRecord(
        Guid playerId,
        InjuryType injuryType,
        InjurySeverity severity,
        string bodyPart,
        DateTime injuryDate)
    {
        PlayerId = playerId;
        InjuryType = injuryType;
        Severity = severity;
        BodyPart = bodyPart;
        InjuryDate = injuryDate;
    }

    public void SetExpectedReturnDate(DateTime date) => ExpectedReturnDate = date;
    public void SetDescription(string description) => Description = description;
    public void SetMatchId(Guid matchId) => MatchId = matchId;

    public void MarkAsRecovered(DateTime actualReturnDate)
    {
        IsRecovered = true;
        ActualReturnDate = actualReturnDate;
        DaysOut = (actualReturnDate - InjuryDate).Days;
    }
}
