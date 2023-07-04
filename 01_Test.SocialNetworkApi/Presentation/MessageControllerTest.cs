using _00_Framework.Application;
using _00_Framework.Application.Models;
using _01_Test.SocialNetworkApi.DataMock;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SocialNetworkApi.Application.Contracts.MessageContracts;
using SocialNetworkApi.Application.Contracts.UserContracts;
using SocialNetworkApi.Application.Contracts.UserRelationContracts;
using SocialNetworkApi.Presentation.WebApi.Controllers;

namespace _01_Test.SocialNetworkApi.Presentation;

public class MessageControllerTest  
{
    private readonly Mock<IMessageApplication> _messageApplicationMock;
    private readonly Mock<IAuthHelper> _authHelperMock;

    public MessageControllerTest()
    {
        _messageApplicationMock= new Mock<IMessageApplication>();
        _authHelperMock=new Mock<IAuthHelper>();
        _authHelperMock.Setup(x => x.GetUserInfo()).ReturnsAsync(new AuthViewModel(1, "ali@example.com"));
    }

    #region SendTests

    [Fact]

    public async Task Send_ValidSendMessageCommand_Return200StatusCode()
    {
        //Arrange
        var createRelationCommand = new SendMessage
        {
            FkFromUserId = 1,
            FkToUserId = 2,
            MessageContent = "Test message"
        };
        _messageApplicationMock.Setup(x => x.Send(It.IsAny<SendMessage>())).Returns(
            new OperationResult().Succedded()
        );

        var sut = new MessageController(_messageApplicationMock.Object, _authHelperMock.Object);

        //Act
        var result = await sut.Send(createRelationCommand);


        //Assert

        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().BeOfType<OperationResult>();

    }
    [Fact]
    public async Task Send_InValidSendMessageCommand_Return400StatusCode()
    {
        //Arrange
        var sendMessageCommand = new SendMessage
        {
            FkFromUserId = 1,
            FkToUserId = 2,
            MessageContent = "Test message"
        };
        _messageApplicationMock.Setup(x => x.Send(It.IsAny<SendMessage>())).Returns(
            new OperationResult().Succedded()
        );
        var sut = new MessageController(_messageApplicationMock.Object, _authHelperMock.Object);

        sut.ModelState.AddModelError("TestModelError", "This is a test Model Error");
        //Act
        var result = await sut.Send(sendMessageCommand);


        //Assert

        result.Should().BeOfType<BadRequestObjectResult>();
        (result as BadRequestObjectResult)?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.As<BadRequestObjectResult>().Value.Should().NotBeNull();

    }

    #endregion

    #region EditTests

    [Fact]

    public async Task Edit_ValidEditMessageCommand_Return200StatusCode()
    {
        //Arrange
        var editMessageCommand = new EditMessage
        {
            Id = new Random().NextInt64(),
            FkFromUserId = 1,
            FkToUserId = 2,
            MessageContent = "Test Message "+new Random().NextInt64(),
        };
        _messageApplicationMock.Setup(x => x.Edit(It.IsAny<EditMessage>())).Returns(
            new OperationResult().Succedded()
        );

        var sut = new MessageController(_messageApplicationMock.Object, _authHelperMock.Object);

        //Act
        var result = await sut.Edit(editMessageCommand);


        //Assert

        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().BeOfType<OperationResult>();

    }


    [Fact]
    public async Task Edit_InValidEditMessageCommand_Return400StatusCode()
    {
        //Arrange
        var editMessageCommand = new EditMessage
        {
            Id = new Random().NextInt64(),
            FkFromUserId = 1,
            FkToUserId = 2,
            MessageContent = "Test Message " + new Random().NextInt64(),
        };
        _messageApplicationMock.Setup(x => x.Edit(It.IsAny<EditMessage>())).Returns(
            new OperationResult().Succedded()
        );

        var sut = new MessageController(_messageApplicationMock.Object, _authHelperMock.Object);


        sut.ModelState.AddModelError("TestModelError", "This is a test Model Error");
        //Act
        var result = await sut.Edit(editMessageCommand);


        //Assert

        result.Should().BeOfType<BadRequestObjectResult>();
        (result as BadRequestObjectResult)?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.As<BadRequestObjectResult>().Value.Should().NotBeNull();

    }

