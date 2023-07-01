using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApi.Application.Contracts.UserRelationContracts;

/// <summary>
/// To accept relation by user with id=<see cref="userIdRequestSentToIt"/>
/// that request is from user id=<see cref="userIdRequestSentFromIt"/>
/// </summary>
public class AcceptUserRelation
{
    [Required(AllowEmptyStrings = false)]
    public long userIdRequestSentFromIt { get; set; }

    [Required(AllowEmptyStrings = false)]
    public long userIdRequestSentToIt { get; set; }
}