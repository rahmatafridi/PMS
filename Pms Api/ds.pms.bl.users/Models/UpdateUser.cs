using ds.pms.dal.Models;

namespace ds.pms.bl.users.Models
{
    public class UpdateUser : AddUser
    {
        //public string Password { get; set; }

        public bool IsActive { get; set; }

        //public DateTime? PasswordChangedDate { get; set; }

        public static implicit operator UpdateUser(TblUser dbUser)
        {
            if (dbUser != null)
            {
                UpdateUser dlUser = new UpdateUser()
                {
                    Id = dbUser.Id,
                    ClientId = dbUser.ClientId,
                    UserName = dbUser.Username,
                    Email = dbUser.Email,
                    Password = dbUser.Password,
                    DisplayName = dbUser.Displayname,
                    Mobile = dbUser.Mobile,
                    IsEmailNotification = dbUser.IsEmailNotification,
                    IsActive = dbUser.IsActive,
                    //PasswordChangedDate = dbUser.PasswordChangedDate
                };
                return dlUser;
            }
            return null;
        }

        public static implicit operator TblUser(UpdateUser dlUser)
        {
            if (dlUser != null)
            {
                TblUser dbUser = new TblUser()
                {
                    Id = dlUser.Id,
                    ClientId = dlUser.ClientId,
                    Username = dlUser.UserName,
                    Email = dlUser.Email,
                    Password = dlUser.Password,
                    Displayname = dlUser.DisplayName,
                    Mobile = dlUser.Mobile,
                    IsEmailNotification = dlUser.IsEmailNotification,
                    IsActive = dlUser.IsActive,
                    //PasswordChangedDate = dlUser.PasswordChangedDate
                };
                return dbUser;
            }
            return null;
        }
    }
}
