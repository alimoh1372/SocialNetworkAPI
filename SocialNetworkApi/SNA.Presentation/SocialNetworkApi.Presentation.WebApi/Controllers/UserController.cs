using System.Text;
using _00_Framework.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApi.Application.Contracts.UserContracts;

namespace SocialNetworkApi.Presentation.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserApplication _userApplication;

    public UserController(IUserApplication userApplication)
    {
        _userApplication = userApplication;
    }
    /// <summary>
    /// Sign up  user to be able to use the app with <paramref name="command"/> values
    /// </summary>
    /// <param name="command">Some required information  to sign you up</param>
    /// <returns> if model <paramref name="command"/> values is valid so return  <see cref="OperationResult"/> with succeed status </returns>
    /// <remarks>
    /// Sample request:
    ///     {
    ///       "name": "ali",
    ///        "lastName": "mohammadzadeh",
    ///        "email": "ali@example.com",
    ///        "birthDay": "1993-06-24T10:30:50.496Z",
    ///        "password": "123456",
    ///        "confirmPassword": "123456",
    ///        "aboutMe": "I'm a developer",
    ///        "profilePicture": "Default picture"
    ///      }
    /// </remarks>
    /// <response code="200">return succeed message and user Id</response>
    /// <response code="400">return fail error messages</response>


    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SignUp(CreateUser command)
    {
        var result = new OperationResult();
        if (ModelState.IsValid) return Ok(_userApplication.Create(command));

        var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToList();

        return BadRequest(ErrorMessages.ToString());
    }

}