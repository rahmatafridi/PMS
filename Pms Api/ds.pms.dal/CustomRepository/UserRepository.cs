using Dapper;
using ds.pms.apicommon.Models;
using ds.pms.dal.GenericRepository;
using ds.pms.dal.Models;
using ds.pms.dal.SortFields;
using LinqToDB;
using LinqToDB.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ds.pms.dal.CustomRepository
{
    public class UserRepository : BaseCustomRepository
    {
        private GenericRepository<TblUser> userGenericRepository;
        private GenericRepository<TblUserRole> userRoleGenericRepository;
        private GenericRepository<TblProviderUser> providerUserGenericRepository;
        private GenericRepository<TblSecurityRolePermission> permissionGenericRepository;

        public UserRepository(string databaseProvider, string connectionString) : base(databaseProvider, connectionString)
        {
            userGenericRepository = new GenericRepository<TblUser>(databaseProvider, connectionString);
            userRoleGenericRepository = new GenericRepository<TblUserRole>(databaseProvider, connectionString);
            providerUserGenericRepository = new GenericRepository<TblProviderUser>(databaseProvider, connectionString);
            permissionGenericRepository = new GenericRepository<TblSecurityRolePermission>(databaseProvider, connectionString);
        }

        public PaginatedList<TblUser> GetActiveUserList(int clientId, string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<TblUser> paginatedUsers = new PaginatedList<TblUser>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                UserSortFields sortField = sortBy.GetUserField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
                var query = dataContext.TblUsers.Where(u => u.IsActive && !u.IsDeleted);

                if (clientId > 0)
                    query = query.Where(u => u.ClientId == clientId);

                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(u => (u.Username != null && u.Username.ToLower().Contains(search))
                                        || (u.Displayname != null && u.Displayname.ToLower().Contains(search))
                                        || (u.Email != null && u.Email.ToLower().Contains(search))
                                        || (u.Mobile != null && u.Mobile.ToLower().Contains(search))
                                        );
                }

                // Sorting
                if (sortField != UserSortFields.None && sortDirection != SortDirection.None)
                {
                    if (sortDirection == SortDirection.Asc)
                        query = query.OrderBy(sortField.GetColumn());
                    else if (sortDirection == SortDirection.Desc)
                        query = query.OrderByDescending(sortField.GetColumn());
                }

                // Pagination
                paginatedUsers.TotalCount = query.LongCount();
                paginatedUsers.PageSize = limit;
                paginatedUsers.CurrentPage = page;
                if (limit != -1)
                    query = query.Skip((page - 1) * limit).Take(limit);

                paginatedUsers.Items = query.ToList();

                return paginatedUsers;
            }
        }
        public PaginatedList<TblUser> GetUserListByClient(int clientId, string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<TblUser> paginatedUsers = new PaginatedList<TblUser>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                UserSortFields sortField = sortBy.GetUserField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();

                //Select Query
                var query = dataContext.TblUsers.Where(u => u.ClientId== clientId  && !u.IsDeleted);

                //if (clientId > 0)
                //    query = query.Where(u => u.ClientId == clientId);

                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(u => (u.Username != null && u.Username.ToLower().Contains(search))
                                        || (u.Displayname != null && u.Displayname.ToLower().Contains(search))
                                        || (u.Email != null && u.Email.ToLower().Contains(search))
                                        || (u.Mobile != null && u.Mobile.ToLower().Contains(search))
                                        );
                }

                // Sorting
                if (sortField != UserSortFields.None && sortDirection != SortDirection.None)
                {
                    if (sortDirection == SortDirection.Asc)
                        query = query.OrderBy(sortField.GetColumn());
                    else if (sortDirection == SortDirection.Desc)
                        query = query.OrderByDescending(sortField.GetColumn());
                }

                // Pagination
                paginatedUsers.TotalCount = query.LongCount();
                paginatedUsers.PageSize = limit;
                paginatedUsers.CurrentPage = page;
                if (limit != -1)
                    query = query.Skip((page - 1) * limit).Take(limit);

                paginatedUsers.Items = query.ToList();

                return paginatedUsers;
            }
        }

        public TblUser GetUserById(int userId)
        {
            return userGenericRepository.GetById(userId);
        }

        public TblUser GetUserByClientAndUserId(long userId, long clientId)
        {
            return userGenericRepository.GetById(userId, clientId);
        }

        public TblUser Add(TblUser user)
        {
            return userGenericRepository.Insert(user);
        }

        public TblUser Update(TblUser user)
        {
            return userGenericRepository.Update(user);
        }

        public TblUser AuthenticateUser(string username, string password)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                var query = dataContext.TblUsers.Where(u => u.Username == username && u.Password == password && u.IsActive && !u.IsDeleted);
                if (query != null)
                {
                    var data = dataContext.TblRoles.Where(x => x.ClientId == 0).FirstOrDefault();
                    if(data != null)
                    {
                        var checkPermission = dataContext.TblSecurityRolePermissions.Where(x => x.RoleId == data.Id).ToList();
                        if(checkPermission.Count ==0)
                        {
                            var permission = dataContext.TblSecurityFeaturePermissions.ToList();
                            foreach (var item in permission)
                            {
                                var add = new TblSecurityRolePermission();
                                add.PermissionId = item.Id;
                                add.RoleId = data.Id;

                                permissionGenericRepository.Insert(add);
                            }
                        }
                    }

                }
                return query.FirstOrDefault();
            }
        }

        public bool DoesUsernameExists(string username)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                var query = dataContext.TblUsers.Any(u => u.Username.ToLower() == username.ToLower() && !u.IsDeleted);
                return query;
            }
        }

        public bool DoesEmailExists(string email)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                var query = dataContext.TblUsers.Any(u => u.Email.ToLower() == email.ToLower() && !u.IsDeleted);
                return query;
            }
        }

        public bool DoesAnyOtherUserUseThisUsername(long? userId, string username)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                if (userId != null && userId.HasValue && userId.Value > 0)
                    return dataContext.TblUsers.Any(u => u.Id != userId.Value && u.Username.ToLower() == username.ToLower() && !u.IsDeleted);
                else
                    return dataContext.TblUsers.Any(u => u.Username.ToLower() == username.ToLower() && !u.IsDeleted);
            }
        }

        public bool DoesAnyOtherUserUseThisEmail(long? userId, string email)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                if (userId != null && userId.HasValue && userId.Value > 0)
                    return dataContext.TblUsers.Any(u => u.Id != userId.Value && u.Email.ToLower() == email.ToLower() && !u.IsDeleted);
                else
                    return dataContext.TblUsers.Any(u => u.Email.ToLower() == email.ToLower() && !u.IsDeleted);
            }
        }

        public bool Delete(int userId)
        {
            return userGenericRepository.DeleteById(userId);
        }

        public bool AssignRoleToUser(int userId, List<int> roleIds)
        {
            if (roleIds.Count > 0)
            {
                foreach (var item in roleIds)
                {
                    TblUserRole tblUserRole = new TblUserRole();
                    tblUserRole.UserId = userId;
                    tblUserRole.RoleId = item;
                    userRoleGenericRepository.Insert(tblUserRole);
                }
                return true;
            }
            return false;
        }

        public TblUserRole AssignRoleToUser(TblUserRole tblUserRole)
        {
            return userRoleGenericRepository.Insert(tblUserRole);
        }

        public bool RemoveRoleFromUser(int userId, List<int> roleIds)
        {
            if (roleIds.Count > 0)
            {
                using (dataContext = new PmsDB(providerName, connectionString))
                {
                    foreach (var item in roleIds)
                    {
                        var query = dataContext.TblUserRoles.Where(ur => ur.UserId == userId && ur.RoleId == item).Delete();
                    }
                }
                return true;
            }
            return false;
        }

        public TblProviderUser AddProviderUser(TblProviderUser tblProviderUser)
        {
            return providerUserGenericRepository.Insert(tblProviderUser);
        }

        public List<int> GetRolesByUserId(int userId)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                return dataContext.TblUserRoles.Where(r => r.UserId == userId).Select(x => x.RoleId).ToList();
            }
        }

        public int CreateUpdateUsers(TblUser createupdateuser)
        {
            int? isSuccess = 0, newUserId = 0;
            string message = string.Empty, error = string.Empty;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (createupdateuser.Id > 0)
                {
                    return 0;
                }
                else
                {
                    string storedProc = "[dbo].[sp_user_create]";
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@users_username", createupdateuser.Username);
                    dynamicParams.Add("@users_email", createupdateuser.Email);
                    dynamicParams.Add("@users_password", createupdateuser.Password);
                    dynamicParams.Add("@users_displayname", createupdateuser.Displayname);
                    dynamicParams.Add("@users_mobile", createupdateuser.Mobile);
                    dynamicParams.Add("@IsSuccess", ParameterDirection.Output);
                    dynamicParams.Add("@Message", ParameterDirection.Output);
                    dynamicParams.Add("@Error", ParameterDirection.Output);
                    dynamicParams.Add("@New_UserId", ParameterDirection.Output);
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    newUserId = dynamicParams.Get<int>("@New_UserId");
                    isSuccess = dynamicParams.Get<int>("@IsSuccess");

                    if (isSuccess != 1 && newUserId == 0)
                        newUserId = 0;
                }
            }
            return (int)newUserId;
        }
    }
}
