using Hangfire.MemoryStorage.Database;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Supabase.Interfaces;
using Supabase.Postgrest.Responses;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using WebAPIRestaurantManagement.ModelRequests;
using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Models;
using WebAPIRestaurantManagement.Services.Configuration;
using WebAPIRestaurantManagement.Services.Roles;
using WebAPIRestaurantManagement.Services.SupabaseClient;
using WebAPIRestaurantManagement.Settings;
using WebAPIRestaurantManagement.Swagger;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace WebAPIRestaurantManagement.Services.Authorization
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ISupabaseClientService _supabaseClientService;
        private readonly IRoleServices _roleServices;
        private readonly Supabase.Client _supabaseClient;
        private readonly WebAPIRestaurantManagement.Services.Configuration.IConfigurationService  _configurationService;
        public AuthService(HttpClient httpClient, ISupabaseClientService supabaseClientService, WebAPIRestaurantManagement.Services.Configuration.IConfigurationService configurationService, Supabase.Client supabaseClient, IRoleServices roleServices)
        {
            _httpClient = httpClient;
            _supabaseClientService = supabaseClientService;
            _configurationService = configurationService;
            _supabaseClient = supabaseClient;
            _roleServices = roleServices;
        }
        // Đăng nhập vào Supabase bằng email và password
        private async Task<SupabaseResponse> AuthenticateWithSupabase(string email, string password, int rolesResponse, string uid)
        {
            try
            {
                Supabase.Gotrue.Session responseAuth = await _supabaseClient.Auth.SignIn(email, password);
                if (responseAuth != null)
                {
                    RolesResponse roleUser = await _roleServices.GetRolesByIDAsync(rolesResponse);
                    List<SP_GetRightByUidResponse> rights_SP= await _supabaseClientService.GetRightByUIdAsync(Guid.Parse(responseAuth.User.Id));
                   
                    SupabaseResponse userInfo = new SupabaseResponse
                    {
                        AccessToken = responseAuth.AccessToken,
                        ExpiresIn = responseAuth.ExpiresIn,
                        RefreshToken = responseAuth.RefreshToken,
                        User = new SupabaseUserResponse
                        {
                            Id = Guid.Parse(responseAuth.User.Id),
                            Email = responseAuth.User.Email,
                            Role = roleUser,
                            Userrights = rights_SP
                        },
                    };
                    return userInfo;
                }else return null;
            }
            catch (Exception ex) {
                return null;
            }
        }
        public async Task<Response<SupabaseResponse>> LoginJWTAsync(LoginSupabaseRequest loginRequest)
        {
            Response<SupabaseResponse> result = new Swagger.Response<SupabaseResponse>();
            List<RightResponse> rightsUser = new List<RightResponse> { };
            /// // Bước 1: Tìm user trong bảng Users bằng username
            SP_GetUserByUNameResponse user = await _supabaseClientService.GetUserByUsernameAsync(loginRequest.UserName);
            if (user == null)
            {
                result.Succeeded = false;
                result.Errors = [];
                result.Message = "Username not found.";
                result.Data = null;
                return result;
            }
             
            // Bước 2: Đăng nhập với Supabase bằng email và password
            SupabaseResponse account = await AuthenticateWithSupabase(user.EmailUser, loginRequest.Password, user.rolesUser, user.Uid);
            if (account == null)
            {
                result.Succeeded = false;
                result.Errors = [];
                result.Message = "Invalid password.";
                result.Data = null;
                return result;
            }
            result.Succeeded = true;
            result.Errors = [];
            result.Message = "Login Success!";
            result.Data = account;
            return result;
        }
        public async Task<Response<SupabaseUserResponse>> Register(UserRegisterResquest userLogin)
        {
            try
            {
                Response<SupabaseUserResponse> result = new Swagger.Response<SupabaseUserResponse>();
                // Đăng ký tài khoản người dùng
                Supabase.Gotrue.Session response = await _supabaseClient.Auth.SignUp(userLogin.Email, userLogin.Password);
                if (response == null)
                {
                    result.Succeeded = false;
                    result.Errors = [];
                    result.Message = "System API Errors";
                    result.Data = null;                  
                }
                else
                {
                    if (response.User != null)
                    {
                        // Kiểm tra xem người dùng có đăng ký thành công không
                        if (response.User.EmailConfirmedAt == null)
                        {
                            // Thực hiện hành động bạn muốn khi người dùng mới đăng ký
                            UsersModel userModel = new UsersModel()
                            {
                                Email = userLogin.Email,
                                Date_create = userLogin.Create_day,
                                FullName = userLogin.FullName,
                                Phone = userLogin.Phone,
                                Role_id = userLogin.RoleId,
                                Status = userLogin.Status,
                                User_Number = userLogin.Usernumber,
                                Username = userLogin.UserName,
                                User_id = Guid.Parse(response.User.Id),
                                //"70db78df-18d0-47ee-bf90-574424e59d76"
                            };
                            ModeledResponse<UsersModel> addResponse = await _supabaseClient.From<UsersModel>().Insert(userModel);
                            UsersModel userRegisterReponse = addResponse?.Models.FirstOrDefault();
                            if (userRegisterReponse != null)
                            {
                                RolesResponse roleUser = await _roleServices.GetRolesByIDAsync(userLogin.RoleId);
                                List<SP_GetRightByUidResponse> rights_SP = await _supabaseClientService.GetRightByUIdAsync(userRegisterReponse.User_id);
                                result.Succeeded = true;
                                result.Errors = [];
                                result.Message = "Register Success!";
                                result.Data = new SupabaseUserResponse
                                {
                                    Id = userRegisterReponse.User_id,
                                    Email = userRegisterReponse.Email,
                                    Role = roleUser,
                                    Userrights = rights_SP
                                };
                            }
                        }
                    }
                         
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<Response<SupabaseResponse>> ReloadByRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
        public async Task<ModelResponse> TestAddUser()
        {

            ModelResponse response = new ModelResponse();
            UsersModel userModel = new UsersModel()
            {
                Email = "hieuneuer0022000@gmail.com",
                FullName = "hieuneuer1111",
                Phone = "8573625496",
                Role_id = 1,
                Status = true,
                User_Number = "097645",
                Username = "hieuneuer1111",
                User_id = Guid.Parse("70db78df-18d0-47ee-bf90-574424e59d76"),
                //"70db78df-18d0-47ee-bf90-574424e59d76"
            };
            ModeledResponse<UsersModel> addResponse = await _supabaseClient.From<UsersModel>().Insert(userModel);
            if (addResponse == null || addResponse.Models.Count <= 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Add Errors");

            }
            else
            {
                response.IsValid = true;
                response.ValidationMessages.Add("Add Success!");
            }
            return response;
        }
    }
}
