using System;
using System.Web;

namespace ChromiumLauncher
{
    internal sealed class NetscapeCookieReader : IDisposable
    {
        private readonly StreamReader _reader;

        public NetscapeCookieReader(string path)
        {
            _reader = new StreamReader(path);
        }

        ~NetscapeCookieReader()
        {
            Dispose();
        }

        public IEnumerable<ChromiumCookie> GetAllCookies()
        {
            string line;

            while ((line = _reader.ReadLine()) != null)
            {
                var parts = line.Split("\t");

                var name = parts[5];

                if (string.IsNullOrEmpty(name))
                    continue;

                var domain = parts[0];
                var path = parts[2];
                var secure = bool.Parse(parts[3]);
                DateTime expiration;
                try
                {
                    expiration = TimeConverter.FromUnixTimestamp(long.Parse(parts[4]));
                }
                catch (ArgumentOutOfRangeException)
                {
                    expiration = DateTime.MinValue;
                }
                var value = parts[6];

                yield return new ChromiumCookie()
                {
                    Domain = domain,
                    Path = path,
                    Secure = secure,
                    Expires = expiration,
                    Name = name,
                    Value = HttpUtility.UrlEncode(value)
                };
            }
        }

        public void Dispose()
        {
            _reader.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
