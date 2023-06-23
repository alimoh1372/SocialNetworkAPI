using _00_Framework.Domain;
using SocialNetworkApi.Domain.MessageAgg;
using SocialNetworkApi.Domain.UserRelationAgg;

namespace SocialNetworkApi.Domain.UserAgg;

/// <summary>
/// User Entity to handling the user operations
/// </summary>
public class User : EntityBase
{
    #region Properties

    public string Name { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public DateTime BirthDay { get; private set; }
    public string Password { get; private set; }
    public string AboutMe { get; private set; }
    public string ProfilePicture { get; private set; }


    //To create self referencing many-to-many UserRelations
    public ICollection<UserRelation> UserARelations { get; private set; }
    public ICollection<UserRelation> UserBRelations { get; private set; }


    //To  Create self referencing many-to-many message
    public ICollection<Message> FromMessages { get; private set; }
    public ICollection<Message> ToMessages { get; private set; }


    #endregion


    #region Methods
    /// <summary>
    /// Define A User
    /// </summary>
    /// <param name="name"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="birthDay"></param>
    /// <param name="password"></param>
    /// <param name="aboutMe">A description About User</param>
    /// <param name="profilePicture"></param>
    public User(string name, string lastName, string email, DateTime birthDay, string password, string aboutMe, string profilePicture)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        BirthDay = birthDay;
        Password = password;
        AboutMe = aboutMe;
        ProfilePicture = profilePicture;
    }
    /// <summary>
    /// Edit User properties that editable
    /// </summary>
    /// <param name="name"></param>
    /// <param name="lastName"></param>
    /// <param name="aboutMe"></param>

    public void Edit(string name, string lastName, string aboutMe)
    {
        Name = name;
        LastName = lastName;
        AboutMe = aboutMe;
    }
    /// <summary>
    /// Edit the profile picture 
    /// </summary>
    /// <param name="profilePicture">string address from root</param>
    public void EditProfilePicture(string profilePicture)
    {
        if (string.IsNullOrWhiteSpace(profilePicture))
            return;
        ProfilePicture = profilePicture;
    }
    /// <summary>
    /// To change the user password
    /// </summary>
    /// <param name="password"></param>
    public void ChangePassword(string password)
    {
        Password = password;
    }



    #endregion




}
