namespace GraphQL.QueryType
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GraphQL.Data;
    using GraphQL.DataLoader;
    using HotChocolate;
    using Microsoft.EntityFrameworkCore;

    public class QuerySpeakers
    {
        /// <summary>
        /// Get instance of Speakers[].
        /// </summary>
        /// <param name="context">Database context of ApiContext.</param>
        /// <returns>Instance of Speakers context.</returns>
        public Task<List<Speaker>> GetSpeakers([ScopedService] ApiContext context) =>
            context.Speakers.ToListAsync();

        public Task<Speaker> GetSpeakerAsync(
            int id,
            GetSpeakerById dataLoader,
            CancellationToken token) =>
                dataLoader.LoadAsync(id, token);
    }
}