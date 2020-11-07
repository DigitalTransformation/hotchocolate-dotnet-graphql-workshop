namespace GraphQL.Data
{
    using Microsoft.EntityFrameworkCore;

    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }
    }
}