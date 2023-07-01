using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApi.Application.Contracts.UserContracts;

public class UserSearchModel
{
    /// <summary>
    /// search the emails that contain <see cref="Email"/> value
    /// </summary>
    public string? Email { get; set; }
}
