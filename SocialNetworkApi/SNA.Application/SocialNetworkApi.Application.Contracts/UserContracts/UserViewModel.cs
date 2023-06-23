namespace SocialNetworkApi.Application.Contracts.UserContracts;

    /// <summary>
    /// A Dto to map the user entity to client side 
    /// </summary>
    public class UserViewModel
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public string Name { set; get; }
        public string LastName { set; get; }
        public string AboutMe { get; set; }
        
    }
