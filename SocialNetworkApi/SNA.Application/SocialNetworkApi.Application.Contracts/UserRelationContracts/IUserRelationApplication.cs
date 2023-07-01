using _00_Framework.Application;

namespace SocialNetworkApi.Application.Contracts.UserRelationContracts;

public interface IUserRelationApplication
{
    OperationResult Create(CreateUserRelation command);


    /// <summary>
    /// Current user  accept the relationship  request From other user
    /// using the <code>UserRelation</code> <paramref name="id"/>
    /// </summary>
    /// <param name="id">id of request or <code>UserRelation</code> Entity</param>
    /// <returns></returns>
    OperationResult Accept(long id);


    /// <summary>
    /// decline the relationship request from user A
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    OperationResult Decline(long id);
    /// <summary>
    /// Get All user except current user 
    /// </summary>
    /// <param name="currentUserId"></param>
    /// <returns></returns>
    Task<List<UserWithRequestStatusVieModel>> GetAllUserWithRequestStatus(long currentUserId);

    /// <summary>
    /// Accept the relation by current user that request sent to it
    /// Just the user that request sent to it can accept
    /// </summary>
    /// <param name="command"> that have User id of requested person and a user request sent to</param>
    /// <returns></returns>
    Task<OperationResult> Accept(AcceptUserRelation command);

    Task<List<UserWithRequestStatusVieModel>> GetFriendsOfUser(long userId);


    Task<int> GetNumberOfMutualFriend(NumberOfMutualFriend request);
}