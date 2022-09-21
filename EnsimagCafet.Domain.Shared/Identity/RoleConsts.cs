namespace EnsimagCafet.Domain.Shared.Identity
{
    public static class RoleConsts
    {
        public const string AdminRoleName = "Administrator";

        public const string MasterRoleName = "Master";

        public const string ManagerRoleName = "Manager";

        public const string CustomerRoleName = "Customer";

        public static readonly Guid AdminRoleId = Guid.Parse("88888888-0001-0001-83d9-bb4152f3299d");

        public static readonly Guid MasterRoleId = Guid.Parse("88888888-0001-0002-a04e-d0da21e72853");

        public static readonly Guid ManagerRoleId = Guid.Parse("88888888-0001-0003-8de1-ca156a62546d");

        public static readonly Guid CustomerRoleId = Guid.Parse("88888888-0001-0004-90ea-d03576334d72");
    }
}