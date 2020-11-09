namespace GraphQL.Data
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Npgsql;

    public class ApiContext : DbContext
    {
        private readonly ILogger<ApiContext> _logger;

        private static IConfiguration _configuration =>
            Startup.Configuration;

        private static IConfigurationSection _dbSection =>
            _configuration.GetSection("DataBinding:Postgres");

        private static string _connectionStr;
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _logger.LogInformation($"Using connectionStr: {_connectionStr}");
            optionsBuilder
                .UseNpgsql(new NpgsqlConnection(_connectionStr));

            if (!optionsBuilder.IsConfigured || string.IsNullOrEmpty(_connectionStr))
            {
                _logger.LogInformation("Could not configure DbContext provider method. Check connectionString or method properties instantiated.");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}