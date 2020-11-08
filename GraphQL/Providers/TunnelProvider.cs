namespace GraphQL.Providers
{
    using System;
    using System.Net.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Renci.SshNet;

    public class TunnelProvider
    {
        private static readonly ILogger<TunnelProvider> _logger;

        private static IConfiguration _configuration =>
            Startup.Configuration;

        private static IConfigurationSection _sshConfig =
            _configuration.GetSection("DataBinding:SshTunnel");

        public static SshClient CreateClient()
        {
            // Create Instance T<SshClient>
            PrivateKeyFile keyFile = new PrivateKeyFile($@"{_sshConfig["KeyPair"]}");
            return new SshClient(
                _sshConfig["Endpoint"],
                short.Parse(_sshConfig["Port"]),
                _sshConfig["AuthUser"],
                keyFile)
            {
                ConnectionInfo =
                {
                    Timeout = new TimeSpan(0, 0, 30),
                },
            };
        }
    }
}