using Supabase;
using Supabase.Gotrue;
using WebAPIRestaurantManagement.ModelResponses;

namespace WebAPIRestaurantManagement.Services.SupabaseClient
{
    public interface ISupabaseClientService
    {
        Supabase.Client GetSupabaseClient();
        Task<SP_GetUserByUNameResponse> GetUserByUsernameAsync(string username);
        Task<List<SP_GetRightByRoleIdResponse>> GetRightByRoleIdAsync(int roleId);
    }
}
