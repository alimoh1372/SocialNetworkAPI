
using _00_Framework.Domain;
using SocialNetworkApi.Domain.UserAgg;

namespace SocialNetworkApi.Domain.MessageAgg;

/// <summary>
/// This class is using to send a message from user A to User B
/// </summary>
public class Message : EntityBase
{

    public long FkFromUserId { get; private set; }
    public User FromUser { get; private set; }

    public long FkToUserId { get; private set; }
    public User ToUser { get; private set; }

    public string MessageContent { get; private set; }

    public bool Like { get; private set; }

    public bool IsRead { get; private set; }

    /// <summary>
    /// To Create A message from User A To the User B
    /// </summary>
    /// <param name="fkFromUserId">Id of User A that sending a message</param>
    /// <param name="fkToUserId"> Id of user b that get a message</param>
    /// <param name="messageContent">A message exchange between User a to user b</param>
    public Message(long fkFromUserId, long fkToUserId, string messageContent)
    {
        FkFromUserId = fkFromUserId;
        FkToUserId = fkToUserId;
        MessageContent = messageContent;
        Like = false;
        IsRead = false;
    }

    /// <summary>
    /// To edit the message into <paramref name="messageContent"/>
    /// </summary>
    /// <param name="messageContent">New message text to edit</param>
    public void Edit(string messageContent)
    {
        if (CreationDate.AddMinutes(+3) < DateTimeOffset.Now)
            MessageContent = messageContent;
    }
    /// <summary>
    /// To like the message
    /// </summary>
    public void LikeMessage()
    {
        Like = true;
    }
    /// <summary>
    /// To unlike the message the default value is false
    /// </summary>
    public void UnLike()
    {
        Like = false;
    }
    /// <summary>
    /// To set the <see cref="MessageContent"/> is read by receiver user
    /// </summary>
    public void MessageAsRead()
    {
        IsRead = true;
    }
}
