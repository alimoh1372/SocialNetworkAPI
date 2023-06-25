using _00_Framework.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApi.Application.Contracts.UserRelationContracts;

namespace SocialNetworkApi.Presentation.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserRelationController : ControllerBase
    {
        private readonly IUserRelationApplication _userRelationApplication;

        public UserRelationController(IUserRelationApplication userRelationApplication)
        {
            _userRelationApplication = userRelationApplication;
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
        /// <response code="200">return succeed message and Request Id</response>
        /// <response code="400">return error message for request model</response>
        /// <response code="500">return internal server error </response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateRelation(CreateUserRelation command)
        {
            var result = new OperationResult();
            if (!ModelState.IsValid)
            {
                var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                return BadRequest(ErrorMessages.ToString());
            }

            result = _userRelationApplication.Create(command);

            if (!result.IsSuccedded)
                return StatusCode(500, result);

            return Ok(result);
        }



        /// <summary>
        /// Current user  accept the relationship  request From other user
        /// using the <code>UserRelationController</code> <paramref name="id"/>
        /// </summary>
        /// <param name="id">id of request or <code>UserRelationController</code> Entity</param>
        /// <returns> if model <paramref name="id"/> values is valid so return  <see cref="OperationResult"/> with succeed status </returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///      "id": 1
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
        public IActionResult AcceptRelationBy(long id)
        {
            var result = new OperationResult();
            if (!ModelState.IsValid)
            {
                var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                return BadRequest(ErrorMessages.ToString());
            }

            result = _userRelationApplication.Accept(id);

            if (!result.IsSuccedded)
                return StatusCode(500, result);

            return Ok(result);
        }

        /// <summary>
        /// Accept the relation by current user=<paramref name="userIdRequestSentToIt"/> that request sent to it
        /// Just the user that request sent to it can accept
        /// </summary>
        /// <param name="userIdRequestSentFromIt">User id that requested relationship</param>
        /// <param name="userIdRequestSentToIt">User id that request sent to it</param>
        /// <returns> if model <paramref name="userIdRequestSentFromIt"/> and <see cref="userIdRequestSentToIt"/> values is valid so return  <see cref="OperationResult"/> with succeed status </returns>
        /// <response code="200">return succeed message and Request Id</response>
        /// <response code="400">return error message for request model</response>
        /// <response code="500">return internal server error </response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AcceptRelation(long userIdRequestSentFromIt, long userIdRequestSentToIt)
        {
            var result = new OperationResult();
            if (!ModelState.IsValid)
            {
                var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                return BadRequest(ErrorMessages.ToString());
            }

            result = await _userRelationApplication.Accept(userIdRequestSentFromIt, userIdRequestSentToIt);

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
        [HttpGet]
        public async Task<List<UserWithRequestStatusVieModel>> GetAllUserWithRequestStatus([FromQuery] long currentUserId)
        {
            return await _userRelationApplication.GetAllUserWithRequestStatus(currentUserId);
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
        [HttpGet]
        public async Task<List<UserWithRequestStatusVieModel>> GetFriendsOfUser([FromQuery] long userId)
        {
            return await _userRelationApplication.GetAllUserWithRequestStatus(userId);
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
        [HttpGet]
        public async Task<int> GetNumberOfMutualFriend([FromQuery] long currentUserId, long friendUserId)
        {
            return await _userRelationApplication.GetNumberOfMutualFriend(currentUserId, friendUserId);
        }
        #endregion
    }
}
