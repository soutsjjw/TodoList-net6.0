namespace TodoList.Domain.Base.Interface;

public interface IHasDomainEvent
{
    public List<DomainEvent> DomainEvents { get; set; }
}
