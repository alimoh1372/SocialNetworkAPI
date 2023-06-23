using _00_Framework.Application;

namespace SocialNetworkApi.Application.Contracts.UserRelationContracts;

/// <summary>
/// An Dto that show user some info with the request statuse
/// </summary>
public class UserWithRequestStatusVieModel
{
    //User that request sent to 
    public long UserId { get; set; }
    //User that request sent to
    public string Name { get; set; }

    public string LastName { get; set; }

    //Status of request
    public RequestStatus RequestStatusNumber { get; set; }
    public DateTimeOffset TimeOffset { get; set; }
    public string ProfilePicture { get; set; }
    public string RelationRequestMessage { get; set; }
    public int MutualFriendNumber { get; set; }
}
