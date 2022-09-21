namespace EnsimagCafet.EntityFrameworkCore.DbMigrator
{
    public record ApplicationDbDataSeedingOptions
    {
        public bool ChecksSuperUser { get; init; } = true;

        public bool ChecksAllRoles { get; init; } = true;

        public string SuperUserDefaultPassword { get; init; } = "";
    }
}