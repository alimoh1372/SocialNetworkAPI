using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApi.Application.Contracts.UserRelationContracts;

/// <summary>
/// To create request of User relation from user A to user B
/// User A is current User
/// </summary>
public class CreateUserRelation
{

    [DisplayName("Request From User A")]
    [Range(1, long.MaxValue)]
    public long FkUserAId { get; set; }

    [DisplayName("Request To User B")]
    [Range(1, long.MaxValue)]
    public long FkUserBId { get; set; }

    [DisplayName("Request message")]
    [Required]
    [StringLength(100)]
    public string RelationRequestMessage { get; set; }

}
