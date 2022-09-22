namespace EnsimagCafet.Web.Models.Account
{
    public sealed class RemoveLoginViewModel
    {
        public string? LoginProvider { get; set; }
        public string? ProviderKey { get; set; }
    }
}