﻿using WebAPIRestaurantManagement.ModelRequests;
using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Swagger;

namespace WebAPIRestaurantManagement.Services.Roles
{
    public interface IRoleServices
    {
        Task<ModelDataResponse<List<RolesResponse>>> GetRolesAsync(string? search);
        Task<ModelDataResponse<List<RightResponse>>> GetRightByRoleIdAsync(int roleId);
        Task<ModelResponse> UpdateRolesAsync(RolesResponse role);
        Task<ModelResponse> AddRolesAsync(RolesResponse role);
        Task<ModelResponse> DeleteRolesAsync(int roleID);
        Task<TableResponse> GetRolesByIDAsync(int roleID);
    }
}
