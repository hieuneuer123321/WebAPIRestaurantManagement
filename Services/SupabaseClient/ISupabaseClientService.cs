using Supabase;
using Supabase.Gotrue;
using WebAPIRestaurantManagement.ModelResponses;

namespace WebAPIRestaurantManagement.Services.SupabaseClient
{
    public interface ISupabaseClientService
    {
        Supabase.Client GetSupabaseClient();
        Task<CheckUserResponse> GetUserByUsernameAsync(string username);
    }
}
