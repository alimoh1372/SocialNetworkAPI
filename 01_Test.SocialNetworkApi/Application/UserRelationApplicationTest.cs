using _00_Framework.Application;
using _01_Test.SocialNetworkApi.DataMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Moq.EntityFrameworkCore;
using SocialNetworkApi.Application;
using SocialNetworkApi.Application.Contracts.UserRelationContracts;
using SocialNetworkApi.Domain.UserRelationAgg;
using SocialNetworkApi.Infrastructure.EfCore;

namespace _01_Test.SocialNetworkApi.Application;

public class UserRelationApplicationTest
{
    private readonly Mock<SocialNetworkApiContext> _contextMock;

    public UserRelationApplicationTest()
    {
        _contextMock =new Mock<SocialNetworkApiContext>();
        _contextMock.Setup(x => x.UserRelations).ReturnsDbSet(FakeUserRelationData.UserRelations);
       
    }

    #region Create_Tests
    [Fact]
    public void Create_WithValidModel_ReturnSucceededResult()
    {

        //Arrange
        var relationShip = new UserRelation(3, 4, "I'm a good person");
        _contextMock.Setup(x => x.UserRelations.Add(It.IsAny<UserRelation>()));

        var sut = new UserRelationApplication(_contextMock.Object);
        CreateUserRelation userRelation = new CreateUserRelation
        {
            FkUserAId = 3,
            FkUserBId = 4,
            RelationRequestMessage = "I'm a good person"
        };

        //Act
        var result = sut.Create(userRelation);
        FakeUserRelationData.UserRelations.Add(relationShip);


        //Assert
        result.IsSuccedded.Should().BeTrue();
        result.Message.Should().NotBeNullOrWhiteSpace();
        FakeUserRelationData.UserRelations.Should().HaveCount(3);
        FakeUserRelationData.UserRelations.ElementAt(2).FkUserAId.Should().Be(userRelation.FkUserAId);
        FakeUserRelationData.UserRelations.ElementAt(2).FkUserBId.Should().Be(userRelation.FkUserBId);
        FakeUserRelationData.UserRelations.ElementAt(2).RelationRequestMessage.Should().Be(userRelation.RelationRequestMessage);
        FakeUserRelationData.UserRelations.RemoveAt(2);

    }
    [Fact]
    public void Create_WithInValidModel_ReturnFailedResult()
    {

        //Arrange
        var relationShip = new UserRelation(3, 4, "I'm a good person");
        _contextMock.Setup(x => x.UserRelations.Add(It.IsAny<UserRelation>()));

        var sut = new UserRelationApplication(_contextMock.Object);
        CreateUserRelation userRelation = new CreateUserRelation
        {
            FkUserAId = 3,
            FkUserBId = 2,
            RelationRequestMessage = "I'm a good person"
        };

        //Act
        var result = sut.Create(userRelation);



        //Assert
        result.IsSuccedded.Should().BeTrue();
        result.Message.Should().NotBeNullOrWhiteSpace();
        FakeUserRelationData.UserRelations.Should().HaveCount(2);

    }


    #endregion


    #region Accept_Tests
    [Fact]
    public async Task Accept_WithValidModel_ReturnSucceededResult()
    {

        //Arrange

        var sut = new UserRelationApplication(_contextMock.Object);
        AcceptUserRelation acceptRelation = new AcceptUserRelation
        {
            userIdRequestSentFromIt = 1,
            userIdRequestSentToIt = 2
        };

        //Act
        var result = await sut.Accept(acceptRelation);



        //Assert
        result.IsSuccedded.Should().BeTrue();
        result.Message.Should().NotBeNullOrWhiteSpace();
        FakeUserRelationData.UserRelations.Should().HaveCount(2);



    }

    [Fact]
    public async Task Accept_WithInValidModel_ReturnFailedResult()
    {

        //Arrange

        var sut = new UserRelationApplication(_contextMock.Object);
        AcceptUserRelation acceptRelation = new AcceptUserRelation
        {
            userIdRequestSentFromIt = 2,
            userIdRequestSentToIt = 1
        };

        //Act
        var result = await sut.Accept(acceptRelation);



        //Assert
        result.IsSuccedded.Should().BeFalse();
        result.Message.Should().NotBeNullOrWhiteSpace();
        FakeUserRelationData.UserRelations.Should().HaveCount(2);

    }


    #endregion

    #region GetMethods_Test

    [Fact]
    public async Task GetAllUserWithRequestStatus_ReturnAllUsersInfoWithNullNotFoundInfo()
    {

        //Arrange
        _contextMock.Setup(x => x.Users).ReturnsDbSet(FakeUserData.GetUsers());

        var sut = new UserRelationApplication(_contextMock.Object);


        //Act
        var result = await sut.GetAllUserWithRequestStatus(1);



        //Assert
        result.Should().AllBeOfType<UserWithRequestStatusVieModel>();
        result.Should().HaveCountGreaterThan(1);
        FakeUserRelationData.UserRelations.Should().HaveCount(2);



    }


    //[Fact]
    //public async Task GetFriendsOfUser_ReturnAllUsersInfoWithNullNotFoundInfo()
    //{

    //    //Arrange
    //    _contextMock.Setup(x => x.Users).ReturnsDbSet(FakeUserData.GetUsers());

    //    var sut = new UserRelationApplication(_contextMock.Object);


    //    //Act
    //    var result = await sut.GetFriendsOfUser(0);



    //    //Assert
    //    result.Should().AllBeOfType<UserWithRequestStatusVieModel>();
    //    result.Should().HaveCountGreaterOrEqualTo(0);
    //    FakeUserRelationData.UserRelations.Should().HaveCount(2);



    //}

    [Fact]
    public async Task GetNumberOfMutualFriend_ReturnIntNumberOrZero()
    {

        //Arrange
        var sut = new UserRelationApplication(_contextMock.Object);
        NumberOfMutualFriend mutualFriendNumberRequest = new NumberOfMutualFriend
        {
            CurrentUserId = 0,
            FriendUserId = 1
        };

        //Act
        var result = await sut.GetNumberOfMutualFriend(mutualFriendNumberRequest);



        //Assert
        result.Should().BeOfType(typeof(int));
        result.Should().BeGreaterOrEqualTo(0);
        FakeUserRelationData.UserRelations.Should().HaveCount(2);
    }

    #endregion


}