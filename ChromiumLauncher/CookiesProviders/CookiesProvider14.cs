using System;

namespace ChromiumLauncher.CookiesProviders
{
    internal sealed class CookiesProvider14 : CookiesProvider
    {
        public CookiesProvider14(string cookiesRelativeDirectory) : base(cookiesRelativeDirectory)
        {
        }

        protected override string CreateQueryPath { get; } = "CreateQuery14.sql";
        protected override string InsertQueryPath { get; } = "InsertQuery14.sql";
    }
}
