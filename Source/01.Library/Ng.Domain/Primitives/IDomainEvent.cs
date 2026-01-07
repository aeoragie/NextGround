namespace Ng.Domain.Primitives;

/// <summary>
/// 도메인 이벤트 인터페이스
/// </summary>
public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredAt { get; }
}
