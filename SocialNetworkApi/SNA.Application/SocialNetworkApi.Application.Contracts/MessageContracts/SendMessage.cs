using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApi.Application.Contracts.MessageContracts;

/// <summary>
/// A Command model to send a message from user with Id=<see cref="FkFromUserId"/> to User with Id=<see cref="FkToUserId"/>
/// </summary>
public class SendMessage
{
    [DisplayName("Message from")]
    [Range(1, long.MaxValue)]
    public long FkFromUserId { get; set; }

    [DisplayName("Message to")]
    [Range(1, long.MaxValue)]
    public long FkToUserId { get; set; }

    [DisplayName("Message text")]
    [Required]
    public string MessageContent { get; set; }
}