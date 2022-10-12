using ChromiumLauncher.CookiesProviders;
using ProcessArguments;
using System;
using System.Diagnostics;

namespace ChromiumLauncher
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            ArgumentsDeserializer.Deserialize<Config>(args, Start);
        }

        private static async Task Start(Config config)
        {
            var userDataDirectory = GetRandomDirectory();
            Console.WriteLine("User data directory = " + userDataDirectory);

            var chromeArgs = $"--user-data-dir=\"{userDataDirectory}\" --bwsi --no-first-run {config.ChromeArgs}";

            if (config.CookiesPath != null)
            {
                Console.WriteLine("Loading cookies...");

                var cookieProviderType = Type.GetType("ChromiumLauncher.CookiesProviders.CookiesProvider" + config.CookiesStoreVersion);

                if (cookieProviderType == null)
                {
                    Console.WriteLine("Not supported cookies store version = " + config.CookiesStoreVersion);
                    return;
                }

                using var cookieProvider = (CookiesProvider)cookieProviderType
                    .GetConstructor(new Type[] { typeof(string) })
                    .Invoke(new object[] { config.ProfileCookiesRelativeDirectory });
                await cookieProvider.CreateAsync(userDataDirectory);
                using var cookieReader = new NetscapeCookieReader(config.CookiesPath);
                await cookieProvider.AddRangeAsync(cookieReader.GetAllCookies());

                Console.WriteLine("Cookies loaded");
            }

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo(config.ChromePath, chromeArgs)
            };

            process.Start();
            Console.WriteLine("Process started");

            process.WaitForExit();
            Console.WriteLine("Process exited");

            while (true)
            {
                try
                {
                    Directory.Delete(userDataDirectory, true);
                    break;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }

            Console.WriteLine("User data deleted");
            Console.WriteLine("Exit");
        }

        private static string GetRandomDirectory()
        {
            var path = Path.GetRandomFileName();
            var directoryInfo = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), path));

            return directoryInfo.FullName;
        }
    }
}