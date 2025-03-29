using Microsoft.AspNetCore.Identity.Data;
using Supabase.Postgrest.Responses;
using WebAPIRestaurantManagement.Helpers;
using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Models;
using WebAPIRestaurantManagement.Services.SupabaseClient;
using WebAPIRestaurantManagement.Swagger;

namespace WebAPIRestaurantManagement.Services.Roles
{
    public class RoleServices : IRoleServices
    {
        private readonly Supabase.Client _clientSupabase;
        private readonly ISupabaseClientService _supabaseClientService;
        public RoleServices (Supabase.Client client, ISupabaseClientService supabaseClientService)
        {
            _clientSupabase = client;
            _supabaseClientService = supabaseClientService;
        }
        public Task<ModelResponse> AddRolesAsync(RolesResponse role)
        {
            throw new NotImplementedException();
        }

        public Task<ModelResponse> DeleteRolesAsync(int roleID)
        {
            throw new NotImplementedException();
        }
        public async Task<ModelDataResponse<List<RightResponse>>> GetRightByRoleIdAsync(int roleId)
        {
            ModelDataResponse<List<RightResponse>> result = new ModelDataResponse<List<RightResponse>>();
            List<SP_GetRightByRoleIdResponse> rightsSP = await _supabaseClientService.GetRightByRoleIdAsync(roleId);
            List<RightResponse> rights = rightsSP.Select( u => new RightResponse
            {
                RightId = u.Right_id_pro,
                RightName = u.Right_name_pro,
            }).ToList();
            result.ItemResponse = rights;
            return result;

        }
        public async Task<ModelDataResponse<List<RolesResponse>>> GetRolesAsync(string? search)
        {
            ModeledResponse<RolesModel> SupabaseResponse = await _clientSupabase.From<RolesModel>().Get();
            List<RolesModel> ListItems = SupabaseResponse.Models.ToList();
            if (!string.IsNullOrWhiteSpace(search))
            {
                StringConvert conv = new StringConvert();
                search = conv.ConvertToUnSign(search);
                ListItems = ListItems.Where(tb => conv.ConvertToUnSign(tb.RoleName).Contains(search)).ToList();
            }
            List<RolesResponse> convertItemResponse = ListItems.Select(u => new RolesResponse
            {
                RoleName = u.RoleName,
                RoleId = u.RoleId,
                Description = u.Description,
            }).ToList();
            ModelDataResponse<List<RolesResponse>> result = new ModelDataResponse<List<RolesResponse>>();
            result.ItemResponse = convertItemResponse;
            return result;
        }

        public Task<TableResponse> GetRolesByIDAsync(int roleID)
        {
            throw new NotImplementedException();
        }

        public Task<ModelResponse> UpdateRolesAsync(RolesResponse role)
        {
            throw new NotImplementedException();
        }
    }
}
