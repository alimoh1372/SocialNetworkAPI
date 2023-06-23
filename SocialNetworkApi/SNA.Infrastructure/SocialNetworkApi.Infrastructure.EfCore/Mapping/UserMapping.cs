using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkApi.Domain.UserAgg;

namespace SocialNetworkApi.Infrastructure.EfCore.Mapping;


[EntityTypeConfiguration(typeof(User))]
public class UserMapping 
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(500);
        builder.Property(x => x.BirthDay).IsRequired();
        builder.Property(x => x.Password).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.AboutMe).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.ProfilePicture).IsRequired().HasMaxLength(2000);



        //Define a self referencing many to many with UserRelation entity
        builder.HasMany(x => x.UserARelations)
            .WithOne(x => x.UserA)
            .HasForeignKey(x => x.FkUserAId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.UserBRelations)
            .WithOne(x => x.UserB)
            .HasForeignKey(x => x.FkUserBId)
            .OnDelete(DeleteBehavior.Restrict);


        //Define a self referencing many to many with message
        builder.HasMany(x => x.FromMessages)
            .WithOne(x => x.FromUser)
            .HasForeignKey(x => x.FkFromUserId);

        builder.HasMany(x => x.ToMessages)
            .WithOne(x => x.ToUser)
            .HasForeignKey(x => x.FkToUserId);



        //Define an index to email 
        builder.HasIndex(x => x.Email).IsUnique();
    }
}
