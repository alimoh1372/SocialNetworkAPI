
using _00_Framework.Application;
using _00_Framework.Domain;
using _01_Test.SocialNetworkApi.DataMock;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using SocialNetworkApi.Application;
using SocialNetworkApi.Application.Contracts.UserContracts;
using SocialNetworkApi.Domain.UserAgg;
using SocialNetworkApi.Infrastructure.EfCore;


namespace _01_Test.SocialNetworkApi.Application;

public class UserApplicationTest
{
    private readonly Mock<IFileUpload> _mockIFileUpload;
    private readonly Mock<IAuthHelper> _mockIAuthHelper;
    private readonly Mock<IPasswordHasher> _mockIPasswordHasher;
    private readonly Mock<SocialNetworkApiContext> _mockDbContext;


    public UserApplicationTest()
    {
        _mockIFileUpload = new Mock<IFileUpload>();
        _mockIAuthHelper = new Mock<IAuthHelper>();
        _mockIPasswordHasher = new Mock<IPasswordHasher>();
        _mockDbContext = new Mock<SocialNetworkApiContext>();
        _mockDbContext.Setup<DbSet<User>>(x => x.Users)
            .ReturnsDbSet(FakeUserData.GetUsers());
    }


    #region CreateUser_Tests

    [Fact]
    public void CreateUser_withValidCommand_returnSucceeded()
    {
        //Arrange
        OperationResult result = new OperationResult();

        //Setup password hasher to return a string value=>123456
        _mockIPasswordHasher.Setup(m => m.Hash(It.IsAny<string>()))
            .Returns("123456");

        var context = _mockDbContext.Object;

        //Create a System Under Test=SUT=>UserApplication
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);

        //Create new model of Creating a user
        CreateUser createUser = new CreateUser
        {
            Name = "mohammadz",
            LastName = "Ahmadi",
            Email = "mohammad@gmail.com",
            BirthDay = new DateTime(1998, 03, 14),
            Password = "123456",
            ConfirmPassword = "123456",
            AboutMe = "I'm a good person"
        };

        //Act

        //A way to add one user to list
        //var users = context.Users.ToList();
        //User user = new User(createUser.Name, createUser.LastName, createUser.Email, createUser.BirthDay,
        //    createUser.Password, createUser.AboutMe,"");
        //users.Add(user);
        //users.Should().HaveCount(3);
        result = sut.Create(createUser);


