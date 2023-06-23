using Microsoft.AspNetCore.Http;

namespace SocialNetworkApi.Application.Contracts.UserContracts;

/// <summary>
/// To edit profile using this command
/// </summary>
public class EditProfilePicture
{
    public long Id { get; set; }
    public IFormFile ProfilePicture { get; set; }
    public string PreviousProfilePicture { get; set; }
}
