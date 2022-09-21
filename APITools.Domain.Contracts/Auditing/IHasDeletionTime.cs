namespace APITools.Domain.Contracts.Auditing
{
    public interface IHasDeletionTime : ISoftDelete
    {
        DateTime? DeletionTime { get; }
    }
}
