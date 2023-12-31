﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApi.Application.Contracts.MessageContracts;

/// <summary>
/// Command model of Edit message
/// </summary>
public class EditMessage
{
    [Required]
    public long Id { get; set; }
    [Required]
    public long FkFromUserId { get; set; }

    public long FkToUserId { get; set; }
    [DisplayName("Message text")]
    [Required]
    public string MessageContent { get; set; }
}

