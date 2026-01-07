namespace Ng.Domain.Primitives;

/// <summary>
/// 감사 정보를 가진 엔티티 인터페이스
/// </summary>
public interface IAuditable
{
    DateTime CreatedAt { get; }
    string? CreatedBy { get; }
    DateTime? UpdatedAt { get; }
    string? UpdatedBy { get; }
}
