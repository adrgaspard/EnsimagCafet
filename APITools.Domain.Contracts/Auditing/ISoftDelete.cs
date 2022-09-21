namespace APITools.Domain.Contracts.Auditing
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; }
    }
}