using ds.pms.dal.Models;

namespace ds.pms.bl.users.Models
{
    public class AddUser : User
    {
        public string Password { get; set; } // users_password (length: 100)

        public static implicit operator AddUser(TblUser dbUser)
        {
            if (dbUser != null)
            {
                AddUser dlUser = new AddUser()
                {
                    Id = dbUser.Id,
                    ClientId = dbUser.ClientId,
                    UserName = dbUser.Username,
                    Email = dbUser.Email,
                    Password = dbUser.Password,
                    DisplayName = dbUser.Displayname,
                    Mobile = dbUser.Mobile,
                    IsEmailNotification = dbUser.IsEmailNotification
                };
                return dlUser;
            }
            return null;
        }

        public static implicit operator TblUser(AddUser dlUser)
        {
            if (dlUser != null)
            {
                TblUser user = new TblUser()
                {
                    Id = dlUser.Id,
                    ClientId = dlUser.ClientId,
                    Username = dlUser.UserName,
                    Email = dlUser.Email,
                    Password = dlUser.Password,
                    Displayname = dlUser.DisplayName,
                    Mobile = dlUser.Mobile,
                    IsEmailNotification = dlUser.IsEmailNotification
                };
                return user;
            }
            return null;
        }
    }
}
