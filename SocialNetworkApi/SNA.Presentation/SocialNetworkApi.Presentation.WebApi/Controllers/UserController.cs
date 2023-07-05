using System.Globalization;
using System.Net.Mime;
using System.Text;
using _00_Framework.Application;
using _00_Framework.Application.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApi.Application.Contracts.UserContracts;

namespace SocialNetworkApi.Presentation.WebApi.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserApplication _userApplication;
        private readonly IAuthHelper _authHelper;
        public UserController(IUserApplication userApplication, IAuthHelper authHelper)
        {
            _userApplication = userApplication;
            _authHelper = authHelper;
        }

        #region Post Apis
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
        ///     }
        /// </remarks>
        /// <response code="200">return succeed message and user Id</response>
        /// <response code="400">return error message for request model</response>
        /// <response code="500">return internal server error </response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
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
                return StatusCode(500, result.Message);

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
        /// <response code="401">return Unauthorized response when you didn't have access permission to this section</response>
        /// <response code="403">return Deny to access content source because didn't have permission</response>
        /// <response code="500">return internal server error </response>
        [HttpPost]
        [ProducesResponseType(statusCode:StatusCodes.Status200OK,Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized,Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden,Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError,Type = typeof(string))]
        public async Task<IActionResult> ChangePassword(ChangePassword command)
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
            if (authMode.Id != command.Id)
                return StatusCode(StatusCodes.Status403Forbidden, ValidatingMessage.ForbiddenToAccess);
            result =await _userApplication.ChangePassword(command);

            if (!result.IsSuccedded)
                return StatusCode(500, result.Message);

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
        /// <response code="401">return Unauthorized response when you didn't have access permission to this section</response>
        /// <response code="403">return Deny to access content source because didn't have permission</response>
        /// <response code="500">return internal server error </response>
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> ChangeProfilePicture([FromForm] EditProfilePicture command)
        {
            var result = new OperationResult();

            //Validation
            if (!ModelState.IsValid)
            {
                var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                return BadRequest(ErrorMessages.ToString());
            }
            var authMode =await _authHelper.GetUserInfo();
            if (authMode == null)
                return Unauthorized("Please first login");
            if (authMode.Id != command.Id)
                return StatusCode(StatusCodes.Status403Forbidden, ValidatingMessage.ForbiddenToAccess);
            result = await _userApplication.ChangeProfilePicture(command);

            if (!result.IsSuccedded)
                return StatusCode(500, result);

            return Ok(result);
        }


        /// <summary>
        /// Get the login information=<paramref name="command"/> 
        /// </summary>
        /// <param name="command">There is UserNam=Email and password to login</param>
        /// <returns>
        /// if UserName and password be correct so return an encrypted token JWE
        /// <remarks>
        /// if they aren't correct so return empty string
        /// </remarks>
        /// </returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///      "UserName": aliMohammadzade@gmail.com,
        ///      "Password": "IFormFile.jpg"
        ///     }
        /// </remarks>
        /// <response code="200">return a JWT token or <code>""</code> empty string if the user or password not match</response>
        /// <response code="400">return error message for request model</response>
        [HttpPost,AllowAnonymous]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        public async Task<IActionResult> Login(Login command)
        {
            //Validation
            if (!ModelState.IsValid)
            {
                var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                return BadRequest(ErrorMessages.ToString());
            }
           
            return Ok(await _userApplication.Login(command));
        }

        #endregion

        #region Get Apis

        /// <summary>
        /// Get The information of user to edit profile picture
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>an <see cref="EditProfilePicture"/> or <see langword="null"/></returns>
        /// <remarks>
        /// Sample Request:
        ///
        ///     ?id=1
        ///
        /// Sample Response:
        /// 
        ///     {
        ///      "Id": 1,
        ///      "ProfilePicture": null,  
        ///      "PreviousProfilePicture": "/DefaultPicture.jpg"
        ///     }
        /// </remarks>
        /// <response code="200">return succeed message</response>
        /// <response code="400">return error message for request model</response>
        /// <response code="401">return Unauthorized response when you didn't have access permission to this section</response>
        /// <response code="403">return Deny to access content source because didn't have permission</response>
        /// <response code="500">return internal server error </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(EditProfilePicture))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetEditProfilePictureDetails([FromQuery]IdModelArgument<long> idModel)
        {
            if (!ModelState.IsValid)
            {
                var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                return BadRequest(ErrorMessages);
            }
            var authMode =await _authHelper.GetUserInfo();
            if (authMode == null)
                return Unauthorized("Please first login");
            if (authMode.Id != idModel.Id)
                return StatusCode(StatusCodes.Status403Forbidden, ValidatingMessage.ForbiddenToAccess);
            return  Ok(await _userApplication.GetEditProfilePictureDetails(idModel.Id));
        }



        /// <summary>
        /// Get list of "UserViewModel" that contain <paramref name="userSearchModel"/> items
        /// </summary>
        /// <param name="userSearchModel">search in emails </param>
        /// <returns>an <see cref="EditProfilePicture"/> or <see langword="null"/></returns>
        /// <remarks>
        /// Sample Request:
        ///
        ///     ?Email=ali
        ///
        /// Sample Response:
        /// 
        ///     [
        ///     {
        ///         "id": 1,
        ///         "email": "ali@example.com",
        ///         "profilePicture": "/UploadFiles/Users/aliProfile.png",
        ///         "name": null,
        ///         "lastName": null,
        ///         "aboutMe": null
        ///     },
        ///     {
        ///         "id": 2,
        ///         "email": "reza@example.com",
        ///         "profilePicture": "/Images/DefaultProfile.png",
        ///         "name": null,
        ///         "lastName": null,
        ///         "aboutMe": null
        ///     }
        ///     ]
        /// </remarks>
        /// <response code="401">return Unauthorized response when you didn't have access permission to this section</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        public async Task<List<UserViewModel>> SearchAsync([FromQuery]UserSearchModel userSearchModel)
        {
            return await _userApplication.SearchAsync(userSearchModel);
        }

        #endregion
    }
}
