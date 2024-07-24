using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name).IsRequired();

            builder.HasMany<Message>(hm => hm.Messages)
                .WithOne(wo => wo.Chat)
                .HasForeignKey(fk => fk.ChatId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(ho => ho.WhoCreated)
                .WithMany(wm => wm.CreatedChats)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
