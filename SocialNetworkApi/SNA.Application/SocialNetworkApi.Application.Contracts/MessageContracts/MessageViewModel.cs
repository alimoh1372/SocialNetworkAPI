namespace SocialNetworkApi.Application.Contracts.MessageContracts;

/// <summary>
/// A model to show the message on client side
/// </summary>
public class MessageViewModel
{
    public long Id { get; set; }
    public DateTimeOffset CreationDate { get; set; }

    public long FkFromUserId { get; set; }
    public string SenderFullName { get; set; }


    public long FkToUserId { get; set; }

    public string ReceiverFullName { get; set; }

    public string MessageContent { get; set; }

    public string FromUserProfilePicture { get; set; }

    public string ToUserProfilePicture { get; set; }
    //TODO:Add FromUser Image and
    //TODO:Implementing Like And ReadMessage operation
}
