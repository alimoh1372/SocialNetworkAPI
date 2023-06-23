using _00_Framework.Application;
using SocialNetworkApi.Application.Contracts.UserRelationContracts;
using SocialNetworkApi.Domain.UserRelationAgg;

namespace SocialNetworkApi.Application;

public class UserRelationApplication : IUserRelationApplication
{
    private readonly IUserRelationRepository _userRelationRepository;

    public UserRelationApplication(IUserRelationRepository userRelationRepository)
    {
        _userRelationRepository = userRelationRepository;
    }

    public OperationResult Create(CreateUserRelation command)
    {
        OperationResult result = new OperationResult();
        //Check the equality of user a and user b  (the users cant request themselves)
        if (command.FkUserAId == command.FkUserBId)
            return result.Failed(ApplicationMessage.CantSelfRequest);
        //check Duplication of request
        if (_userRelationRepository.IsExists(x =>
                x.FkUserAId == command.FkUserAId && x.FkUserBId == command.FkUserBId))
            return result.Failed(ApplicationMessage.Duplication);

        //create user relation instance
        UserRelation friendShipRequest = new UserRelation(command.FkUserAId, command.FkUserBId, command.RelationRequestMessage);

        //Add to database
        _userRelationRepository.Create(friendShipRequest);

        _userRelationRepository.SaveChanges();

        return result.Succedded();


    }

    public OperationResult Accept(long id)
    {
        OperationResult result = new OperationResult();

        //Find the relation
        UserRelation friendShipRequest = _userRelationRepository.Get(id);
        if (friendShipRequest == null)
            return result.Failed(ApplicationMessage.NotFound);
        friendShipRequest.AcceptRelation();

        _userRelationRepository.SaveChanges();

        return result.Succedded();
    }

    public OperationResult Decline(long id)
    {
        OperationResult result = new OperationResult();

        //Find the relation
        UserRelation friendShipRequest = _userRelationRepository.Get(id);
        if (friendShipRequest == null)
            return result.Failed(ApplicationMessage.NotFound);

        //Decline the relationShip
        friendShipRequest.DeclineRelation();

        _userRelationRepository.SaveChanges();

        return result.Succedded();
    }

    public async Task<List<UserWithRequestStatusVieModel>> GetAllUserWithRequestStatus(long currentUserId)
    {
        return await _userRelationRepository.GetAllUserWithRequestStatus(currentUserId);
    }

    public async Task<OperationResult> Accept(long userIdRequestSentFromIt, long userIdRequestSentToIt)
    {
        var result = new OperationResult();
        var relation = await _userRelationRepository.GetRelationBy(userIdRequestSentFromIt, userIdRequestSentToIt);
        if (relation == null)
            return result.Failed(ApplicationMessage.NotFound);
        relation.AcceptRelation();
        _userRelationRepository.SaveChanges();
        return result.Succedded();
    }

    public async Task<List<UserWithRequestStatusVieModel>> GetFriendsOfUser(long userId)
    {
        return await _userRelationRepository.GetFriendsOfUser(userId);
    }

    public async Task<int> GetNumberOfMutualFriend(long currentUserId, long friendUserId)
    {
        return await _userRelationRepository.GetMutualFriendNumber(currentUserId, friendUserId);
    }
}
