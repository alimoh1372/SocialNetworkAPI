using _00_Framework.Application;
using _00_Framework.Application.Models;
using _01_Test.SocialNetworkApi.DataMock;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    #region ChangePassword_Tests
    [Fact]
    public async Task ChangePassword_ValidChangePasswordCommand_Return200StatusCode()
    {
        OperationResult? operationResult = new OperationResult();

        //Arrange
        _userApplicationMock.Setup(x => x.ChangePassword(It.IsAny<ChangePassword>())).ReturnsAsync(operationResult.Succedded());
        _authHelperMock.Setup(x => x.GetUserInfo()).ReturnsAsync(new AuthViewModel(1, "ali@example.com"));
        var changePassword = new ChangePassword
        {
            Id = 1,
            Password = "123456",
            ConfirmPassword = "123456"
        };

        var sut = new UserController(_userApplicationMock.Object, _authHelperMock.Object);


        //Act
        var result = await sut.ChangePassword(changePassword);


        //Assert
        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().BeOfType<OperationResult>();

    }

    [Fact]
    public async Task ChangePassword_InValidChangePasswordCommand_Return200StatusCode()
    {
        OperationResult? operationResult = new OperationResult();

        //Arrange
        _userApplicationMock.Setup(x => x.ChangePassword(It.IsAny<ChangePassword>())).ReturnsAsync(operationResult.Succedded());
        _authHelperMock.Setup(x => x.GetUserInfo()).ReturnsAsync(new AuthViewModel(1, "ali@example.com"));
        var changePassword = new ChangePassword
        {
            Id = 1,
            Password = "123456",
            ConfirmPassword = "123456"
        };

        var sut = new UserController(_userApplicationMock.Object, _authHelperMock.Object);
        sut.ModelState.AddModelError("test Error", "This is a test model error");

        //Act
        var result = await sut.ChangePassword(changePassword);


        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        (result as BadRequestObjectResult)?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.As<BadRequestObjectResult>().Value.Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.Should().BeOfType<String>();


    }



    #endregion

    #region ChangeProfilePicture_Tests

    [Fact]
    public async Task ChangeProfilePicture_ValidEditProfilePictureCommand_Return200StatusCode()
    {
        var operationResult = new OperationResult().Succedded();
        //Arrange
        _userApplicationMock.Setup(x => x.ChangeProfilePicture(It.IsAny<EditProfilePicture>())).ReturnsAsync(operationResult.Succedded());
        _authHelperMock.Setup(x => x.GetUserInfo()).ReturnsAsync(new AuthViewModel(1, "ali@example.com"));
        var editProfilePicture = new EditProfilePicture
        {
            Id = 1,
            ProfilePicture = null,
            PreviousProfilePicture = "/Images/DefaultProfile.png"

        };

        var sut = new UserController(_userApplicationMock.Object, _authHelperMock.Object);


        //Act
        var result = await sut.ChangeProfilePicture(editProfilePicture);


        //Assert
        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().BeOfType<OperationResult>();

    }
    [Fact]
    public async Task ChangeProfilePicture_InValidEditProfilePictureCommand_Return400StatusCode()
    {
        OperationResult? operationResult = new OperationResult();

        //Arrange
        _userApplicationMock.Setup(x => x.ChangeProfilePicture(It.IsAny<EditProfilePicture>())).ReturnsAsync(operationResult.Succedded());
        _authHelperMock.Setup(x => x.GetUserInfo()).ReturnsAsync(new AuthViewModel(1, "ali@example.com"));
        var editProfilePicture = new EditProfilePicture
        {
            Id = 1,
            ProfilePicture = null,
            PreviousProfilePicture = "/Images/DefaultProfile.png"

        };


        var sut = new UserController(_userApplicationMock.Object, _authHelperMock.Object);
        sut.ModelState.AddModelError("test Error", "This is a test model error");

        //Act
        var result = await sut.ChangeProfilePicture(editProfilePicture);


        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        (result as BadRequestObjectResult)?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.As<BadRequestObjectResult>().Value.Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.Should().BeOfType<String>();


    }

    #endregion


    #region Login_Test
    [Fact]
    public async Task Login_ValidLoginCommand_Return200StatusCode()
    {

        //Arrange
        _userApplicationMock.Setup(x => x.Login(It.IsAny<Login>())).ReturnsAsync("This is a test Token.This is payload.This is signature");
        var login = new Login();

        var sut = new UserController(_userApplicationMock.Object, _authHelperMock.Object);


        //Act
        var result = await sut.Login(login);


        //Assert
        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().BeOfType<string>();
        var token = result.As<OkObjectResult>().Value.As<string>();
        token.Should().NotBeNullOrEmpty();
        token.Split(".").Should().HaveCount(3);


    }

    [Fact]
    public async Task login_WrongUserNameOrPasswordCommand_Return200statusCodeWithEmptyString()
    {

        //Arrange
        _userApplicationMock.Setup(x => x.Login(It.IsAny<Login>())).ReturnsAsync("");
        var login = new Login();

        var sut = new UserController(_userApplicationMock.Object, _authHelperMock.Object);


        //Act
        var result = await sut.Login(login);




        //Assert
        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().BeOfType<string>();
        var token = result.As<OkObjectResult>().Value.As<string>();
        token.Should().BeNullOrWhiteSpace();



    }

    [Fact]
    public async Task Login_InValidLoginCommand_Return400StatusCode()
    {

        //Arrange
        _userApplicationMock.Setup(x => x.Login(It.IsAny<Login>())).ReturnsAsync("This is a test Token.This is payload.This is signature");

        var login = new Login();

        var sut = new UserController(_userApplicationMock.Object, _authHelperMock.Object);
        sut.ModelState.AddModelError("test Error", "This is a test model error");


        //Act
        var result = await sut.Login(login);


        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        (result as BadRequestObjectResult)?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.As<BadRequestObjectResult>().Value.Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.Should().BeOfType<String>();


    }



    #endregion

    #region GetMethods
    [Fact]

    public async Task GetEditProfilePictureDetails_ValidIdModel_Return200StatusCode()
    {
        //Arrange
        _authHelperMock.Setup(x => x.GetUserInfo()).ReturnsAsync(new AuthViewModel(1, "ali@example.com"));
        _userApplicationMock.Setup(x => x.
            GetEditProfilePictureDetails(It.IsAny<long>()))
            .ReturnsAsync(
            new EditProfilePicture
            {
                Id = 1,
                ProfilePicture = null,
                PreviousProfilePicture = "/Images/DefaultProfile.png"
            });

        var sut = new UserController(_userApplicationMock.Object, _authHelperMock.Object);

        //Act
        var result = await sut.GetEditProfilePictureDetails(new IdModelArgument<long>()
        {
            Id = 1
        });

        //Assert

        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult)?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.As<OkObjectResult>().Value.Should().BeOfType<EditProfilePicture?>();
        var editProfilePicture = result.As<OkObjectResult>().Value.As<EditProfilePicture>();
        editProfilePicture.Id.Should().Be(1);

    }

    [Fact]
    public async Task GetEditProfilePictureDetails_ReturnListOfUserViewModel()
    {
        //Arrange
        var users = FakeUserData.GetUsers().Select((x, Index) => new UserViewModel
        {
            Id = Index,
            Email = x.Email,
            ProfilePicture = x.ProfilePicture,
            Name = x.Name,
            LastName = x.LastName,
            AboutMe = x.AboutMe
        }).ToList();

        _userApplicationMock.Setup(x => x.SearchAsync(It.IsAny<UserSearchModel>()))
            .ReturnsAsync(users);
        var sut = new UserController(_userApplicationMock.Object, _authHelperMock.Object);

        //Act
        var result = await sut.SearchAsync(new UserSearchModel
        {
            Email = null
        });

        //Assert

        result.Should().BeOfType<List<UserViewModel>>();
        result.Should().HaveCount(2);
    }



    #endregion

}