using ProcessArguments;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChromiumLauncher
{
    internal sealed class Config
    {
        [Required]
        public string ChromePath { get; private set; }
        public string ChromeArgs { get; private set; } = "";
        public string CookiesPath { get; private set; }
        [RequiredIf("IsCookiesStoreVersionRequired")]
        public int CookiesStoreVersion { get; private set; }

        private bool IsCookiesStoreVersionRequired()
        {
            return CookiesPath != null;
        }
    }
}
