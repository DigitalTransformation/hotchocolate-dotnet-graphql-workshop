namespace GraphQL.Data
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class ApiContext : DbContext
    {
        private static IConfiguration _configuration =>
            Startup.Configuration;

        private static IConfigurationSection _section =>
            _configuration.GetSection("DataBinding:Postgres");

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = _section["Default"];

            optionsBuilder
                .UseNpgsql(connection);

            if (!optionsBuilder.IsConfigured)
            {
                Console.WriteLine("[ApiContext]: Could not configure DbContext provider method. Check connectionString or method properties instantiated.");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}