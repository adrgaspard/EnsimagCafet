namespace APITools.Domain.Contracts.Auditing
{
    public interface IHasModificationTime
    {
        DateTime LastModificationTime { get; }
    }
}
