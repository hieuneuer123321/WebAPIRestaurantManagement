using Supabase;
using Supabase.Gotrue;
using WebAPIRestaurantManagement.ModelResponses;

namespace WebAPIRestaurantManagement.Services.SupabaseClient
{
    public interface ISupabaseClientService
    {
        Supabase.Client GetSupabaseClient();
        Task<SP_GetUserByUNameResponse> GetUserByUsernameAsync(string username);
        Task<List<SP_GetRightByRoleIdResponse>> GetRightByRoleIdAsync(string roleId);
        Task<List<SP_GetRightByUidResponse>> GetRightByUIdAsync(Guid userIdResquest);
        
      
    }
}
