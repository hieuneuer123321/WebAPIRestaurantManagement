using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPIRestaurantManagement.ModelRequests;
using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Models;
using WebAPIRestaurantManagement.Swagger;

namespace WebAPIRestaurantManagement.Services.Authorization
{
    public interface IAuthService
    {
        Task<Response<SupabaseResponse>> LoginJWTAsync(LoginSupabaseRequest loginRequest);
        Task<Response<SupabaseUserResponse>> Register(UserRegisterResquest userLogin);
        Task<Response<SupabaseResponse>> ReloadByRefreshToken(string refreshToken);
        Task<ModelResponse> TestAddUser();
    }
}
