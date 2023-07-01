using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApi.Application.Contracts.MessageContracts;


/// <summary>
/// A model to load chat history by using two user Id
/// id of current user <see cref="IdUserACurrentUser"/> and id other user =<see cref="IdUserB"/>
/// </summary>
public class LoadChat
{
    [Required(AllowEmptyStrings = false)]
    public long IdUserACurrentUser { get; set; }

    [Required(AllowEmptyStrings = false)]
    public long IdUserB { get; set; }
}