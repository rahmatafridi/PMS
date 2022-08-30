using ds.pms.apicommon.Models;
using ds.pms.bl.users.Models;
using System.Collections.Generic;

namespace ds.pms.bl.users.IServices
{
    public interface IUserService
    {
        PaginatedList<User> GetActiveUserList(int clientId, string search, int? limit = 10, int? page = 1, string sort = "");
        User GetUserById(int userId);
        UserCommonResponse AddUser(AddUser addUser, string userName);
        UserCommonResponse UpdateUser(UpdateUser updateUser, string userName);
        UserCommonResponse AuthenticateUser(LoginUser loginUser);
        bool IsValidUsername(string username);
        bool IsValidEmail(string email);
        bool IsValidUsername(int? userId, string username);
        bool IsValidEmail(int? userId, string email);
        bool IsValidUsernameEmail(string email, string username);
        bool IsValidUsernameEmail(int userId, string email, string username);
        UserCommonResponse SoftDelete(int userId, string userName);
        bool HardDelete(int userId);
        bool ChangePassword(int clientId, int userId, string password, int loggedInUserId);
        bool AssignRolesToUser(AssignRolesToUser assignRolesToUser);
        bool RemoveRolesFromUser(AssignRolesToUser assignRolesToUser);
        bool AddProviderUser(int providerId, int userId);
        List<int> GetRolesByUserId(int userId);
        PaginatedList<User> GetUserListByClient(int clientId, string search, int? limit = 10, int? page = 1, string sort = "");

    }
}
