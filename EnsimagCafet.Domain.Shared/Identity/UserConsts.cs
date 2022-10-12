﻿using System.Text.RegularExpressions;

namespace EnsimagCafet.Domain.Shared.Identity
{
    public static class UserConsts
    {
        public const string SuperUserUserName = "super.user";
        public const string SuperUserEmail = "super.user@null.null";
        public static readonly Guid SuperUserId = Guid.Parse("88888888-0002-0001-bd02-a0500e0cc7f3");

        public const int UserNameMinLength = 7;
        public const int UserNameMaxLength = 64;
        public static readonly Regex UserNameRegex = new(@"^[a-z0-9]{3,}\.[a-z0-9]{3,}$");

        public const int PasswordHashMinLength = 32;
        public const int PasswordHashMaxLength = 256;

        public const string EmailSuffix = "@grenoble-inp.org";
    }
}