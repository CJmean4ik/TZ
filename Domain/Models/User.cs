using System.Text.RegularExpressions;
using Domain.Generals;

namespace Domain.Models
{
    public class User : IEntity<Guid>
    {
        private readonly List<Chat> _chats = new List<Chat>();
        private readonly List<Message> _messages = new List<Message>();

        public Guid Id { get; init; }
        public string Name { get; init; }

        public List<Chat> CreatedChats => _chats;
        public List<Message> SentMessages => _messages;

        public User() { }
        private User(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddChat(Chat chat) => _chats.Add(chat);
        public void AddMessage(Message message) => _messages.Add(message);

        public static ResultT<User> Create(Guid id, string name) 
        {
            if(id == Guid.Empty)
                return Result.Failure<User>(new Error(ErrorCodes.EmptyId, "User id must not be empty"));

            if (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name,"^[a-zA-Z0-9]+$"))
                return Result.Failure<User>(new Error(ErrorCodes.IncorrectString, "The name must not contain null, empty values, or only spaces"));

            var user = new User(id, name);
            return Result.Success<User>(user);
        }
    }
}
