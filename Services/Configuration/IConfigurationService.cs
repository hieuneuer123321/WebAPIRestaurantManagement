using WebAPIRestaurantManagement.Settings;

namespace WebAPIRestaurantManagement.Services.Configuration
{
    public interface IConfigurationService
    {
        Cryptography GetCryptography();
        JWT GetJWT();
    }
}
