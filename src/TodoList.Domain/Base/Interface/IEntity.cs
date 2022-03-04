namespace TodoList.Domain.Base.Interface;

public interface IEntity<T>
{
    public T Id { get; set; }
}