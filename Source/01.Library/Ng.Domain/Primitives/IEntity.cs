namespace Ng.Domain.Primitives;

/// <summary>
/// 엔티티 인터페이스
/// </summary>
public interface IEntity
{
    Guid Id { get; }
}

/// <summary>
/// 제네릭 ID를 가진 엔티티 인터페이스
/// </summary>
public interface IEntity<TId> where TId : notnull
{
    TId Id { get; }
}
