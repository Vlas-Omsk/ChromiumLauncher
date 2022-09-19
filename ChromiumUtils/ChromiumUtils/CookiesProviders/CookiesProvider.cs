using System;
using System.Data.SQLite;
using System.Net;
using System.Text;

namespace ChromiumLauncher.CookiesProviders
{
    internal abstract class CookiesProvider : IDisposable
    {
        private SQLiteConnection _connection;

        ~CookiesProvider()
        {
            Dispose();
        }

        protected abstract string CookiesRelativeDirectory { get; }
        protected abstract string CreateQueryPath { get; }
        protected abstract string InsertQueryPath { get; }

        public async Task CreateAsync(string userDataDirectory)
        {
            var createQueryTask = File.ReadAllTextAsync(CreateQueryPath);

            var cookiesDirectory = Path.Combine(userDataDirectory, CookiesRelativeDirectory);

            Directory.CreateDirectory(cookiesDirectory);

            _connection = new SQLiteConnection(@$"Data Source=""{Path.Combine(cookiesDirectory, "Cookies")}""; Version=3; New=True;");
            _connection.Open();

            using var createCommand = new SQLiteCommand(await createQueryTask, _connection);
            createCommand.ExecuteNonQuery();
        }

        public async Task AddRangeAsync(IEnumerable<Cookie> cookies)
        {
            var insertQueryTask = File.ReadAllTextAsync(InsertQueryPath);

            using var command = new SQLiteCommand(_connection);
            var queryBuilder = new StringBuilder();
            var creationTime = TimeConverter.ToUtcTime(DateTime.Now).ToString();
            var i = 0;

            foreach (var cookie in cookies)
            {
                queryBuilder.AppendLine(
                    (await insertQueryTask)
                        .Replace("@creationUtc", $"@creationUtc{i}")
                        .Replace("@hostKey", $"@hostKey{i}")
                        .Replace("@name", $"@name{i}")
                        .Replace("@value", $"@value{i}")
                        .Replace("@path", $"@path{i}")
                        .Replace("@expiresUtc", $"@expiresUtc{i}")
                        .Replace("@isSecure", $"@isSecure{i}")
                        .Replace("@sourcePort", $"@sourcePort{i}")
                );

                command.Parameters.AddWithValue($"@creationUtc{i}", creationTime);
                command.Parameters.AddWithValue($"@hostKey{i}", cookie.Domain);
                command.Parameters.AddWithValue($"@name{i}", cookie.Name);
                command.Parameters.AddWithValue($"@value{i}", cookie.Value);
                command.Parameters.AddWithValue($"@path{i}", cookie.Path);
                command.Parameters.AddWithValue($"@expiresUtc{i}", TimeConverter.ToUtcTime(cookie.Expires));
                command.Parameters.AddWithValue($"@isSecure{i}", cookie.Secure ? 1 : 0);
                command.Parameters.AddWithValue($"@sourcePort{i}", cookie.Secure ? 443 : 80);

                i++;

                if (i == 1000)
                {
                    command.CommandText = queryBuilder.ToString();
                    var task = command.ExecuteNonQueryAsync();

                    queryBuilder.Clear();
                    i = 0;

                    await task;
                }
            }

            if (i != 0)
            {
                command.CommandText = queryBuilder.ToString();
                command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
