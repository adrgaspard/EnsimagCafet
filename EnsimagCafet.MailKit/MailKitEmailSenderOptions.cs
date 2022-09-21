namespace EnsimagCafet.MailKit
{
    public sealed class MailKitEmailSenderOptions
    {
        public const string MailKitSectionName = "MailKit";

        public string SenderEmail { get; set; } = "";

        public string SenderName { get; set; } = "";

        public string SenderPassword { get; set; } = "";

        public string SmtpHost { get; set; } = "";

        public int SmtpPort { get; set; }

        public bool UseSsl { get; set; }

        public bool CheckCertificateRevocation { get; set; }

        public MailContentType ContentType { get; set; }
    }
}
