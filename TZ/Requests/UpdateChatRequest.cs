namespace Api.Requests
{
    public class UpdateChatRequest
    {
        public Guid ChatId { get; set; }
        public string NewName { get; set; }
    }
}
