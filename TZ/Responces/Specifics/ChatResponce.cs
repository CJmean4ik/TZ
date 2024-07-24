public class ChatResponce
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public UserResponce WhoCreateChat { get; set; }
    public List<MessageResponce> Messages { get; set; }
}