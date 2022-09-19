using System;

namespace ChromiumLauncher.CookiesProviders
{
    internal sealed class CookiesProvider14 : CookiesProvider
    {
        protected override string CookiesRelativeDirectory { get; } = "Default";
        protected override string CreateQueryPath { get; } = "CreateQuery14.sql";
        protected override string InsertQueryPath { get; } = "InsertQuery14.sql";
    }
}
