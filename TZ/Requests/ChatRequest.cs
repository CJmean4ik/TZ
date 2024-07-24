namespace Api.Requests;

public class ChatRequest
{
    public string ChatName { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
}