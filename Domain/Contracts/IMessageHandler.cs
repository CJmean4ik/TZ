namespace Domain.Contracts;

public interface IMessageHandler
{
    Task ReceiveMessage(string userName, string message);
}