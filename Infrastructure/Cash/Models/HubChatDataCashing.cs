namespace Infrastructure.Cash.Models;

[Serializable]
public class HubChatDataCashing
{
    public Guid ChatId { get; set; }
    public string ChatRoomName { get; set; }
    
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    
}