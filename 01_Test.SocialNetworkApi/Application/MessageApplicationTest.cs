using System.Linq.Expressions;
using _01_Test.SocialNetworkApi.DataMock;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;
using SocialNetworkApi.Application;
using SocialNetworkApi.Application.Contracts.MessageContracts;
using SocialNetworkApi.Domain.MessageAgg;
using SocialNetworkApi.Infrastructure.EfCore;

namespace _01_Test.SocialNetworkApi.Application;

public class MessageApplicationTest
{
    private readonly Mock<SocialNetworkApiContext> _contextMock;

    public MessageApplicationTest()
    {
        _contextMock = new Mock<SocialNetworkApiContext>();
        _contextMock.Setup(x => x.Messages).ReturnsDbSet(FakeMessageData.Messages.AsQueryable());
    }

    #region Send_Tests
    [Fact]
    public void Send_WithValidMessageModel_ReturnSucceededResult()
    {
        //Arrange
        _contextMock.Setup(x => x.UserRelations).ReturnsDbSet(FakeUserRelationData.UserRelations.AsQueryable());

        SendMessage sendMessageCommand = new SendMessage
        {
            FkFromUserId = 1,
            FkToUserId = 2,
            MessageContent = "Test 3"
        };

        var sut = new MessageApplication(_contextMock.Object);


        //Act

        var result = sut.Send(sendMessageCommand);

        var message = new Message(sendMessageCommand.FkFromUserId, sendMessageCommand.FkToUserId,
            sendMessageCommand.MessageContent);

        FakeMessageData.Messages.Add(message);



        //Assert

        result.IsSuccedded.Should().BeTrue();
        result.Message.Should().NotBeNullOrWhiteSpace();
        FakeMessageData.Messages.Should().HaveCount(3);

        FakeMessageData.Messages.RemoveAt(2);
    }


    [Fact]
    public void Send_WithInValidMessageModel_ReturnFailedResult()
    {
        _contextMock.Setup(x => x.UserRelations).ReturnsDbSet(FakeUserRelationData.UserRelations.AsQueryable());

        //Arrange
        SendMessage sendMessageCommand = new SendMessage
        {
            FkFromUserId = 1,
            FkToUserId = 1,
            MessageContent = "Test 3"
        };

        var sut = new MessageApplication(_contextMock.Object);


        //Act
        var result = sut.Send(sendMessageCommand);


        //Assert

        result.IsSuccedded.Should().BeFalse();
        result.Message.Should().NotBeNullOrWhiteSpace();
        FakeMessageData.Messages.Should().HaveCount(2);
    }


    #endregion

    #region Edit_Tests


    [Fact]
    public void Edit_WithValidEditModel_ReturnSucceededResult()
    {
        //Arrange

        EditMessage EditmessageCommand = new EditMessage
        {
            Id = 0,
            FkFromUserId = 1,
            FkToUserId = 2,
            MessageContent = "Edited Test"
        };

        var sut = new MessageApplication(_contextMock.Object);


        //Act

        var result = sut.Edit(EditmessageCommand);



        FakeMessageData.Messages[0].Edit(EditmessageCommand.MessageContent);





        //Assert

        result.IsSuccedded.Should().BeTrue();
        result.Message.Should().NotBeNullOrWhiteSpace();
        FakeMessageData.Messages.Should().HaveCount(2);
        FakeMessageData.Messages[0].MessageContent.Should().Be(EditmessageCommand.MessageContent);
        FakeMessageData.Messages[0].Edit("Salam khoobi");
    }


    [Fact]
    public void Edit_WithInValidEditModel_ReturnFailedResult()
    {
        //Arrange

        EditMessage EditmessageCommand = new EditMessage
        {
            Id = 1,
            FkFromUserId = 5,
            FkToUserId = 4,
            MessageContent = "Edited Test"
        };

        var sut = new MessageApplication(_contextMock.Object);


        //Act

        var result = sut.Edit(EditmessageCommand);







        //Assert

        result.IsSuccedded.Should().BeFalse();
        result.Message.Should().NotBeNullOrWhiteSpace();
        FakeMessageData.Messages.Should().HaveCount(2);
        FakeMessageData.Messages[0].MessageContent.Should().NotBe(EditmessageCommand.MessageContent);

    }

    #endregion


    #region GetMethods_Tests


    [Fact]
    public async Task GetEditMessageBy_ReturnReturnFirstElementOfData()
    {
        //Arrange

        var sut = new MessageApplication(_contextMock.Object);


        //Act

        var result = await sut.GetEditMessageBy(0);


        //Assert

        result.Should().BeOfType<EditMessage?>();
        result.Should().NotBeNull();
        result?.MessageContent.Should().Be(FakeMessageData.Messages[0].MessageContent);




    }

    //[Fact]
    //public async Task GetMessageViewModelBy_ReturnReturnFirstElementOfData()
    //{
    //    //Arrange

    //    var sut = new MessageApplication(_contextMock.Object);


    //    //Act

    //    var result = await sut.GetMessageViewModelBy(0);


    //    //Assert

    //    result.Should().BeOfType<MessageViewModel?>();
    //    result.Should().NotBeNull();
    //    result?.MessageContent.Should().Be(FakeMessageData.Messages[0].MessageContent);




    //}

    #endregion

}