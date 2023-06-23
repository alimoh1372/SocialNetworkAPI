using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApi.Application.Contracts.UserContracts;

/// <summary>
/// A Data transfer object to map client inputs to edit user properties that are editable
/// </summary>
public class EditUser
{

    public long Id { get; set; }
    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    [Required]
    [StringLength(255)]
    [DisplayName("Last name")]
    public string LastName { get; set; }


    [Required]
    [StringLength(2000)]
    [DisplayName("About Me")]
    public string AboutMe { get; set; }

    [Required]
    [StringLength(2000)]
    [DisplayName("Profile picture")]
    public string ProfilePicture { get; set; }
}
