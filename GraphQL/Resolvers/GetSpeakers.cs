namespace GraphQL.Resolvers
{
    using System.Linq;
    using GraphQL.Data;
    using HotChocolate;

    public class GetSpeakers
    {
        /// <summary>
        /// Get instance of Speakers[].
        /// </summary>
        /// <param name="context">Database context of ApiContext.</param>
        /// <returns>Instance of Speakers context.</returns>
        public IQueryable<Speaker> ISpeakersQueryable([Service] ApiContext context) =>
            context.Speakers;
    }
}