namespace GraphQL.Data
{
    using GraphQL.Providers;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Npgsql;
    using Renci.SshNet;

    public class ApiContext : DbContext
    {
        private static ILogger _logger => LogProvider.CreateLogger<ApiContext>();

        private static IConfiguration _configuration =>
            Startup.Configuration;

        private static IConfigurationSection _dbSection =>
            _configuration.GetSection("DataBinding:Postgres");

        private static string _connectionStr;

        private static SshClient _client;

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
            // Build Default ConnStr
            string endpoint = new NpgsqlConnectionStringBuilder(_dbSection["ConnectionString"]).Host;
            _logger.LogInformation($"Buffer endpoint: {endpoint}");
            uint port = uint.Parse(_dbSection["Port"]);

            // Tunnel:SshClient DatabaseHook
            _client = TunnelProvider.CreateClient();
            (SshClient TunnelClient, ForwardedPortLocal PortLocal) tunnel =
                TunnelProvider.HookDatabase(_client, endpoint, port);
            _client = tunnel.TunnelClient;

            // Check:Active SshClientConnections
            TunnelProvider.ActiveSshConnections(_client);

            // Check:Tunnel WebPortForwardState
            TunnelProvider.SshPortForwardState(_client);

            // Set:Database Connection String
            _connectionStr =
                new NpgsqlConnectionStringBuilder(_dbSection["ConnectionString"])
                {
                    Host = tunnel.PortLocal.BoundHost,
                    Port = (int)tunnel.PortLocal.BoundPort,
                    CommandTimeout = 30,
                    KeepAlive = 30,
                }.ToString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(new NpgsqlConnection(_connectionStr));

            if (!optionsBuilder.IsConfigured || string.IsNullOrEmpty(_connectionStr))
            {
                _logger.LogInformation("Could not configure DbContext provider method. Check connectionString or method properties instantiated.");
            }

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Speaker> Speakers { get; set; }
    }
}