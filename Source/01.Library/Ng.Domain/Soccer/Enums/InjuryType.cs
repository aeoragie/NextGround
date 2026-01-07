namespace Ng.Domain.Soccer.Enums;

/// <summary>
/// 부상 유형
/// </summary>
public enum InjuryType
{
    /// <summary>
    /// 근육 부상
    /// </summary>
    Muscle = 1,

    /// <summary>
    /// 인대 부상
    /// </summary>
    Ligament = 2,

    /// <summary>
    /// 골절
    /// </summary>
    Fracture = 3,

    /// <summary>
    /// 뇌진탕
    /// </summary>
    Concussion = 4,

    /// <summary>
    /// 염좌
    /// </summary>
    Sprain = 5,

    /// <summary>
    /// 긴장 (근육 긴장)
    /// </summary>
    Strain = 6,

    /// <summary>
    /// 타박상
    /// </summary>
    Bruise = 7,

    /// <summary>
    /// 찰과상/열상
    /// </summary>
    Cut = 8,

    /// <summary>
    /// 질병
    /// </summary>
    Illness = 9,

    /// <summary>
    /// 기타
    /// </summary>
    Other = 99
}
