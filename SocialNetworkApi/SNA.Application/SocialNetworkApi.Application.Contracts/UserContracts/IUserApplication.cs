using _00_Framework.Application;

namespace SocialNetworkApi.Application.Contracts.UserContracts;

/// <summary>
/// A Abstraction to implement user logic using other layers
/// </summary>
public interface IUserApplication
{

    OperationResult Create(CreateUser command);
    OperationResult Edit(EditUser command);

    Task<OperationResult> ChangePassword(ChangePassword command);
    EditUser? GetDetails(long id);

    /// <summary>
    /// Get The information of user to edit profile picture
    /// </summary>
    /// <param name="id">user id</param>
    /// <returns></returns>
    Task<EditProfilePicture?> GetEditProfilePictureDetails(long id);

    /// <summary>
    /// Filter users with the <paramref name="userSearchModel"/>
    /// </summary>
    /// <param name="userSearchModel"></param>
    /// <returns></returns>
    Task<List<UserViewModel>> SearchAsync(UserSearchModel userSearchModel);
    Task<OperationResult> ChangeProfilePicture(EditProfilePicture command);
    /// <summary>
    /// Get the login information=<paramref name="command"/> 
    /// </summary>
    /// <param name="command">There is UserNam=Email and password to login</param>
    /// <returns>
    /// if UserName and password be correct so return an encrypted token JWE
    /// <remarks>
    /// if they aren't correct so return empty string
    /// </remarks>
    /// </returns>
    Task<string> Login(Login command);
    void Logout();

    /// <summary>
    /// Get the user info by id of user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserViewModel?> GetUserInfoAsyncBy(long id);
}
