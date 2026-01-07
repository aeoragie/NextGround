namespace Ng.Domain.Primitives;

/// <summary>
/// 소프트 삭제를 지원하는 엔티티 인터페이스
/// </summary>
public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    string? DeletedBy { get; }
}
