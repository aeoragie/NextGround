namespace Ng.Domain.Primitives;

/// <summary>
/// 도메인 이벤트 기본 클래스
/// </summary>
public abstract record DomainEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
