using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApi.Application.Contracts.MessageContracts;

/// <summary>
/// Command model of Edit message
/// </summary>
public class EditMessage
{
    public long Id { get; set; }
    public long FkFromUserId { get; set; }
    public long FkToUserId { get; set; }
    [DisplayName("Message text")]
    [Required]
    public string MessageContent { get; set; }
}

