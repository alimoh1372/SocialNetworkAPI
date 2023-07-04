using _00_Framework.Application;
using _00_Framework.Application.Models;
using _01_Test.SocialNetworkApi.DataMock;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SocialNetworkApi.Application.Contracts.UserRelationContracts;
using SocialNetworkApi.Presentation.WebApi.Controllers;

namespace _01_Test.SocialNetworkApi.Presentation;

public class UserRelationControllerTest
{
    private readonly Mock<IUserRelationApplication> _userRelationApplicationMock;
    private readonly Mock<IAuthHelper> _authHelperMock;

    public UserRelationControllerTest()
    {
        _userRelationApplicationMock= new Mock<IUserRelationApplication>();
        _authHelperMock=new Mock<IAuthHelper>();
        _authHelperMock.Setup(x => x.GetUserInfo()).ReturnsAsync(new AuthViewModel(1, "ali@example.com"));
    }

    #region CreateRelationTests

    [Fact]

    public async Task CreateRelation_ValidCreateRelationCommand_Return200StatusCode()
    {
        //Arrange
        var createRelationCommand = new CreateUserRelation
        {
            FkUserAId = 1,
            FkUserBId = 2,
            RelationRequestMessage = "I want to be friend with you"
        };
        _userRelationApplicationMock.Setup(x => x.Create(It.IsAny<CreateUserRelation>())).Returns(
            new OperationResult().Succedded()
        );

        var sut = new UserRelationController(_userRelationApplicationMock.Object, _authHelperMock.Object);

        //Act
        var result = await sut.CreateRelation(createRelationCommand);


        //Assert

        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().BeOfType<string>();

    }
    [Fact]
    public async Task CreateRelation_InValidCreateRelationCommand_Return400StatusCode()
    {
        //Arrange
        var createRelationCommand = new CreateUserRelation
        {
            FkUserAId = 1,
            FkUserBId = 2,
            RelationRequestMessage = "I want to be friend with you"
        };
        _userRelationApplicationMock.Setup(x => x.Create(It.IsAny<CreateUserRelation>())).Returns(
            new OperationResult().Succedded()
        );

        var sut = new UserRelationController(_userRelationApplicationMock.Object, _authHelperMock.Object);
        sut.ModelState.AddModelError("TestModelError", "This is a test Model Error");
        //Act
        var result = await sut.CreateRelation(createRelationCommand);


        //Assert

        result.Should().BeOfType<BadRequestObjectResult>();
        (result as BadRequestObjectResult)?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.As<BadRequestObjectResult>().Value.Should().NotBeNull();

    }

    #endregion

    #region AcceptRelationTests

    [Fact]

    public async Task AcceptRelation_ValidUserRelationCommand_Return200StatusCode()
    {
        //Arrange
        var acceptRelationCommand = new AcceptUserRelation
        {
            userIdRequestSentFromIt = 2,
            userIdRequestSentToIt = 1
        };
        _userRelationApplicationMock.Setup(x => x.Accept(It.IsAny<AcceptUserRelation>())).ReturnsAsync(
            new OperationResult().Succedded()
        );

        var sut = new UserRelationController(_userRelationApplicationMock.Object, _authHelperMock.Object);

        //Act
        var result = await sut.AcceptRelation(acceptRelationCommand);


        //Assert

        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().BeOfType<OperationResult>();

    }


    [Fact]
    public async Task AcceptRelation_InValidAcceptRelationCommand_Return400StatusCode()
    {
        //Arrange
        var acceptRelationCommand = new AcceptUserRelation
        {
            userIdRequestSentFromIt = 2,
            userIdRequestSentToIt = 1
        };
        _userRelationApplicationMock.Setup(x => x.Accept(It.IsAny<AcceptUserRelation>())).ReturnsAsync(
            new OperationResult().Succedded()
        );

        var sut = new UserRelationController(_userRelationApplicationMock.Object, _authHelperMock.Object);

        sut.ModelState.AddModelError("TestModelError", "This is a test Model Error");
        //Act
        var result = await sut.AcceptRelation(acceptRelationCommand);


        //Assert

        result.Should().BeOfType<BadRequestObjectResult>();
        (result as BadRequestObjectResult)?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.As<BadRequestObjectResult>().Value.Should().NotBeNull();

    }

