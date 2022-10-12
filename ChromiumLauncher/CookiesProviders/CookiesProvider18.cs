using System;

namespace ChromiumLauncher.CookiesProviders
{
    internal sealed class CookiesProvider18 : CookiesProvider
    {
        public CookiesProvider18(string cookiesRelativeDirectory) : base(cookiesRelativeDirectory)
        {
        }

        protected override string CreateQueryPath { get; } = "CreateQuery18.sql";
        protected override string InsertQueryPath { get; } = "InsertQuery18.sql";
    }
}
