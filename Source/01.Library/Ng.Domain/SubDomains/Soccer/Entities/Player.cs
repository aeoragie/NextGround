using Ng.Domain.Enums;
using Ng.Domain.Primitives;
using Ng.Domain.SubDomains.Soccer.Enums;
using Ng.Domain.SubDomains.Soccer.Values;

namespace Ng.Domain.SubDomains.Soccer.Entities;

/// <summary>
/// 축구 선수 애그리게이트 루트
/// </summary>
public class Player : AggregateRoot
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();

    public int? JerseyNumber { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public int Age => DateTime.Today.Year - DateOfBirth.Year -
        (DateTime.Today.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);

    public string Nationality { get; private set; } = string.Empty;
    public SoccerPosition PrimaryPosition { get; private set; }
    public List<SoccerPosition> SecondaryPositions { get; private set; } = new();

    public int? Height { get; private set; }
    public int? Weight { get; private set; }
    public PreferredFoot PreferredFoot { get; private set; } = PreferredFoot.Right;
    public PlayerStatus Status { get; private set; } = PlayerStatus.Active;

    public DateTime? ContractStartDate { get; private set; }
    public DateTime? ContractEndDate { get; private set; }
    public decimal? MarketValue { get; private set; }
    public decimal? Salary { get; private set; }
    public string? PhotoUrl { get; private set; }

    public Guid? TeamId { get; private set; }
    public Team? Team { get; private set; }

    private readonly List<PlayerStatistics> mStatistics = new();
    public IReadOnlyCollection<PlayerStatistics> Statistics => mStatistics.AsReadOnly();

    private readonly List<MatchLineup> mMatchLineups = new();
    public IReadOnlyCollection<MatchLineup> MatchLineups => mMatchLineups.AsReadOnly();

    private readonly List<MatchEvent> mMatchEvents = new();
    public IReadOnlyCollection<MatchEvent> MatchEvents => mMatchEvents.AsReadOnly();

    private readonly List<InjuryRecord> mInjuryRecords = new();
    public IReadOnlyCollection<InjuryRecord> InjuryRecords => mInjuryRecords.AsReadOnly();

    private readonly List<TransferRecord> mTransferRecords = new();
    public IReadOnlyCollection<TransferRecord> TransferRecords => mTransferRecords.AsReadOnly();

    private Player() { }

    public Player(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string nationality,
        SoccerPosition primaryPosition) : base()
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Nationality = nationality;
        PrimaryPosition = primaryPosition;
    }

    public void UpdatePersonalInfo(string firstName, string lastName, string nationality)
    {
        FirstName = firstName;
        LastName = lastName;
        Nationality = nationality;
    }

    public void UpdatePhysicalInfo(int? height, int? weight, PreferredFoot preferredFoot)
    {
        Height = height;
        Weight = weight;
        PreferredFoot = preferredFoot;
    }

    public void UpdatePosition(SoccerPosition primaryPosition, List<SoccerPosition>? secondaryPositions = null)
    {
        PrimaryPosition = primaryPosition;
        if (secondaryPositions != null)
        {
            SecondaryPositions = secondaryPositions;
        }
    }

    public void AssignToTeam(Guid teamId, int? jerseyNumber = null)
    {
        TeamId = teamId;
        JerseyNumber = jerseyNumber;
    }

    public void UpdateContract(DateTime startDate, DateTime endDate, decimal? salary = null)
    {
        ContractStartDate = startDate;
        ContractEndDate = endDate;
        Salary = salary;
    }

    public void UpdateMarketValue(decimal value)
    {
        MarketValue = value;
    }

    public void ChangeStatus(PlayerStatus status)
    {
        Status = status;
    }

    public void AddInjuryRecord(InjuryRecord injury)
    {
        mInjuryRecords.Add(injury);
        if (!injury.IsRecovered)
        {
            Status = PlayerStatus.Injured;
        }
    }

    public void AddTransferRecord(TransferRecord transfer)
    {
        mTransferRecords.Add(transfer);
        TeamId = transfer.ToTeamId;
    }
}
