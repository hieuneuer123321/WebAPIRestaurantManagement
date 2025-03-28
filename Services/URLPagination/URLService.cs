﻿using Microsoft.AspNetCore.WebUtilities;
using WebAPIRestaurantManagement.Helpers;
using WebAPIRestaurantManagement.Services.URLPagination;

namespace WebAPIRestaurantManagement.Services.URI
{
    public class URLService : IURLService
    {
        private readonly string _baseUri;
        public URLService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetPageUri(PaginationFilterHelper filter, string route)
        {
            var _endpointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(_endpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}
