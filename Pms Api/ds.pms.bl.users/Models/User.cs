using ds.pms.dal.Models;
using System.Collections.Generic;

namespace ds.pms.bl.users.Models
{
    public class User
    {
        public int Id { get; set; } // users_id (Primary key)
        public int ClientId { get; set; } // int 
        public string UserName { get; set; } // users_username (length: 100)
        public string Email { get; set; } // users_email (length: 255)
        //public string Password { get; set; } // users_password (length: 100)
        public string DisplayName { get; set; } // users_displayname (length: 200)
        public bool IsActive { get; set; } // users_isactive
        //public byte? Isexcluded { get; set; } // users_isexcluded
        //public DateTime? PasswordChangedDate { get; set; } // users_password_changed_date
        public string Mobile { get; set; } // users_mobile (length: 50)
        public bool IsEmailNotification { get; set; } // users_is_email_notification
        //public DateTime? AddedDate { get; set; } // users_created_date
        //public DateTime? ModifiedDate { get; set; } // users_modified_date
        //public DateTime? DeletedDate { get; set; } // users_deleted_date

        public List<UserPermission> PermissionList { get; set; }
        public static implicit operator User(TblUser dbUser)
        {
            if (dbUser != null)
            {
                User dlUser = new User()
                {
                    Id = dbUser.Id,
                    ClientId = dbUser.ClientId,
                    UserName = dbUser.Username,
                    Email = dbUser.Email,
                    DisplayName = dbUser.Displayname,
                    Mobile = dbUser.Mobile,
                    IsEmailNotification = dbUser.IsEmailNotification,
                    IsActive= dbUser.IsActive
                    //AddedDate = dbUser.AddedDate,
                    //ModifiedDate = dbUser.ModifiedDate
                };
                return dlUser;
            }
            return null;
        }
    }

    public class UserPermission
    {
        public int RoleId { get; set; }
        public string Permission { get; set; }
        
    }
}