    #endregion

    [Fact]
    public async Task LoadChatHistory_Return200StatusCodeWithListOfMessageViewModel()
    {
        //Arrange
        var loadChatRequest = new LoadChat
        {
            IdUserACurrentUser = 1,
            IdUserB = 3
        };
       var random = new Random();
        var messages = FakeMessageData.Messages.Select((x,index)=>new MessageViewModel
        {
            Id = index+1,
            CreationDate = x.CreationDate,
            FkFromUserId = x.FkFromUserId,
            SenderFullName = "Sender Test Name",
            FkToUserId = x.FkToUserId,
            ReceiverFullName = "Receiver Test Name",
            MessageContent = "Test Message"+(index+1),
            FromUserProfilePicture = "Test User Profile Address",
            ToUserProfilePicture = "Test Receiver Profile Address"
        }).ToList();
        _messageApplicationMock.Setup(x => x.LoadChatHistory(It.IsAny<LoadChat>())).ReturnsAsync(
            messages
        );

        var sut = new MessageController(_messageApplicationMock.Object, _authHelperMock.Object);

        //Act
        var result = await sut.LoadChatHistory(loadChatRequest);


        //Assert

        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().NotBeNull();
        var list = result.As<OkObjectResult>().Value.As<List<MessageViewModel>>();
        list.Should().NotBeNullOrEmpty();
        list.Should().AllBeOfType<MessageViewModel>();
        list.Should().HaveCount(2);


    }


    [Fact]
    public async Task GetEditMessageBy_Return200StatusCodeWithEditMessageModel()
    {
        //Arrange
        IdModelArgument<long> idModel = new IdModelArgument<long>()
        {
            Id = 1
        };
        var random = new Random();
        var message = FakeMessageData.Messages.Select((x, index) => new EditMessage
        {
            Id = index+1,
            FkFromUserId = x.FkFromUserId,
            FkToUserId = x.FkToUserId,
            MessageContent = x.MessageContent
        }).ToList().First();
        _messageApplicationMock.Setup(x => x.GetEditMessageBy(It.IsAny<long>())).ReturnsAsync(
           message
        );

        var sut = new MessageController(_messageApplicationMock.Object, _authHelperMock.Object);

        //Act
        var result = await sut.GetEditMessageBy(idModel);


        //Assert

        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().NotBeNull();
        var list = result.As<OkObjectResult>().Value.As<EditMessage>();
        list.Should().NotBeNull();



    }

    [Fact]
    public async Task GetMessageViewModelBy_Return200StatusCodeWithMessageViewModel()
    {
        //Arrange
        IdModelArgument<long> idModel = new IdModelArgument<long>()
        {
            Id = 1
        };


        MessageViewModel? message = FakeMessageData.Messages.Select((x, index) => new MessageViewModel
        {
            Id = index+1,
            FkFromUserId = x.FkFromUserId,      
            FkToUserId = x.FkToUserId,
            MessageContent = x.MessageContent,
        }).ToList().First();
        _messageApplicationMock.Setup(x => x.GetMessageViewModelBy(It.IsAny<long>())).ReturnsAsync(
            message
        );

        var sut = new MessageController(_messageApplicationMock.Object, _authHelperMock.Object);

        //Act
        var result = await sut.GetMessageViewModelBy(idModel);


        //Assert

        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().NotBeNull();
        var resultMessage = result.As<OkObjectResult>().Value.As<MessageViewModel>();
        resultMessage.Should().NotBeNull();

    }
}