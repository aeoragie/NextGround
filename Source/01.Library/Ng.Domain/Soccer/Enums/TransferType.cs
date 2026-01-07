namespace Ng.Domain.Soccer.Enums;

/// <summary>
/// 이적 유형
/// </summary>
public enum TransferType
{
    /// <summary>
    /// 완전 이적
    /// </summary>
    PermanentTransfer = 1,

    /// <summary>
    /// 임대 이적
    /// </summary>
    LoanTransfer = 2,

    /// <summary>
    /// 임대 (구매 옵션 포함)
    /// </summary>
    LoanWithOptionToBuy = 3,

    /// <summary>
    /// 임대 (의무 구매 조항 포함)
    /// </summary>
    LoanWithObligationToBuy = 4,

    /// <summary>
    /// 자유 이적
    /// </summary>
    FreeTransfer = 5,

    /// <summary>
    /// 유스 승격
    /// </summary>
    YouthPromotion = 6,

    /// <summary>
    /// 계약 연장
    /// </summary>
    ContractExtension = 7,

    /// <summary>
    /// 계약 해지
    /// </summary>
    ContractTermination = 8,

    /// <summary>
    /// 임대 복귀
    /// </summary>
    LoanReturn = 9
}
