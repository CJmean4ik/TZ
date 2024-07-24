using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
           builder.HasKey(x => x.Id);
           builder.Property(x => x.Name).IsRequired();

            builder.HasMany<Chat>(hm => hm.CreatedChats)
                    .WithOne(wo => wo.WhoCreated)
                    .HasForeignKey(fk => fk.UserId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany<Message>(hm => hm.SentMessages)
                    .WithOne(wo => wo.User)
                    .HasForeignKey(fk => fk.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
