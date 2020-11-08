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

        public static (SshClient TunnelClient, ForwardedPortLocal PortLocal) HookDatabase(
            SshClient client, string endpoint, uint port)
        {
            try
            {
                // Connect:SshTunnel
                client.HostKeyReceived += (sender, e) =>
                    e.CanTrust = true;
                client.Connect();
                _logger.LogInformation("Established tunnel to client.");

                // Tunnel:PortForward:Endpoint
                ForwardedPortLocal portLocal =
                    new ForwardedPortLocal(
                        "127.0.0.1",
                        endpoint,
                        port);
                client.AddForwardedPort(portLocal);
                portLocal.Exception += (sender, e) =>
                    _logger.LogInformation(e.Exception.ToString());
                portLocal.Start();

                // Return:SshTunnel:DatabaseHookClient
                return (client, portLocal);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not establish tunnel. {ex}");
            }
        }

        public static string ActiveSshConnections(SshClient client)
        {
            // State:SshdConnections
            string cmd = $"sudo lsof -i -n | grep sshd";
            string replConsole = client.RunCommand(cmd).Result;
            _logger.LogInformation(replConsole);

            return replConsole;
        }
    }
}