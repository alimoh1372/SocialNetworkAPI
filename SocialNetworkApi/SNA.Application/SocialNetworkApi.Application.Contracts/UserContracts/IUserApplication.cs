using _00_Framework.Application;

namespace SocialNetworkApi.Application.Contracts.UserContracts;

/// <summary>
/// A Abstraction to implement user logic using other layers
/// </summary>
public interface IUserApplication
{

    OperationResult Create(CreateUser command);
    OperationResult Edit(EditUser command);

    OperationResult ChangePassword(ChangePassword command);
    EditUser GetDetails(long id);

    /// <summary>
    /// Get The information of user to edit profile picture
    /// </summary>
    /// <param name="id">user id</param>
    /// <returns></returns>
    Task<EditProfilePicture> GetEditProfileDetails(long id);

    /// <summary>
    /// Filter users with the <paramref name="searchModel"/>
    /// </summary>
    /// <param name="searchModel"></param>
    /// <returns></returns>
    Task<List<UserViewModel>> SearchAsync(SearchModel searchModel);
    Task<OperationResult> ChangeProfilePicture(EditProfilePicture command);

    OperationResult Login(Login command);
    void Logout();
    /// <summary>
    /// Get the user info by id of user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserViewModel> GetUserInfoAsyncBy(long id);
}
