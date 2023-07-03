using SocialNetworkApi.Domain.UserRelationAgg;

namespace _01_Test.SocialNetworkApi.DataMock;

public static class FakeUserRelationData
{
    public static List<UserRelation> UserRelations { get; set; } = new List<UserRelation>()
    {
        new UserRelation(1, 2, "Hello friend"),
        new UserRelation(2, 3, "hello john")
    };


}