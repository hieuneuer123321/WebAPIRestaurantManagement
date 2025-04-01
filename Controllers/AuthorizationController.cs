using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supabase.Gotrue;
using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Models;
using WebAPIRestaurantManagement.Swagger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIRestaurantManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
       
        private readonly WebAPIRestaurantManagement.Services.Authorization.IAuthService _authorizationService;
        public AuthorizationController(WebAPIRestaurantManagement.Services.Authorization.IAuthService authorizationService) {
            _authorizationService = authorizationService;
        }
        // GET: api/<AuthorizationController>


        // POST api/<AuthorizationController>
        [HttpPost("LoginAuthorizationJWT")]
        public async Task<ActionResult> LoginJWTAsync ([FromBody] ModelRequests.LoginSupabaseRequest request)
        {
            Response<SupabaseResponse> result = await _authorizationService.LoginJWTAsync(request);
            return Ok(result);
        }
        // POST api/<AuthorizationController>
        [HttpPost("RegisterAuthorizationJWT")]
        public async Task<ActionResult> RegisterAuthorizationJWT([FromBody] ModelRequests.UserRegisterResquest request)
        {
            Response<SupabaseUserResponse> result = await _authorizationService.Register(request);
            return Ok(result);
        }
        //[HttpPost("TestAddRecord")]
        //public async Task<ActionResult> TestAddRecord()
        //{
        //    ModelResponse result = await _authorizationService.TestAddUser();
        //    return result.IsValid ? Ok(result) : BadRequest(result);
        //}
    }
}
