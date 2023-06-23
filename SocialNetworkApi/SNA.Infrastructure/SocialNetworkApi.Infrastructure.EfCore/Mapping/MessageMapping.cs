using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkApi.Domain.MessageAgg;

namespace SocialNetworkApi.Infrastructure.EfCore.Mapping;


public class MessageMapping :IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");
        builder.HasKey(x => x.Id);
        //Design properties
        builder.Property(x => x.MessageContent).IsRequired();


        //Design self reference many to many 

        builder.HasOne(x => x.FromUser)
            .WithMany(x => x.FromMessages)
            .HasForeignKey(x => x.FkFromUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ToUser)
            .WithMany(x => x.ToMessages)
            .HasForeignKey(x => x.FkToUserId)
            .OnDelete(DeleteBehavior.NoAction);


        //design index for user a and b
        builder.HasIndex(x => new { x.FkFromUserId, x.FkToUserId });




    }
}
