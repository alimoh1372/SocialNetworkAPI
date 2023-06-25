using _00_Framework.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApi.Application.Contracts.MessageContracts;

namespace SocialNetworkApi.Presentation.WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessageApplication _messageApplication;

    public MessageController(IMessageApplication messageApplication)
    {
        _messageApplication = messageApplication;
    }

    #region Post Methods

    /// <summary>
    /// Send A message From UserA To UserB
    /// </summary>
    /// <param name="command">Required value to send message</param>
    /// <returns> if model <paramref name="command"/> values is valid so return  <see cref="OperationResult"/> with succeed status </returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///      "fkFromUserId": 1,
    ///      "fkToUserId": 2,
    ///       "messageContent": "Hello Reza.How are yor?"
    ///     }
    /// </remarks>
    /// <response code="200">return succeed message and Request Id</response>
    /// <response code="400">return error message for request model</response>
    /// <response code="500">return internal server error </response>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult SendSend(SendMessage command)
    {
        var result = new OperationResult();
        if (!ModelState.IsValid)
        {
            var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToList();

            return BadRequest(ErrorMessages.ToString());
        }

        result = _messageApplication.Send(command);

        if (!result.IsSuccedded)
            return StatusCode(500, result);

        return Ok(result);
    }



    /// <summary>
    /// To edit message by user A(Sender) 
    /// First finding the message then edit message content
    /// </summary>
    /// <param name="command">Required value to send message</param>
    /// <returns> if model <paramref name="command"/> values is valid so return  <see cref="OperationResult"/> with succeed status </returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///      "Id": 1,
    ///      "MessageContent": "Hello Reza.How are yor?"
    ///     }
    /// </remarks>
    /// <response code="200">return succeed message and Request Id</response>
    /// <response code="400">return error message for request model</response>
    /// <response code="500">return internal server error </response>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Edit(EditMessage command)
    {
        var result = new OperationResult();
        if (!ModelState.IsValid)
        {
            var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToList();

            return BadRequest(ErrorMessages.ToString());
        }

        result = _messageApplication.Edit(command);

        if (!result.IsSuccedded)
            return StatusCode(500, result);

        return Ok(result);
    }

    #endregion


    #region Get Methods

    /// <summary>
    /// Get all messages between two user=> userA=<paramref name="idUserA"/> and userB=<paramref name="idUserB"/>
    /// </summary>
    /// <param name="idUserA">Id of user A in chat message</param>
    /// <param name="idUserB">Id of user B in chat message</param>
    /// <returns>List of <see cref="MessageViewModel"/> between <paramref name="idUserA"/> and <paramref name="idUserB"/></returns>
    /// <remarks>
    /// Sample Request:
    ///
    ///     ?idUserA=1&amp;idUserB=2
    ///
    /// Sample Response:
    /// 
    ///     [
    ///     {
    ///         "id": 4,
    ///         "creationDate": "2023-06-25T08:28:42.584567+00:00",
    ///         "fkFromUserId": 1,
    ///         "senderFullName": "ali mohammadzadeh",
    ///         "fkToUserId": 3,
    ///         "receiverFullName": "sara nemati",
    ///         "messageContent": "Hellooooooo",
    ///         "fromUserProfilePicture": "/UploadFiles/Users/aliProfile.png",
    ///         "toUserProfilePicture": "/Images/DefaultProfile.png"
    ///     },
    ///     {
    ///         "id": 5,
    ///         "creationDate": "2023-06-25T08:30:29.4078184+00:00",
    ///         "fkFromUserId": 3,
    ///         "senderFullName": "sara nemati",
    ///         "fkToUserId": 1,
    ///         "receiverFullName": "ali mohammadzadeh",
    ///         "messageContent": "hello how are you",
    ///         "fromUserProfilePicture": "/Images/DefaultProfile.png",
    ///         "toUserProfilePicture": "/UploadFiles/Users/aliProfile.png"
    ///     }
    ///     ]
    /// </remarks>
    [HttpGet]
    public async Task<List<MessageViewModel>> LoadChatHistory([FromQuery] long idUserA, long idUserB)
    {
        return await _messageApplication.LoadChatHistory(idUserA, idUserB);
    }




    /// <summary>
    /// Get The edit model=<see cref="EditMessage"/> 
    /// </summary>
    /// <param name="id">id of message you want to edit</param>
    /// <returns><see langword="null"/> if there isn't any message with <paramref name="id"/></returns>
    /// <remarks>
    /// Sample Request:
    ///
    ///     ?id=1
    ///
    /// Sample Response:
    /// 
    ///     {
    ///         "id": 1,
    ///         "fkFromUserId": 1,
    ///         "fkToUserId": 2,
    ///         "messageContent": "Hello Reza.How are yor?"
    ///     }
    /// </remarks>
    [HttpGet]
    public async Task<EditMessage?> GetEditMessageBy([FromQuery] long id)
    {
        return await _messageApplication.GetEditMessageBy(id);
    }


    /// <summary>
    /// Get Message viewmodel=<see cref="MessageViewModel"/> 
    /// </summary>
    /// <param name="id">id of message you want to get</param>
    /// <returns><see langword="null"/> if there isn't any message with <paramref name="id"/></returns>
    /// <remarks>
    /// Sample Request:
    ///
    ///     ?id=1
    ///
    /// Sample Response:
    /// 
    ///     {
    ///         "id": 4,
    ///         "creationDate": "2023-06-25T08:28:42.584567+00:00",
    ///         "fkFromUserId": 1,
    ///         "senderFullName": "ali mohammadzadeh",
    ///         "fkToUserId": 3,
    ///         "receiverFullName": "sara nemati",
    ///         "messageContent": "Hellooooooo",
    ///         "fromUserProfilePicture": "/UploadFiles/Users/aliProfile.png",
    ///         "toUserProfilePicture": "/Images/DefaultProfile.png"
    ///     }
    /// </remarks>
    [HttpGet]
    public async Task<MessageViewModel?> GetMessageViewModelBy(long id)
    {
        return await _messageApplication.GetMessageViewModelBy(id);
    }
    #endregion
}