    #endregion


    #region GetMethodsTests

    [Fact]
    public async Task GetAllUserWithRequestStatus_Return200StatusCodeWithUserWithRequestStatusModel()
    {
        //Arrange
        IdModelArgument<long> idModel = new IdModelArgument<long>()
        {
            Id = 1
        };
        var random = new Random();
        var usersWithRequestStatus = FakeUserData.GetUsers().ToList().Select((x, index) => new UserWithRequestStatusVieModel
        {
            UserId = index,
            Name = x.Name,
            LastName = x.LastName,
            RequestStatusNumber = (RequestStatus)random.Next(0, 6),
            TimeOffset = default,
            ProfilePicture = x.ProfilePicture,
            RelationRequestMessage = "Test message" + index,
            MutualFriendNumber = random.Next(0, 50)
        }).ToList();
        _userRelationApplicationMock.Setup(x => x.GetAllUserWithRequestStatus(It.IsAny<long>())).ReturnsAsync(
            usersWithRequestStatus
        );

        var sut = new UserRelationController(_userRelationApplicationMock.Object, _authHelperMock.Object);

        //Act
        var result = await sut.GetAllUserWithRequestStatus(idModel);


        //Assert

        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().NotBeNull();
        var list = result.As<OkObjectResult>().Value.As<List<UserWithRequestStatusVieModel>>();
        list.Should().AllBeOfType<UserWithRequestStatusVieModel>();
        list.Should().HaveCount(2);


    }


    [Fact]
    public async Task GetFriendsOfUser_Return200StatusCodeWithUserWithRequestStatusModel()
    {
        //Arrange
        IdModelArgument<long> idModel = new IdModelArgument<long>()
        {
            Id = 1
        };
        var random = new Random();
        var usersWithRequestStatus = FakeUserData.GetUsers().ToList().Select((x, index) => new UserWithRequestStatusVieModel
        {
            UserId = index,
            Name = x.Name,
            LastName = x.LastName,
            RequestStatusNumber = (RequestStatus)random.Next(0, 6),
            TimeOffset = default,
            ProfilePicture = x.ProfilePicture,
            RelationRequestMessage = "Test message" + index,
            MutualFriendNumber = random.Next(0, 50)
        }).ToList();
        _userRelationApplicationMock.Setup(x => x.GetFriendsOfUser(It.IsAny<long>())).ReturnsAsync(
            usersWithRequestStatus
        );

        var sut = new UserRelationController(_userRelationApplicationMock.Object, _authHelperMock.Object);

        //Act
        var result = await sut.GetFriendsOfUser(idModel);


        //Assert

        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().NotBeNull();
        var list = result.As<OkObjectResult>().Value.As<List<UserWithRequestStatusVieModel>>();
        list.Should().AllBeOfType<UserWithRequestStatusVieModel>();
        list.Should().HaveCount(2);


    }

    [Fact]
    public async Task GetNumberOfMutualFriend_Return200StatusCodeWithIntNumberbetween0ToInfinit()
    {
        //Arrange
        NumberOfMutualFriend request = new NumberOfMutualFriend
        {
            CurrentUserId = 1,
            FriendUserId = 2
        };


        _userRelationApplicationMock.Setup(x => x.GetNumberOfMutualFriend(It.IsAny<NumberOfMutualFriend>())).ReturnsAsync(
            new Random().Next(int.MaxValue)
        );

        var sut = new UserRelationController(_userRelationApplicationMock.Object, _authHelperMock.Object);

        //Act
        var result = await sut.GetNumberOfMutualFriend(request);


        //Assert

        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().NotBeNull();
        var intNumber = result.As<OkObjectResult>().Value.As<int>();
        intNumber.Should().BeGreaterOrEqualTo(0);

    }

    #endregion

}