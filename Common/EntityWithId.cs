using System.ComponentModel.DataAnnotations;

namespace Common;

public class EntityWithId: EntityWithGenericId<int>
{
    
}

public class EntityWithGenericId<T>
{
    [Key]
    public T Id { get; set; }
}