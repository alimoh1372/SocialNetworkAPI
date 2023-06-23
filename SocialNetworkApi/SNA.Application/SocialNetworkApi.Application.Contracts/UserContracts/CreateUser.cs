using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApi.Application.Contracts.UserContracts;

    /// <summary>
    /// A Data transfer object to map client inputs into User Entity
    /// </summary>
    public class CreateUser
    {
        [Required]
        [StringLength(255)]
        public string Name { get;  set; }

        [Required]
        [StringLength(255)]
        [DisplayName("Last name")]
        public string LastName { get;  set; }
        [Required]
        [EmailAddress]
        [DisplayName("User Name/Email")]
        [StringLength(500)]
        public string Email { get;  set; }

        
        public DateTime BirthDay { get;  set; }


        [Required]
        [DataType(DataType.Password)]
        [StringLength(30)]
        [MinLength(4)]
        public string Password { get;  set; }

        [Required]
        [Compare("Password")]
        [StringLength(30)]
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(2000)]
        [DisplayName("About Me")]
        public string AboutMe { get;  set; }

        [Required]
        [StringLength(2000)]
        [DisplayName("Profile picture")]
        public string ProfilePicture { get;  set; }
    }
;