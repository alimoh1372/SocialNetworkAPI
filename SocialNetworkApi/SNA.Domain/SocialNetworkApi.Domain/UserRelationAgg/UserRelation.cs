using _00_Framework.Domain;
using SocialNetworkApi.Domain.UserAgg;







namespace SocialNetworkApi.Domain.UserRelationAgg;

/// <summary>
/// The Entity and valueObject to handle the Relation of users between
/// </summary>
public class UserRelation : EntityBase
{
    public long FkUserAId { get; private set; }
    public User UserA { get; private set; }

    public long FkUserBId { get; private set; }
    public User UserB { get; private set; }

    public string RelationRequestMessage { get; private set; }
    public bool Approve { get; private set; }

    #region UserMethods

    /// <summary>
    /// Make the Request of relation from User A To User B
    /// </summary>
    /// <param name="fkUserAId">Id of user A</param>
    /// <param name="fkUserBId">Id of user B</param>
    /// <param name="relationRequestMessage">A message to show user that must be accept request</param>
    public UserRelation(long fkUserAId, long fkUserBId, string relationRequestMessage)
    {
        if (fkUserAId == fkUserBId)
            return;
        FkUserAId = fkUserAId;
        FkUserBId = fkUserBId;
        RelationRequestMessage = relationRequestMessage;
        Approve = false;
    }

    /// <summary>
    /// Accepting The relation by User B
    /// </summary>
    public void AcceptRelation()
    {
        Approve = true;
    }

    /// <summary>
    /// Decline Relation By user B
    /// </summary>
    public void DeclineRelation()
    {
        Approve = false;
    }


    #endregion


}
