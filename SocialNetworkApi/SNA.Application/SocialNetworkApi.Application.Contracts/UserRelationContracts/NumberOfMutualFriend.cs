using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApi.Application.Contracts.UserRelationContracts;

/// <summary>
/// There is two id first is for 
/// </summary>
public class NumberOfMutualFriend
{
    [Required(AllowEmptyStrings = false)]
    public long CurrentUserId { get; set; }

    [Required(AllowEmptyStrings = false)]
    public long FriendUserId { get; set; }
}