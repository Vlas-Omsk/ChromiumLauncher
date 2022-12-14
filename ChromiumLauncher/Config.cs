using ProcessArguments;
using System;

namespace ChromiumLauncher
{
    internal sealed class Config
    {
        [Required]
        public string ChromePath { get; private set; }
        public string ChromeArgs { get; private set; } = "";
        public string CookiesPath { get; private set; }
        public string ProfileCookiesRelativeDirectory { get; private set; } = "Default";
        [RequiredIf(nameof(IsCookiesStoreVersionRequired))]
        public int CookiesStoreVersion { get; private set; }

        private bool IsCookiesStoreVersionRequired()
        {
            return CookiesPath != null;
        }
    }
}
