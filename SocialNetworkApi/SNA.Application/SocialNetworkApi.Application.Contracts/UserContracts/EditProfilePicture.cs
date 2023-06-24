using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace SocialNetworkApi.Application.Contracts.UserContracts;

/// <summary>
/// To edit profile using this command
/// </summary>
public class EditProfilePicture
{
    [Required]
    public long Id { get; set; }
    [Required]
    public IFormFile ProfilePicture { get; set; }
    [Required(AllowEmptyStrings = true)]
    public string PreviousProfilePicture { get; set; }
}
