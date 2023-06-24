using System.Text;
using _00_Framework.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApi.Application.Contracts.UserContracts;

namespace SocialNetworkApi.Presentation.WebApi.Controllers
{

    [Route("api/[controller]/[action]")]
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
        /// 
        ///     {
        ///      "name": "ali",
        ///      "lastName": "mohammadzadeh",
        ///      "email": "ali@example.com",
        ///      "birthDay": "1993-06-24T10:55:56.228Z",
        ///      "password": "123456",
        ///      "confirmPassword": "123456",
        ///      "aboutMe": "I'm a good person",
        ///      "profilePicture": "Default/picture.jpg"
        ///     }
        /// </remarks>
        /// <response code="200">return succeed message and user Id</response>
        /// <response code="400">return error message for request model</response>
        /// <response code="500">return internal server error </response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SignUp(CreateUser command)
        {
            var result = new OperationResult();
            if (!ModelState.IsValid)
            {
                var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                return BadRequest(ErrorMessages.ToString());
            }

            result = _userApplication.Create(command);

            if (!result.IsSuccedded)
                return StatusCode(500, result);

            return Ok(result);
        }


        /// <summary>
        /// Change your account password
        /// </summary>
        /// <param name="command">required field to change password</param>
        /// <returns>fail or succeed message and status code</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///      "id": 1,
        ///      "password": "654321",
        ///      "confirmPassword": "654321"
        ///     }
        /// </remarks>
        /// <response code="200">return succeed message</response>
        /// <response code="400">return error message for request model</response>
        /// <response code="500">return internal server error </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ChangePassword(ChangePassword command)
        {
            var result = new OperationResult();
            if (!ModelState.IsValid)
            {
                var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                return BadRequest(ErrorMessages.ToString());
            }

            result = _userApplication.ChangePassword(command);

            if (!result.IsSuccedded)
                return StatusCode(500, result);

            return Ok(result);
        }


        /// <summary>
        /// Change your Profile picture
        /// </summary>
        /// <param name="command">required field to change password</param>
        /// <returns>fail or succeed message and status code</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///      "id": 1,
        ///      "ProfilePicture": "IFormFile.jpg",
        ///      "PreviousProfilePicture": "nullable"
        ///     }
        /// </remarks>
        /// <response code="200">return succeed message</response>
        /// <response code="400">return error message for request model</response>
        /// <response code="500">return internal server error </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeProfilePicture([FromForm]EditProfilePicture command)
        {
            var result = new OperationResult();
            if (!ModelState.IsValid)
            {
                var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                return BadRequest(ErrorMessages.ToString());
            }

            result =await _userApplication.ChangeProfilePicture(command);

            if (!result.IsSuccedded)
                return StatusCode(500, result);

            return Ok(result);
        }

        //TODO:Implementing login and return token to the client
    }
}
