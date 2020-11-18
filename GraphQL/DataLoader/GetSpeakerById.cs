namespace GraphQL.DataLoader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GraphQL.Data;
    using GreenDonut;
    using HotChocolate.DataLoader;
    using Microsoft.EntityFrameworkCore;

    public class GetSpeakerById : BatchDataLoader<int, Speaker>
    {
        private readonly IDbContextFactory<ApiContext> _contextFactory;

        public GetSpeakerById(
            IBatchScheduler batchScheduler,
            IDbContextFactory<ApiContext> contextFactory)
            : base(batchScheduler)
        {
            this._contextFactory = contextFactory ??
                throw new ArgumentNullException(nameof(contextFactory));
        }

        protected override async Task<IReadOnlyDictionary<int, Speaker>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            await using ApiContext apiContext =
                this._contextFactory.CreateDbContext();

            return await apiContext
                .Speakers
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}