using Hangfire.MemoryStorage.Database;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Supabase.Interfaces;
using Supabase.Postgrest.Responses;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using WebAPIRestaurantManagement.ModelRequests;
using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Models;
using WebAPIRestaurantManagement.Services.Configuration;
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
        private readonly Supabase.Client _supabaseClient;
        private readonly WebAPIRestaurantManagement.Services.Configuration.IConfigurationService  _configurationService;
        public AuthService(HttpClient httpClient, ISupabaseClientService supabaseClientService, WebAPIRestaurantManagement.Services.Configuration.IConfigurationService configurationService, Supabase.Client supabaseClient)
        {
            _httpClient = httpClient;
            _supabaseClientService = supabaseClientService;
            _configurationService = configurationService;
            _supabaseClient = supabaseClient;
        }
        // Đăng nhập vào Supabase bằng email và password
        private async Task<SupabaseResponse> AuthenticateWithSupabase(string email, string password)
        {
            try
            {
                Supabase.Gotrue.Session responseAuth = await _supabaseClient.Auth.SignIn(email, password);
                if (responseAuth != null)
                {
                    SupabaseResponse userInfo = new SupabaseResponse
                    {
                        AccessToken = responseAuth.AccessToken,
                        ExpiresIn = responseAuth.ExpiresIn,
                        RefreshToken = responseAuth.RefreshToken,
                        User = new SupabaseUserResponse
                        {
                            Id = responseAuth.User.Id,
                            Email = responseAuth.User.Email,
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
            /// // Bước 1: Tìm user trong bảng Users bằng username
            CheckUserResponse user = await _supabaseClientService.GetUserByUsernameAsync(loginRequest.UserName);
            if (user == null)
            {
                result.Succeeded = false;
                result.Errors = [];
                result.Message = "Username not found.";
                result.Data = null;
                return result;
            }
            // Bước 2: Đăng nhập với Supabase bằng email và password
            SupabaseResponse account = await AuthenticateWithSupabase(user.EmailUser, loginRequest.Password);
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
        public Task<Response<SupabaseResponse>> Register(UsersModel usersModel)
        {
            throw new NotImplementedException();
        }

        public Task<Response<SupabaseResponse>> ReloadByRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
