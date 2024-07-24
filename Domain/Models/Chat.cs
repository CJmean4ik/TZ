using Domain.Generals;

namespace Domain.Models
{
    public class Chat  : IEntity<Guid>
    {
        private readonly List<Message> _messages = new List<Message>();

        public Guid Id { get; init; }
        public string Name { get; init; }
        public Guid? UserId { get; init; }
        public User? WhoCreated { get; init; }

        public List<Message> Messages => _messages;

        public Chat() { }
        private Chat(Guid id, string name, User whoCreated, Guid? userId)
        {
            Id = id;
            Name = name;
            WhoCreated = whoCreated;
            UserId = userId;
        }

        public void AddMessage(Message message) => _messages.Add(message);

        public static ResultT<Chat> Create(Guid id, string name, User? user = null, Guid? userId = null)
        {
            if (id == Guid.Empty)
                return Result.Failure<Chat>(new Error(ErrorCodes.EmptyId, "Chat id must not be empty"));

            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Chat>(new Error(ErrorCodes.IncorrectString, "The name must not contain null, empty values, or only spaces"));         

            var chat = new Chat(id, name, user, userId);
            return Result.Success<Chat>(chat);
        }
    }
}
