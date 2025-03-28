namespace WebAPIRestaurantManagement.Settings
{
    public class JWT
    {
        public string ValidIsuser { get; set; }
        public string ValidAudience { get; set; }
        public string JWTSecret { get; set; }
        public string SUPABASE_URL { get; set; }
        public string SUPABASE_KEY { get; set; }
  }
}
