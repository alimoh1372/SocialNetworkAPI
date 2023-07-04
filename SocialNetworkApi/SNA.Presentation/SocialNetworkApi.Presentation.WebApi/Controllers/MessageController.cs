using _00_Framework.Application;
using _00_Framework.Application.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApi.Application.Contracts.MessageContracts;

namespace SocialNetworkApi.Presentation.WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MessageController : ControllerBase
{
    private readonly IMessageApplication _messageApplication;
    private readonly IAuthHelper _authHelper;
    public MessageController(IMessageApplication messageApplication, IAuthHelper authHelper)
    {
        _messageApplication = messageApplication;
        _authHelper = authHelper;
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
    /// <response code="200">return succeed message</response>
    /// <response code="400">return error message for request model</response>
    /// <response code="401">return Unauthorized response when you didn't have access permission to this section</response>
    /// <response code="403">return Deny to access content source because didn't have permission</response>
    /// <response code="500">return internal server error </response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Send(SendMessage command)
    {
        var result = new OperationResult();
        if (!ModelState.IsValid)
        {
            var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToList();

            return BadRequest(ErrorMessages.ToString());
        }
        var authMode =await _authHelper.GetUserInfo();
        if (authMode == null)
            return Unauthorized("Please first login");
        if (authMode.Id != command.FkFromUserId)
            return StatusCode(StatusCodes.Status403Forbidden, ValidatingMessage.ForbiddenToAccess);
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
    /// <response code="200">return succeed message</response>
    /// <response code="400">return error message for request model</response>
    /// <response code="401">return Unauthorized response when you didn't have access permission to this section</response>
    /// <response code="403">return Deny to access content source because didn't have permission</response>
    /// <response code="500">return internal server error </response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit(EditMessage command)
    {
        var result = new OperationResult();
        if (!ModelState.IsValid)
        {
            var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToList();

            return BadRequest(ErrorMessages.ToString());
        }
        var authMode = await _authHelper.GetUserInfo();
        if (authMode == null)
            return Unauthorized("Please first login");
        if (authMode.Id != command.FkFromUserId)
            return StatusCode(StatusCodes.Status403Forbidden, ValidatingMessage.ForbiddenToAccess);
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
    /// <param name="request">A model that have current user id and other user id to load chats between them</param>
    /// <returns>List of <see cref="MessageViewModel"/> between two user in <paramref name="request"/></returns>
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
    ///         "receiverFullName": "reza mohammadzade",
    ///         "messageContent": "Hellooooooo",
    ///         "fromUserProfilePicture": "/UploadFiles/Users/aliProfile.png",
    ///         "toUserProfilePicture": "/Images/DefaultProfile.png"
    ///     },
    ///     {
    ///         "id": 5,
    ///         "creationDate": "2023-06-25T08:30:29.4078184+00:00",
    ///         "fkFromUserId": 3,
    ///         "senderFullName": "reza mohammadzade",
    ///         "fkToUserId": 1,
    ///         "receiverFullName": "ali mohammadzadeh",
    ///         "messageContent": "hello how are you",
    ///         "fromUserProfilePicture": "/Images/DefaultProfile.png",
    ///         "toUserProfilePicture": "/UploadFiles/Users/aliProfile.png"
    ///     }
    ///     ]
    /// </remarks>
    /// <response code="200">return succeed message</response>
    /// <response code="400">return error message for request model</response>
    /// <response code="401">return Unauthorized response when you didn't have access permission to this section</response>
    /// <response code="403">return Deny to access content source because didn't have permission</response>
    /// <response code="500">return internal server error </response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoadChatHistory([FromQuery]LoadChat request)
    {
        if (!ModelState.IsValid)
        {
            var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToList();

            return BadRequest(ErrorMessages.ToString());
        }
        var authMode = await _authHelper.GetUserInfo();
        if (authMode == null)
            return Unauthorized("Please first login");
        if (authMode.Id != request.IdUserACurrentUser)
            return StatusCode(StatusCodes.Status403Forbidden, ValidatingMessage.ForbiddenToAccess);
        return Ok( await _messageApplication.LoadChatHistory(request));
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
    /// <response code="200">return succeed message</response>
    /// <response code="400">return error message for request model</response>
    /// <response code="401">return Unauthorized response when you didn't have access permission to this section</response>
    /// <response code="403">return Deny to access content source because didn't have permission</response>
    /// <response code="500">return internal server error </response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEditMessageBy([FromQuery]IdModelArgument<long> idModel)
    {
        if (!ModelState.IsValid)
        {
            var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToList();

            return BadRequest(ErrorMessages.ToString());
        }
        var authMode = await _authHelper.GetUserInfo();
        if (authMode == null)
            return Unauthorized("Please first login");
        var editMessage= await _messageApplication.GetEditMessageBy(idModel.Id);
        if (authMode.Id != editMessage.FkFromUserId)
            return StatusCode(StatusCodes.Status403Forbidden, ValidatingMessage.ForbiddenToAccess);
        return Ok(editMessage);
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
    /// <response code="200">return succeed message</response>
    /// <response code="400">return error message for request model</response>
    /// <response code="401">return Unauthorized response when you didn't have access permission to this section</response>
    /// <response code="403">return Deny to access content source because didn't have permission</response>
    /// <response code="500">return internal server error </response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMessageViewModelBy([FromQuery]IdModelArgument<long> idModel)
    {
        if (!ModelState.IsValid)
        {
            var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToList();

            return BadRequest(ErrorMessages.ToString());
        }
        var authMode = await _authHelper.GetUserInfo();
        if (authMode == null)
            return Unauthorized("Please first login");
        var message = await _messageApplication.GetMessageViewModelBy(idModel.Id);
        if (authMode.Id != message?.FkFromUserId)
            return StatusCode(StatusCodes.Status403Forbidden, ValidatingMessage.ForbiddenToAccess);
        return Ok(message);
    }
    #endregion
}
