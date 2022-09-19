using ChromiumLauncher.CookiesProviders;
using ProcessArguments;
using System;
using System.Diagnostics;
using System.Net;

namespace ChromiumLauncher
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            ArgumentsDeserializer.Deserialize<Config>(args, Start);
        }

        private static async Task Start(Config arguments)
        {
            var userDataDirectory = GetRandomDirectory();
            Console.WriteLine("User data directory = " + userDataDirectory);

            var chromeArgs = $"--user-data-dir=\"{userDataDirectory}\" --bwsi --no-first-run {arguments.ChromeArgs}";

            if (arguments.CookiesPath != null)
            {
                Console.WriteLine("Loading cookies...");

                using var cookieProvider = new CookiesProvider18();
                await cookieProvider.CreateAsync(userDataDirectory);
                await cookieProvider.AddRangeAsync(ReadCookies(arguments.CookiesPath));

                Console.WriteLine("Cookies loaded");
            }

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo(arguments.ChromePath, chromeArgs)
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

        private static IEnumerable<Cookie> ReadCookies(string cookiesPath)
        {
            using var reader = new StreamReader(cookiesPath);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split("\t");

                var domain = parts[0];
                // var flag = bool.Parse(parts[1]);
                var path = parts[2];
                var secure = bool.Parse(parts[3]);
                var expiration = TimeConverter.FromUnixTimestamp(long.Parse(parts[4]));
                var name = parts[5];
                var value = parts[6];

                Cookie cookie = null;

                try
                {
                    cookie = new Cookie()
                    {
                        Domain = domain,
                        Path = path,
                        Secure = secure,
                        Expires = expiration,
                        Name = name,
                        Value = value
                    };
                }
                catch
                {
                }

                if (cookie != null)
                    yield return cookie;
            }
        }

        private static string GetRandomDirectory()
        {
            var path = Path.GetRandomFileName();
            var directoryInfo = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), path));

            return directoryInfo.FullName;
        }
    }
}