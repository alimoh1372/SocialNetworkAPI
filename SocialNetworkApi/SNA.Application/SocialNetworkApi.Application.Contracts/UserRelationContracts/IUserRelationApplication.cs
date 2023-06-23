using _00_Framework.Application;

namespace SocialNetworkApi.Application.Contracts.UserRelationContracts;

public interface IUserRelationApplication
{
    OperationResult Create(CreateUserRelation command);


    /// <summary>
    /// accept the relationship request user A
    /// </summary>
    /// <param name="id">id of request</param>
    /// <returns></returns>
    OperationResult Accept(long id);


    /// <summary>
    /// decline the relationship request from user A
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    OperationResult Decline(long id);

    Task<List<UserWithRequestStatusVieModel>> GetAllUserWithRequestStatus(long currentUserId);

    /// <summary>
    /// Accept the relation in application of user that request sent to it
    /// Just the user that request sent to it can accept
    /// </summary>
    /// <param name="userIdRequestSentFromIt">User id that requested relationship</param>
    /// <param name="userIdRequestSentToIt">User id that request sent to it</param>
    /// <returns></returns>
    Task<OperationResult> Accept(long userIdRequestSentFromIt, long userIdRequestSentToIt);

    Task<List<UserWithRequestStatusVieModel>> GetFriendsOfUser(long userId);
    Task<int> GetNumberOfMutualFriend(long currentUserId, long friendUserId);
}
