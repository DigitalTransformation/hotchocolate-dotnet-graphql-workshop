namespace GraphQL.Mutations
{
    using System.Threading.Tasks;
    using GraphQL.Data;
    using HotChocolate;

    public class MutationSpeaker
    {
        public async Task<AddSpeakerPayload> AddSpeakerAsync(
            AddSpeakerInput input,
            [Service] ApiContext context)
        {
            var speaker = new Speaker
            {
                Name = input.Name,
                Bio = input.Bio,
                WebSite = input.webSite,
            };

            context.Speakers.Add(speaker);
            await context.SaveChangesAsync()
                .ConfigureAwait(false);

            return new AddSpeakerPayload(speaker);
        }
    }
}