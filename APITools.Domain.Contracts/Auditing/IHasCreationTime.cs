namespace APITools.Domain.Contracts.Auditing
{
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; }
    }
}
