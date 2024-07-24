using Domain.Generals;

namespace Domain.Models
{
    public class Message : IEntity<Guid>
    {
        public Guid Id { get; init; }
        public string Text { get; init; }

        public Guid UserId { get; init; }
        public User User { get; init; }

        public Guid ChatId { get; init; }
        public Chat Chat { get; init; }

        public Message() { }

        private Message(Guid id, string text, User whoSend, Chat chat)
        {
            Id = id;
            Text = text;
            User = whoSend;
            UserId = whoSend.Id;
            Chat = chat;
            ChatId = chat.Id;
        }

        public static ResultT<Message> Create(Guid id, string text, User whoSend, Chat chat)
        {
            if (id == Guid.Empty)
                return Result.Failure<Message>(new Error(ErrorCodes.EmptyId, "Message id must not be empty"));

            if (string.IsNullOrWhiteSpace(text))
                return Result.Failure<Message>(new Error(ErrorCodes.IncorrectString, "The text must not contain null, empty values, or only spaces"));

            if (whoSend is null)
                return Result.Failure<Message>(new Error(ErrorCodes.ValueNull, "The message must be assigned a sender"));
           
            if (chat is null)
                return Result.Failure<Message>(new Error(ErrorCodes.ValueNull, "The message should be stuck behind the chat"));

            var message = new Message(id, text, whoSend, chat);
            return Result.Success<Message>(message);
        }

    }
}
