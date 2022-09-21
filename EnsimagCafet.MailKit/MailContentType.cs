namespace EnsimagCafet.MailKit
{
    public enum MailContentType
    {
        Plain = 0,
        Text = Plain,
        Flowed = 1,
        Html = 2,
        Enriched = 3,
        CompressedRichText = 4,
        RichText = 5
    }
}