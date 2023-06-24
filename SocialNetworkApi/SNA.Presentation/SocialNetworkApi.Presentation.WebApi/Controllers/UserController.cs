using System.Text;
using _00_Framework.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApi.Application.Contracts.UserContracts;

namespace SocialNetworkApi.Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserApplication _userApplication;

        public UserController(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignUp(CreateUser command)
        {
            var result=new OperationResult();
            if (ModelState.IsValid)
            {
                return Ok( _userApplication.Create(command));
            }

            var ErrorMessages = ModelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToList();
            
            return BadRequest(ErrorMessages.ToString());
        }
    }
}
