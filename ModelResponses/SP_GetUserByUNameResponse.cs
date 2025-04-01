namespace WebAPIRestaurantManagement.ModelResponses
{
    
    public class SP_GetUserByUNameResponse
    {
        public string Uid { get; set; }      // Trường UID kiểu INT8 (long)
        public string NameUser { get; set; } // Trường NameUser kiểu varchar
        public string EmailUser { get; set; } // Trường EmailUser kiểu varchar
        public int rolesUser { get; set; }
        public SP_GetUserByUNameResponse(string uid, string nameUser, string emailUser, string passUser, int userRole)
        {
            Uid = uid;
            NameUser = nameUser;
            EmailUser = emailUser;
            rolesUser = userRole;
        }

        // Hàm mặc định không tham số
        public SP_GetUserByUNameResponse() { }
    }
}
