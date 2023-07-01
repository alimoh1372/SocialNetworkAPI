using _00_Framework.Application;
using _00_Framework.Application.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApi.Application.Contracts.UserRelationContracts;

namespace SocialNetworkApi.Presentation.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserRelationController : ControllerBase
    {
        private readonly IUserRelationApplication _userRelationApplication;
        private readonly IAuthHelper _authHelper;
        public UserRelationController(IUserRelationApplication userRelationApplication, IAuthHelper authHelper)
        {
            _userRelationApplication = userRelationApplication;
            _authHelper = authHelper;
        }

        /// <summary>
        /// Create relation request from User A to User B 
        /// </summary>
        /// <param name="command">Some required information  to Create User Relation Request</param>
        /// <returns> if model <paramref name="command"/> values is valid so return  <see cref="OperationResult"/> with succeed status </returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///      "fkUserAId": 1,
        ///      "fkUserBId": 1,
        ///       "relationRequestMessage": "string"
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
        public async  Task<IActionResult> CreateRelation(CreateUserRelation command)
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
            if (authMode.Id != command.FkUserAId)
                return StatusCode(StatusCodes.Status403Forbidden, ValidatingMessage.ForbiddenToAccess);
            result = _userRelationApplication.Create(command);

            if (!result.IsSuccedded)
                return StatusCode(500, result);

            return Ok(result.Message);
        }

        //This is a bad method because doesn't have any control on it,and can accept any relation from any one

        ///// <summary>
        ///// Current user  accept the relationship  request From other user
        ///// using the <code>UserRelation</code> <paramref name="idModel"/>
        ///// </summary>
        ///// <param name="idModel">id of request or <code>UserRelationController</code> Entity</param>
        ///// <returns> if model <paramref name="idModel"/> values is valid so return  <see cref="OperationResult"/> with succeed status </returns>
        ///// <remarks>
        ///// Sample request:
        ///// 
        /////     {
        /////      "id": 1
        /////     }
        ///// </remarks>
        ///// <response code="200">return succeed message</response>
        ///// <response code="400">return error message for request model</response>
        ///// <response code="401">return Unauthorized response when you didn't have access permission to this section</response>
        ///// <response code="500">return internal server error </response>
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult AcceptRelationBy(IdModelArgument<long> idModel)
        //{
        //    var result = new OperationResult();
        //    if (!ModelState.IsValid)
        //    {
        //        var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
        //            .Select(x => x.ErrorMessage).ToList();

        //        return BadRequest(ErrorMessages.ToString());
        //    }

        //    result = _userRelationApplication.Accept(idModel.Id);

        //    if (!result.IsSuccedded)
        //        return StatusCode(500, result);

        //    return Ok(result);
        //}

        /// <summary>
        /// Accept the relation by current user=<paramref name="userIdRequestSentToIt"/> that request sent to it
        /// Just the user that request sent to it can accept
        /// </summary>
        /// <param name="userIdRequestSentFromIt">User id that requested relationship</param>
        /// <param name="userIdRequestSentToIt">User id that request sent to it</param>
        /// <returns> if model <paramref name="userIdRequestSentFromIt"/> and <see cref="userIdRequestSentToIt"/> values is valid so return  <see cref="OperationResult"/> with succeed status </returns>
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
        public async Task<IActionResult> AcceptRelation([FromQuery]AcceptUserRelation command)
        {
            var result = new OperationResult();
            if (!ModelState.IsValid)
            {
                var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                return BadRequest(ErrorMessages.ToString());
            }
            var authMode =await  _authHelper.GetUserInfo();
            if (authMode == null)
                return Unauthorized("Please first login");
            if (authMode.Id !=command.userIdRequestSentToIt)
                return StatusCode(StatusCodes.Status403Forbidden, ValidatingMessage.ForbiddenToAccess);
            result = await _userRelationApplication.Accept(command);
            if (!result.IsSuccedded)
                return StatusCode(500, result);

            return Ok(result);
        }

        #region Get APIs
        /// <summary>
        /// Get all users except user with id = <paramref name="currentUserId"/>
        /// </summary>
        /// <param name="currentUserId">Id of user that we want except from response list</param>
        /// <returns>List of <see cref="UserWithRequestStatusVieModel"/> except <see cref="currentUserId"/> or <see langword="null"/></returns>
        /// <remarks>
        /// Sample Request:
        ///
        ///     ?currentUserId=1
        ///
        /// Sample Response:
        /// 
        ///      [
        ///      {
        ///          "userId": 2,
        ///          "name": "reza",
        ///          "lastName": "mohammadzadeh",
        ///          "requestStatusNumber": 2,
        ///          "timeOffset": "0001-01-01T00:00:00+00:00",
        ///          "profilePicture": "/Images/DefaultProfile.png",
        ///          "relationRequestMessage": null,
        ///          "mutualFriendNumber": 0
        ///      },
        ///      {
        ///          "userId": 3,
        ///          "name": "mohammad",
        ///          "lastName": "ahamad",
        ///          "requestStatusNumber": 3,
        ///          "timeOffset": "0001-01-01T00:00:00+00:00",
        ///          "profilePicture": "/Images/DefaultProfile.png",
        ///          "relationRequestMessage": null,
        ///          "mutualFriendNumber": 1
        ///      }
        ///      ]
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
        public async Task<IActionResult> GetAllUserWithRequestStatus([FromQuery]IdModelArgument<long> currentUserId)
        {
            if (!ModelState.IsValid)
            {
                var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                return BadRequest(ErrorMessages.ToString());
            }
            var authModel =await _authHelper.GetUserInfo();
            if (authModel == null)
                return Unauthorized("Please first login");
            if (authModel.Id != currentUserId.Id)
                return StatusCode(StatusCodes.Status403Forbidden, ValidatingMessage.ForbiddenToAccess);
            return Ok(await _userRelationApplication.GetAllUserWithRequestStatus(currentUserId.Id));

        }



        /// <summary>
        /// Get number of mutual friend of 
        /// </summary>
        /// <param name="userId">Id of user that we want get friend's list</param>
        /// <returns>List of <see cref="UserWithRequestStatusVieModel"/> that friend with <see cref="userId"/> or <see langword="null"/></returns>
        /// <remarks>
        /// Sample Request:
        ///
        ///     ?userId=1
        ///
        /// Sample Response:
        /// 
        ///      [
        ///      {
        ///          "userId": 2,
        ///          "name": "reza",
        ///          "lastName": "mohammadzadeh",
        ///          "requestStatusNumber": 2,
        ///          "timeOffset": "0001-01-01T00:00:00+00:00",
        ///          "profilePicture": "/Images/DefaultProfile.png",
        ///          "relationRequestMessage": null,
        ///          "mutualFriendNumber": 0
        ///      },
        ///      {
        ///          "userId": 3,
        ///          "name": "mohammad",
        ///          "lastName": "ahamad",
        ///          "requestStatusNumber": 3,
        ///          "timeOffset": "0001-01-01T00:00:00+00:00",
        ///          "profilePicture": "/Images/DefaultProfile.png",
        ///          "relationRequestMessage": null,
        ///          "mutualFriendNumber": 1
        ///      }
        ///      ]
        /// </remarks>
        /// <response code="200">return succeed message</response>
        /// <response code="400">return error message for request model</response>
        /// <response code="401">return Unauthorized response when you didn't have access permission to this section</response>
        /// <response code="500">return internal server error </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFriendsOfUser([FromQuery]IdModelArgument<long> userId)
        {
            if (!ModelState.IsValid)
            {
                var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                return BadRequest(ErrorMessages);
            }
            return Ok( await _userRelationApplication.GetAllUserWithRequestStatus(userId.Id));
        }



        /// <summary>
        /// Get number of mutual friends of user = <paramref name="currentUserId"/> with it's friend <paramref name="friendUserId"/>
        /// </summary>
        /// <param name="currentUserId">Id of user Send request</param>
        /// /// <param name="friendUserId">Id of user want to get mutual friend with it</param>
        /// <returns>number of mutual friend an <see langword="int"/></returns>
        /// <remarks>
        /// Sample Request:
        ///
        ///     ?currentUserId=&amp;friendUserId=3
        ///
        /// Sample Response:
        /// 
        ///     1
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNumberOfMutualFriend([FromQuery]NumberOfMutualFriend request)
        {
            if (!ModelState.IsValid)
            {
                var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                return BadRequest(ErrorMessages);
            }
            var authModel = await _authHelper.GetUserInfo();
            if (authModel == null)
                return Unauthorized("Please first login");
            if (authModel.Id !=request.CurrentUserId)
                return StatusCode(StatusCodes.Status403Forbidden, ValidatingMessage.ForbiddenToAccess);
            return Ok(await _userRelationApplication.GetNumberOfMutualFriend(request));
        }
        #endregion
    }
}
