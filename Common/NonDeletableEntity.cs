namespace Common;

public class NonDeletableEntity : NonDeletableGenericEntity<int>
{
}

public class NonDeletableGenericEntity<T> : EntityWithGenericId<T>
{
    public bool IsDeleted { get; set; } = false;
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime? LastEditDate { get; set; }
}