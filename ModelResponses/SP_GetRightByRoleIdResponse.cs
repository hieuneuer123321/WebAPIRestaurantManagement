namespace WebAPIRestaurantManagement.ModelResponses
{
    public class SP_GetRightByRoleIdResponse
    {
        public string Right_id_pro { get; set; }      // Trường UID kiểu INT8 (long)
        public string Right_name_pro { get; set; } // Trường NameUser kiểu varchar
        public SP_GetRightByRoleIdResponse(string right_id_pro, string right_name_pro)
        {
            Right_id_pro = right_id_pro;
            Right_name_pro = right_name_pro;
        }
        // Hàm mặc định không tham số
        public SP_GetRightByRoleIdResponse() { }
    }
}
