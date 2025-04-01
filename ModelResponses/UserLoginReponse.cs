namespace WebAPIRestaurantManagement.ModelResponses
{
    public class UserLoginReponse
    {
        public Guid Uid { get; set; }
        public string UName { get; set; }
        public string email { get; set; }
        public RolesResponse RoleUser {  get; set; }
        public List<SP_GetRightByUidResponse> Rights { get; set; }
        public UserLoginReponse (Guid uid, string uName, string email, RolesResponse rolesUser, List<SP_GetRightByUidResponse> rights)
        {
            this.Uid = uid;
            this.UName = uName;
            this.email = email;
            this.Rights = rights;
            this.RoleUser = rolesUser;
        }
        public UserLoginReponse(){}
    }
}
