namespace WebAPIRestaurantManagement.ModelResponses
{
    
    public class SP_GetUserByUNameResponse
    {
        public long Uid { get; set; }      // Trường UID kiểu INT8 (long)
        public string NameUser { get; set; } // Trường NameUser kiểu varchar
        public string EmailUser { get; set; } // Trường EmailUser kiểu varchar
        public SP_GetUserByUNameResponse(long uid, string nameUser, string emailUser, string passUser)
        {
            Uid = uid;
            NameUser = nameUser;
            EmailUser = emailUser;
        }

        // Hàm mặc định không tham số
        public SP_GetUserByUNameResponse() { }
    }
}
