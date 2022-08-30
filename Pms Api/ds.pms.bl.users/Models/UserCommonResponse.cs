namespace ds.pms.bl.users.Models
{
    public class UserCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public AddUser AddUser { get; set; }
        public UpdateUser UpdateUser { get; set; }
    }
}
