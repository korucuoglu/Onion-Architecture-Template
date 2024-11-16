namespace MyTemplate.Domain.Entities;

public class BaseEntity
{
}

public class BaseEntity<T>
{
    public  T Id { get; set; }
}

public interface IEntityWithDate
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

