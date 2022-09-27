namespace EnsimagCafet.Web.Models.Account
{
    public sealed class IndexViewModel
    {
        public bool HasPassword { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }
    }
}