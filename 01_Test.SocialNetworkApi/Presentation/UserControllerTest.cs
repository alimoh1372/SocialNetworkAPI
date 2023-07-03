using _00_Framework.Application;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Moq;
using SocialNetworkApi.Application.Contracts.UserContracts;
using SocialNetworkApi.Presentation.WebApi.Controllers;

namespace _01_Test.SocialNetworkApi.Presentation;

public class UserControllerTest
{
    private readonly Mock<IUserApplication> _userApplicationMock;
    private readonly Mock<IAuthHelper> _authHelperMock;

    public UserControllerTest()
    {
        _userApplicationMock = new Mock<IUserApplication>();
        _authHelperMock = new Mock<IAuthHelper>();
    }


    #region SignUp
    [Fact]
    public void SignUp_ValidCreateCommand_Return200StatusCode()
    {
        var operationResult = new OperationResult();

        //Arrange
        _userApplicationMock.Setup(x => x.Create(It.IsAny<CreateUser>())).Returns(new OperationResult().Succedded());

        var CreateUserCommand = new CreateUser
        {
            Name = "Ali",
            LastName = "Mohammadzadeh",
            Email = "ali@gmail.com",
            BirthDay = new DateTime(1993, 02, 23),
            Password = "123456",
            ConfirmPassword = "123456",
            AboutMe = "I'm a person"
        };

        var sut = new UserController(_userApplicationMock.Object, _authHelperMock.Object);


        //Act
        var result = sut.SignUp(CreateUserCommand);


        //Assert
        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().BeOfType<OperationResult>();

    }


    [Fact]
    public void SignUp_InValidCreateCommand_Return400StatusCode()
    {


        //Arrange
        var CreateUserCommand = new CreateUser();


        var sut = new UserController(_userApplicationMock.Object, _authHelperMock.Object);
        sut.ModelState.AddModelError("TestModelError", "This is a test Model Error");

        //Act
        var result = sut.SignUp(CreateUserCommand);


        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        (result as BadRequestObjectResult)?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.As<BadRequestObjectResult>().Value.Should().NotBeNull();

    }


    #endregion

    [Fact]
    public async Task ChangePassword_ValidCreateCommand_Return200StatusCode()
    {
        OperationResult? operationResult = new OperationResult();

        //Arrange
        _userApplicationMock.Setup(x => x.ChangePassword(It.IsAny<ChangePassword>())).ReturnsAsync(operationResult.Succedded());

        var changePassword = new ChangePassword();

        var sut = new UserController(_userApplicationMock.Object, _authHelperMock.Object);
      

        //Act
        var result =await sut.ChangePassword(changePassword);


        //Assert
        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().BeOfType<OperationResult>();

    }



}