        //Assert
        result.IsSuccedded.Should().BeTrue();
        result.Message.Should().NotBeNullOrWhiteSpace();
        context.Users.Should().HaveCount(2);
    }


    [Fact]
    public void CreateUser_WithInValidCommand_ReturnFailedResult()
    {
        //Arrange
        OperationResult result = new OperationResult();
        _mockIPasswordHasher.Setup(m => m.Hash(It.IsAny<string>()))
            .Returns("123456");
        

        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);
        CreateUser createUser = new CreateUser
        {
            Name = "mohammadz",
            LastName = "Ahmadi",
            Email = "ali@gmail.com",
            BirthDay = new DateTime(1998, 03, 14),
            Password = "123456",
            ConfirmPassword = "123456",
            AboutMe = "I'm a good person"
        };

        //A way to test number of user after added

        //var users = FakeUserData.GetUsers().ToList();
        //users.Add(new User(createUser.Name, createUser.LastName
        //    , createUser.Email, createUser.BirthDay, createUser.Password
        //    , createUser.AboutMe, "/Images/Default.jpg"));
        //users.Should().HaveCount(2);
        //Act
        result = sut.Create(createUser);


        //Assert
        result.IsSuccedded.Should().BeFalse();
        result.Message.Should().NotBeNullOrWhiteSpace();
        context.Users.Should().HaveCount(2);
    }


    #endregion

    #region ChangePassword_Tests
    [Fact]
    public async Task ChangePassword_WithValidCommand_ReturnSucceededTrue()
    {
        //Arrange
        OperationResult result = new OperationResult();

        _mockIPasswordHasher.Setup(m => m.Hash(It.IsAny<string>()))
            .Returns("123456");

        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object,
            _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);

        ChangePassword changePassword = new ChangePassword
        {
            Id = 0,
            Password = "123456",
            ConfirmPassword = "123456"
        };

        //Act
        result = await sut.ChangePassword(changePassword);


        //Assert
        result.IsSuccedded.Should().BeTrue();
        result.Message.Should().NotBeNullOrWhiteSpace();
        context.Users.Should().HaveCount(2);

    }
    [Fact]
    public async Task ChangePassword_WithInValidCommand_ReturnSucceededFalse()
    {
        //Arrange
        OperationResult result = new OperationResult();
       
        _mockIPasswordHasher.Setup(m => m.Hash(It.IsAny<string>()))
            .Returns("123456");
        
        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);
        ChangePassword changePassword = new ChangePassword
        {
            Id = 0,
            Password = "123456",
            ConfirmPassword = "1234567"
        };

        //Act
        result = await sut.ChangePassword(changePassword);


        //Assert
        result.IsSuccedded.Should().BeFalse();
        result.Message.Should().NotBeNullOrWhiteSpace();
        context.Users.Should().HaveCount(2);
    }



    #endregion

    #region GetDetail_Tests

    [Fact]
    public void GetDetails_WithValidId_ReturnAnEditModel()
    {
        //Arrange
        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);


        //Act
        var result = sut.GetDetails(0);


        //Assert
        result.Should().NotBeNull();
        result?.Should().BeOfType<EditUser?>();
        result?.Id.Should().Be(0);
        result?.Name.Should().Be("ali");
        result?.LastName.Should().Be("mohammadzade");

    }
    [Fact]
    public void GetDetails_WithInValidId_ReturnNull()
    {
        //Arrange
        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);


        //Act
        var result = sut.GetDetails(1);


        //Assert
        result.Should().BeNull();
        result?.Should().BeOfType<EditUser?>();

    }


    #endregion


    #region GetEditProfilePictureDetails

    [Fact]
    public async Task GetEditProfilePictureDetails_WithValidId_ReturnEditProfilePicture()
    {
        //Arrange
        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);


        //Act
        var result = await sut.GetEditProfilePictureDetails(0);


        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<EditProfilePicture?>();
        result.Id.Should().Be(0);
        result.PreviousProfilePicture.Should().NotBeNullOrWhiteSpace();
        result.ProfilePicture.Should().BeNull();

    }

    [Fact]
    public async Task GetEditProfilePictureDetails_WithInValidId_ReturnNull()
    {
        //Arrange
        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);


        //Act
        var result = await sut.GetEditProfilePictureDetails(1);


        //Assert
        result.Should().BeNull();
        result?.Should().BeOfType<EditProfilePicture?>();
        Assert.ThrowsAny<NullReferenceException>(() => result.Id);

    }


    #endregion

    #region SearchAsync_Tests

    [Fact]
    public async Task SearchAsync_WithEmptyModel_Return2Element()
    {
        //Arrange
        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);

        UserSearchModel searchModel = new UserSearchModel();


        //Act
        var result = await sut.SearchAsync(searchModel);


        //Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(2);
    }


    [Fact]
    public async Task SearchAsync_WithNonEmptyModel_Return2orLessThan2Element()
    {
        //Arrange
        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);
        UserSearchModel searchModel = new UserSearchModel
        {
            Email = "ahmad"
        };


        //Act
        var result = await sut.SearchAsync(searchModel);


        //Assert
        result.Should().HaveCountLessOrEqualTo(2);
        result.Should().AllBeOfType<UserViewModel>();
        
    }

    #endregion


    #region ChangeProfilePicture
    [Fact]
    public async Task ChangeProfilePicture_WithValidInputCommand_ReturnSucceeded()
    {
        //Arrange

        //Setup upload file to return path of file after saving on server
        _mockIFileUpload.Setup(_ => _.UploadFile(It.IsAny<IFormFile>(),
            It.IsAny<string>())).Returns("/pathOfnewPicture");

        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);

        EditProfilePicture editProfile = new EditProfilePicture
        {
            Id = 0,
            ProfilePicture = null,
            PreviousProfilePicture = null
        };


        //Act
        var result = await sut.ChangeProfilePicture(editProfile);


        //Assert
        result.IsSuccedded.Should().BeTrue();
        result.Message.Should().NotBeNullOrWhiteSpace();
    }


    [Fact]
    public async Task ChangeProfilePicture_WithInValidInputCommand_ReturnFailed()
    {
        //Arrange

        _mockIFileUpload.Setup(_ => _.UploadFile(It.IsAny<IFormFile>(), It.IsAny<string>())).Returns("/pathOfnewPicture");
       
        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);

        EditProfilePicture editProfile = new EditProfilePicture
        {
            Id = 1,
            ProfilePicture = null,
            PreviousProfilePicture = null
        };


        //Act
        var result = await sut.ChangeProfilePicture(editProfile);


        //Assert
        result.IsSuccedded.Should().BeFalse();
        result.Message.Should().NotBeNullOrWhiteSpace();

    }


    #endregion

    #region Login

    [Fact]
    public async Task Login_WithCorrectUserPassword_ReturnToken()
    {
        //Arrange

        _mockIAuthHelper.Setup(m => m.CreateToken(It.IsAny<AuthViewModel>()))
            .ReturnsAsync("ThisIsAFakeToken.ToKeepMeInside.Signature");

        _mockIPasswordHasher.Setup(m => 
            m.Check(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((true, true));

       
        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);
        var login = new Login
        {
            UserName = "ali@gmail.com",
            Password = "123456"
        };


        //Act
        var result = await sut.Login(login);


        //Assert
        result.Should().NotBeNullOrWhiteSpace();
        result.Split('.').Should().HaveCount(3);

    }


    [Fact]
    public async Task Login_WithWrongUserPassword_ReturnToken()
    {
        //Arrange
        
        _mockIAuthHelper.Setup(m => m.CreateToken(It.IsAny<AuthViewModel>()))
            .ReturnsAsync("ThisIsAFakeToken.ToKeepMeInside.Signature");

        _mockIPasswordHasher.Setup(m => m.Check(It.IsAny<string>(), It.IsAny<string>())).Returns((false, false));
        
        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);
        var login = new Login
        {
            UserName = "ali@gmail.com",
            Password = "1231"
        };


        //Act
        var result = await sut.Login(login);


        //Assert
        result.Should().BeNullOrWhiteSpace();


    }



    #endregion

    #region GetUserInfoAsyncBy

    [Fact]
    public async Task GetUserInfoAsyncBy_WithValidId_ReturnUserViewModel()
    {
        //Arrange
        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);



        //Act
        var result = await sut.GetUserInfoAsyncBy(0);


        //Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(0);
        result.Name.Should().Be("ali");
        result.LastName.Should().Be("mohammadzade");
        result.Email.Should().Be("ali@gmail.com");
    }



    [Fact]
    public async Task GetUserInfoAsyncBy_WithInValidId_ReturnNull()
    {
        //Arrange

        var context = _mockDbContext.Object;
        var sut = new UserApplication(context, _mockIAuthHelper.Object, _mockIPasswordHasher.Object,
            _mockIFileUpload.Object);



        //Act
        var result = await sut.GetUserInfoAsyncBy(1);


        //Assert
        result.Should().BeNull();
        Assert.ThrowsAny<NullReferenceException>(() => result.Id);

    }


    #endregion




}