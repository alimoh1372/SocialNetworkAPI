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
    /// To edit message by user A(Sender)<br/>
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
}
