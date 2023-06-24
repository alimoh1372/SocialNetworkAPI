using _00_Framework.Application;
using _00_Framework.Domain;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApi.Application.Contracts.UserContracts;
using SocialNetworkApi.Domain.UserAgg;
using SocialNetworkApi.Infrastructure.EfCore;

namespace SocialNetworkApi.Application;

public class UserApplication : IUserApplication
{
    private readonly SocialNetworkApiContext _context;
    private readonly IPasswordHasher _passwordHasher;

    private readonly IFileUpload _fileUpload;
    public UserApplication(SocialNetworkApiContext context, IPasswordHasher passwordHasher, IFileUpload fileUpload)
    {

        _passwordHasher = passwordHasher;
        _fileUpload = fileUpload;
    }

    public OperationResult Create(CreateUser command)
    {
        var operation = new OperationResult();

        if (!_context.Users.Any(x => x.Email == command.Email))
            return operation.Failed(ApplicationMessage.Duplication);

        //encrypt the password of user to save on database
        var password = _passwordHasher.Hash(command.Password);
        var account = new User(command.Name, command.LastName, command.Email, command.BirthDay, password,
            command.AboutMe, "/Images/DefaultProfile.png");

        //add to database
        _context.Add(account);
        _context.SaveChanges();
        return operation.Succedded($"Account created.User id:{account.Id}");
    }

    public OperationResult Edit(EditUser command)
    {

        throw new NotImplementedException();
    }

    public OperationResult ChangePassword(ChangePassword command)
    {
        var operation = new OperationResult();
        var user = _context.Users.FirstOrDefaultAsync(x => x.Id == command.Id);
        if (user.Result == null)
            return operation.Failed(ApplicationMessage.NotFound);

        if (command.Password != command.ConfirmPassword)
            return operation.Failed(ApplicationMessage.PasswordsNotMatch);

        var password = _passwordHasher.Hash(command.Password);
        user.Result.ChangePassword(password);
        _context.SaveChanges();
        return operation.Succedded();
    }

    public EditUser? GetDetails(long id)
    {
        return _context.Users.Select(x => new EditUser
        {
            Id = x.Id,
            Name = x.Name,
            LastName = x.LastName,
            AboutMe = x.AboutMe,
            ProfilePicture = x.ProfilePicture
        }).FirstOrDefault(x => x.Id == id);

    }

    public async Task<EditProfilePicture?> GetEditProfileDetails(long id)
    {
        return await _context.Users.Select(x => new EditProfilePicture
        {
            Id = x.Id,
            PreviousProfilePicture = x.ProfilePicture
        })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<UserViewModel>> SearchAsync(SearchModel searchModel)
    {
        var query = _context.Users.Select(x => new UserViewModel
        {
            Id = x.Id,
            Email = x.Email,
            ProfilePicture = x.ProfilePicture
        });
        if (!string.IsNullOrWhiteSpace(searchModel.Email))
            query = query.Where(x => x.Email == searchModel.Email);

        return query.ToListAsync();
    }

    public async Task<OperationResult> ChangeProfilePicture(EditProfilePicture command)
    {
        var operationResult = new OperationResult();
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.Id);
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
        await _context.SaveChangesAsync();
        return operationResult.Succedded();
    }

    public async Task<string> Login(Login command)
    {

        var operation = new OperationResult();
        var user =await _context.Users.FirstOrDefaultAsync(x => x.Email == command.UserName);
        if (user == null)
            return "";
        //Compare the inserted password and the saved password
        var result = _passwordHasher.Check(user.Password, command.Password);
        //if password wrong the operation failed
        if (!result.Verified)
            return "";


        //Adding the identity items to the cookie of client
        var authViewModel = new AuthViewModel(user.Id, user.Email);

        //ToDo:Creating token and return it

        return "";
    }

    public void Logout()
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Get the information of user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<UserViewModel?> GetUserInfoAsyncBy(long id)
    {
        return _context.Users.Select(x=>new UserViewModel
        {
            Id = x.Id,
            Email = x.Email,
            ProfilePicture = x.ProfilePicture,
            Name = x.Name,
            LastName = x.LastName,
            AboutMe = x.AboutMe
        }).FirstOrDefaultAsync(x=>x.Id==id);
    }
}
