using WebAPIRestaurantManagement.Helpers;
namespace WebAPIRestaurantManagement.Services.URLPagination
{
    public interface IURLService
    {
        public Uri GetPageUri(PaginationFilterHelper filter, string route);
    }
}
