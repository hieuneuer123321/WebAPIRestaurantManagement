﻿namespace WebAPIRestaurantManagement.Helpers
{
    public class PaginationFilterHelper
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public PaginationFilterHelper()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public PaginationFilterHelper(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 100 ? 100 : pageSize;
        }
    }
}
