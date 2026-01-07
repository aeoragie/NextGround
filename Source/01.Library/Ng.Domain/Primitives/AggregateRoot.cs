namespace Ng.Domain.Primitives;

/// <summary>
/// 애그리게이트 루트 기본 클래스
/// </summary>
public abstract class AggregateRoot : Entity, IAggregateRoot
{
    private readonly List<IDomainEvent> mDomainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => mDomainEvents.AsReadOnly();

    protected AggregateRoot()
    {
    }

    protected AggregateRoot(Guid id) : base(id)
    {
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        mDomainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        mDomainEvents.Clear();
    }
}
