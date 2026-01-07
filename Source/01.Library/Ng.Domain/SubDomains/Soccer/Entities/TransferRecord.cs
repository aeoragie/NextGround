using Ng.Domain.Common;
using Ng.Domain.SubDomains.Soccer.Enums;

namespace Ng.Domain.SubDomains.Soccer.Entities;

/// <summary>
/// 이적 기록 엔티티
/// </summary>
public class TransferRecord : Entity
{
    public Guid PlayerId { get; private set; }
    public Player Player { get; private set; } = null!;

    public Guid? FromTeamId { get; private set; }
    public Team? FromTeam { get; private set; }

    public Guid? ToTeamId { get; private set; }
    public Team? ToTeam { get; private set; }

    public TransferType TransferType { get; private set; }
    public decimal? TransferFee { get; private set; }
    public int? ContractLength { get; private set; }

    public DateTime TransferDate { get; private set; }
    public string? Season { get; private set; }
    public string? TransferWindow { get; private set; }
    public string? AdditionalClauses { get; private set; }
    public bool IsOfficial { get; private set; } = true;

    private TransferRecord() { }

    public TransferRecord(
        Guid playerId,
        TransferType transferType,
        DateTime transferDate,
        Guid? fromTeamId = null,
        Guid? toTeamId = null)
    {
        PlayerId = playerId;
        TransferType = transferType;
        TransferDate = transferDate;
        FromTeamId = fromTeamId;
        ToTeamId = toTeamId;
    }

    public void SetTransferFee(decimal fee) => TransferFee = fee;
    public void SetContractLength(int years) => ContractLength = years;

    public void SetSeasonInfo(string season, string? transferWindow = null)
    {
        Season = season;
        TransferWindow = transferWindow;
    }

    public void SetAdditionalClauses(string clauses) => AdditionalClauses = clauses;
    public void MarkAsOfficial() => IsOfficial = true;
    public void MarkAsUnofficial() => IsOfficial = false;
}
