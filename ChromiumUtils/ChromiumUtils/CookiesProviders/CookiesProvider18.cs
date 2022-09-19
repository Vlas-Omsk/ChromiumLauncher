using System;

namespace ChromiumLauncher.CookiesProviders
{
    internal sealed class CookiesProvider18 : CookiesProvider
    {
        protected override string CookiesRelativeDirectory { get; } = @"Default\Network";
        protected override string CreateQueryPath { get; } = "CreateQuery18.sql";
        protected override string InsertQueryPath { get; } = "InsertQuery18.sql";
    }
}
