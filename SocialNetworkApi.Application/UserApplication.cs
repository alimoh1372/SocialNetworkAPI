using _00_Framework.Application;
using SocialNetworkApi.Application.Contracts.UserContracts;

namespace SocialNetworkApi.Application;

public class UserApplication : IUserApplication
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthHelper _authHelper;
    private readonly IFileUpload _fileUpload;
    public UserApplication(IUserRepository userRepository, IPasswordHasher passwordHasher, IAuthHelper authHelper, IFileUpload fileUpload)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _authHelper = authHelper;
        _fileUpload = fileUpload;
    }

    public OperationResult Create(CreateUser command)
    {
        var operation = new OperationResult();

        if (_userRepository.IsExists(x => x.Email == command.Email))
            return operation.Failed(ApplicationMessage.Duplication);
        //encrypt the password of user to save on database
        var password = _passwordHasher.Hash(command.Password);
        var account = new User(command.Name, command.LastName, command.Email, command.BirthDay, password,
            command.AboutMe, "/Images/DefaultProfile.png");

        //add to database
        _userRepository.Create(account);
        _userRepository.SaveChanges();
        return operation.Succedded();
    }

    public OperationResult Edit(EditUser command)
    {

        throw new NotImplementedException();
    }

    public OperationResult ChangePassword(ChangePassword command)
    {
        var operation = new OperationResult();
        var user = _userRepository.Get(command.Id);
        if (user == null)
            return operation.Failed(ApplicationMessage.NotFound);

        if (command.Password != command.ConfirmPassword)
            return operation.Failed(ApplicationMessage.PasswordsNotMatch);

        var password = _passwordHasher.Hash(command.Password);
        user.ChangePassword(password);
        _userRepository.SaveChanges();
        return operation.Succedded();
    }

    public EditUser GetDetails(long id)
    {
        return _userRepository.GetDetails(id);
    }

    public async Task<EditProfilePicture> GetEditProfileDetails(long id)
    {
        return await _userRepository.GetAsyncEditProfilePicture(id);
    }

    public Task<List<UserViewModel>> SearchAsync(SearchModel searchModel)
    {
        return _userRepository.SearchAsync(searchModel);
    }

    public async Task<OperationResult> ChangeProfilePicture(EditProfilePicture command)
    {
        var operationResult = new OperationResult();
        var user = _userRepository.Get(command.Id);
        //Check user is exist
        if (user == null)
            return await Task.FromResult(operationResult.Failed(ApplicationMessage.NotFound));
        var previousPictureAddress = user.ProfilePicture;
        var basePath = $"/UploadFiles/Users";
        var newPicturePath = _fileUpload.UploadFile(command.ProfilePicture, basePath);
        if (string.IsNullOrWhiteSpace(newPicturePath))
            return await Task.FromResult(operationResult.Failed(ApplicationMessage.OperationFailed));

        if (previousPictureAddress != "/Images/DefaultProfile.png")
            _fileUpload.DeleteFile(previousPictureAddress);
        user.EditProfilePicture(newPicturePath);
        _userRepository.SaveChanges();
        return operationResult.Succedded();
    }

    public OperationResult Login(Login command)
    {

        var operation = new OperationResult();
        var user = _userRepository.GetBy(command.UserName);
        if (user == null)
            return operation.Failed(ApplicationMessage.WrongUserPass);
        //Compare the inserted password and the saved password
        var result = _passwordHasher.Check(user.Password, command.Password);
        //if password wrong the operation failed
        if (!result.Verified)
            return operation.Failed(ApplicationMessage.WrongUserPass);


        //Adding the identity items to the cookie of client
        var authViewModel = new AuthViewModel(user.Id, user.Email);

        _authHelper.Signin(authViewModel);

        return operation.Succedded();
    }

    public void Logout()
    {
        _authHelper.SignOut();
    }
    /// <summary>
    /// Get the information of user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<UserViewModel> GetUserInfoAsyncBy(long id)
    {
        return _userRepository.GetUserInfoAsyncBy(id);
    }
}
