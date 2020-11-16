namespace GraphQL.Mutations
{
    using GraphQL.Data;

    public class AddSpeakerPayload
    {
        public AddSpeakerPayload(Speaker speaker)
        {
            this.Speaker = speaker;
        }

        public Speaker Speaker { get; }
    }
}