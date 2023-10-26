using TestPandape.Entity.Message;

namespace TestPandape.Entity.Experiences
{
    public class ExperienceResponse
    {
        public int? IdExperience{ get; set; }
        public int? IdCandidate { get; set; }
        public MessageResponse MessageResponse { get; set; } = new MessageResponse();
    }
}
