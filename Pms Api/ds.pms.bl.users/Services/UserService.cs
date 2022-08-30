using ds.pms.apicommon.Models;
using ds.pms.bl.logger;
using ds.pms.bl.users.Converters;
using ds.pms.bl.users.IServices;
using ds.pms.bl.users.Models;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using ds.pms.helpers.Extentions;
using ds.pms.helpers.Query;
using ds.pms.helpers.Security;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace ds.pms.bl.users.Services
{
    public class UserService : IUserService
    {
        private UserRepository userRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;
        private RoleRepository roleRepository;
        public UserService(string _databaseProvider, string _connectionString, ILogger<UserService> logger)
        {
            userRepository = new UserRepository(_databaseProvider, _connectionString);
            roleRepository = new RoleRepository(_databaseProvider, _connectionString);
            logging = new Logging(logger);
            _className = this.GetType().Name;
        }

        public UserService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds, ILogger<UserService> logger)
        {
            userRepository = new UserRepository(_databaseProvider, _connectionString);
            roleRepository = new RoleRepository(_databaseProvider, _connectionString);

            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(logger);
            _className = this.GetType().Name;
        }

        public UserCommonResponse AuthenticateUser(LoginUser loginUser)
        {
            UserCommonResponse userCommonResponse = new UserCommonResponse();
            try
            {
                userCommonResponse.Success = false;
                string password = Hash.GeneratePasswordHash(loginUser.Password);
                TblUser dbUser = userRepository.AuthenticateUser(loginUser.UserName, password);
                if (dbUser != null && dbUser.Id > 0)
                {
                    // authentication successful so generate jwt token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(jwtSecretKey);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.Sid, dbUser.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, dbUser.Username),
                    new Claim(ClaimTypes.Email, dbUser.Email),
                    new Claim(ClaimTypes.Name, dbUser.Displayname),
                    new Claim(ClaimTypes.GroupSid, dbUser.ClientId.ToString()),
                        }),

                        Expires = DateTime.UtcNow.AddSeconds(tokenTimeoutSeconds),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    userCommonResponse.Token = tokenHandler.WriteToken(token);
                    if (!string.IsNullOrEmpty(userCommonResponse.Token))
                    {

                        userCommonResponse.Success = true;
                    }
                    else
                        userCommonResponse.Message = "Unable to generate token.";
                }
                else
                    userCommonResponse.Message = "Username or password is incorrect.";
            }
            catch (Exception ex)
            {
                userCommonResponse.Message = "Exception occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return userCommonResponse;
        }

        public PaginatedList<User> GetActiveUserList(int clientId, string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<User> usersPaginatedList = new PaginatedList<User>();
            try
            {
                int pageSize = limit ?? 10;
                int pageNumber = page ?? 1;
                string sortBy = string.Empty, sortDirection = string.Empty;
                if (!string.IsNullOrEmpty(sort))
                {
                    string[] sorting = SortDirection.SortDir(sort);
                    if (sorting.Length > 1)
                    {
                        sortBy = sorting[0];
                        sortDirection = sorting[1];
                    }
                }
                usersPaginatedList = Pagination.ConvertDalToBl(userRepository.GetActiveUserList(clientId, search, pageSize, pageNumber, sortBy, sortDirection));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return usersPaginatedList;
        }

        public User GetUserById(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    User user = userRepository.GetUserById(userId);
                    //if (user.ClientId > 0)
                    //{
                        var roleId = roleRepository.GetRoleIdByUser(userId);
                        var listPermission = roleRepository.RolePermission(roleId);
                        var assignedPermission = new List<UserPermission>();
                        if (listPermission.Count > 0)
                        {
                            foreach (var item in listPermission)
                            {
                                assignedPermission.Add(new UserPermission()
                                {
                                    Permission = item.PermissionId,
                                    RoleId = (int)item.RoleId
                                });
                            }
                        }
                        user.PermissionList = assignedPermission;
                    //}
                    //if (user != null)
                        return user;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        private static string CreateRandomPassword(int length = 15)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        public UserCommonResponse AddUser(AddUser addUser, string userName)
        {
            UserCommonResponse userCommonResponse = new UserCommonResponse();
            try
            {
                userCommonResponse.Success = false;
                if (addUser != null)
                {
                    if (IsValidUsernameEmail(addUser.Email, addUser.UserName))
                    {
                        TblUser tblUser = addUser;
                        if (string.IsNullOrEmpty(addUser.Password))
                            addUser.Password = CreateRandomPassword(10);
                        tblUser.Password = Hash.GeneratePasswordHash(addUser.Password);
                        tblUser.IsActive = addUser.IsActive;
                        tblUser.AddedBy = userName;
                        tblUser.AddedDate = DateTime.UtcNow;
                        //tblUser.IsDeleted = false;
                        userCommonResponse.AddUser = userRepository.Add(tblUser);
                        if (userCommonResponse.AddUser != null && userCommonResponse.AddUser.Id > 0)
                        {
                            userCommonResponse.Success = true;
                            return userCommonResponse;
                        }
                        else
                            userCommonResponse.Message = "Unable to add user.";
                        //addUser.Id = userRepository.CreateUpdateUsers(tblUser);
                        //return addUser;
                    }
                    else
                        userCommonResponse.Message = "Username or Email already in use.";
                }
                else
                    userCommonResponse.Message = "Supplied user information is not valid.";
            }
            catch (Exception ex)
            {
                userCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return userCommonResponse;

        }

        public UserCommonResponse UpdateUser(UpdateUser updateUser, string userName)
        {
            UserCommonResponse userCommonResponse = new UserCommonResponse();
            try
            {
                userCommonResponse.Success = false;
                if (updateUser != null && updateUser.Id > 0)
                {
                    if (IsValidUsernameEmail(updateUser.Id, updateUser.Email, updateUser.UserName))
                    {
                        TblUser tblUser;
                        tblUser = userRepository.GetUserById(updateUser.Id);
                        if (tblUser != null)
                        {
                            tblUser.ClientId = updateUser.ClientId;
                            tblUser.Username = updateUser.UserName;
                            tblUser.Password = string.IsNullOrEmpty(updateUser.Password) ? tblUser.Password : Hash.GeneratePasswordHash(updateUser.Password);
                            tblUser.Email = updateUser.Email;
                            tblUser.Mobile = updateUser.Mobile;
                            tblUser.Displayname = updateUser.DisplayName;
                            tblUser.IsEmailNotification = updateUser.IsEmailNotification;
                            tblUser.IsActive = updateUser.IsActive;
                            tblUser.ModifiedBy = userName;
                            tblUser.ModifiedDate = DateTime.UtcNow;
                            userCommonResponse.UpdateUser = userRepository.Update(tblUser);
                            if (userCommonResponse.UpdateUser != null && userCommonResponse.UpdateUser.Id > 0)
                            {
                                userCommonResponse.Success = true;
                                return userCommonResponse;
                            }
                            else
                                userCommonResponse.Message = "Unable to update user.";
                        }
                        else
                            userCommonResponse.Message = "User does not exists.";
                    }
                    else
                        userCommonResponse.Message = "Username or Email already in use.";
                }
                else
                    userCommonResponse.Message = "Supplied user information is not valid.";
            }
            catch (Exception ex)
            {
                userCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return userCommonResponse;
        }

        public bool IsValidUsernameEmail(int userId, string email, string username)
        {
            try
            {
                return !userRepository.DoesAnyOtherUserUseThisEmail(userId, email) && !userRepository.DoesAnyOtherUserUseThisUsername(userId, username);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                return false;
            }
        }

        public bool IsValidUsername(string username)
        {
            try
            {
                return !userRepository.DoesUsernameExists(username);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                return false;
            }
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                return !userRepository.DoesEmailExists(email);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                return false;
            }
        }

        public bool IsValidUsername(int? userId, string username)
        {
            try
            {
                return !userRepository.DoesAnyOtherUserUseThisUsername(userId, username);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                return false;
            }
        }

        public bool IsValidEmail(int? userId, string email)
        {
            try
            {
                return !userRepository.DoesAnyOtherUserUseThisEmail(userId, email);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                return false;
            }
        }

        public bool IsValidUsernameEmail(string email, string username)
        {
            try
            {
                return !userRepository.DoesEmailExists(email) && !userRepository.DoesUsernameExists(username);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                return false;
            }
        }

        public UserCommonResponse SoftDelete(int userId, string userName)
        {
            UserCommonResponse userCommonResponse = new UserCommonResponse();
            try
            {
                userCommonResponse.Success = false;
                if (userId > 0)
                {
                    TblUser tblUser = userRepository.GetUserById(userId);
                    if (tblUser != null && tblUser.Id > 0)
                    {
                        tblUser.ModifiedBy = userName;
                        tblUser.ModifiedDate = DateTime.UtcNow;
                        tblUser.IsDeleted = true;
                        tblUser.DeletedBy = userName;
                        tblUser.DeletedDate = DateTime.Now;
                        userCommonResponse.UpdateUser = userRepository.Update(tblUser);
                        if (userCommonResponse.UpdateUser != null && userCommonResponse.UpdateUser.Id > 0)
                        {
                            userCommonResponse.Success = true;
                            return userCommonResponse;
                        }
                        else
                            userCommonResponse.Message = "Unable to delete user.";
                    }
                    else
                        userCommonResponse.Message = "User does not exists.";
                }
                else
                    userCommonResponse.Message = "Supplied user information is not valid.";
            }
            catch (Exception ex)
            {
                userCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {

            }
            return userCommonResponse;
        }

        public bool HardDelete(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    return userRepository.Delete(userId);
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return false;
        }

        public User GetCurrentUser(ClaimsPrincipal user)
        {
            try
            {
                if (user.Identity.IsAuthenticated)
                {
                    User currentUser = new User()
                    {
                        Id = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value.ToInt(),
                        UserName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                        Email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value,
                        DisplayName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value,
                        ClientId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GroupSid).Value.ToInt(),
                        //Role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value,
                    };
                    return currentUser;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public bool ChangePassword(int clientId, int userId, string password, int loggedInUserId)
        {
            try
            {
                if (clientId > 0 && userId > 0 && loggedInUserId > 0)
                {
                    //1 change for own user = logged in user = provided user id
                    //2- if current logged in user client and provided user id client  =  + client admin 
                    //3 - system admin 
                    password = Hash.GeneratePasswordHash(password);
                    TblUser usersPasswordTobeChanged = userRepository.GetUserByClientAndUserId(clientId, userId);
                    TblUser loggedInUser = userRepository.GetUserByClientAndUserId(clientId, loggedInUserId);
                    if (loggedInUser != null && loggedInUser.Id == userId)
                    //|| (usersPasswordTobeChanged.ClientId == loggedInUser.ClientId && loggedInUser.IsClientAdmin) 
                    //|| loggedInUser.IsSystemAdmin)
                    {
                        if (loggedInUser != null)
                        {
                            loggedInUser.Password = password;
                            loggedInUser.PasswordChangedDate = DateTime.UtcNow;
                            loggedInUser.ModifiedBy = loggedInUser.Displayname;
                            loggedInUser.ModifiedDate = DateTime.UtcNow;
                            userRepository.Update(loggedInUser);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return false;
        }

        public bool AssignRolesToUser(AssignRolesToUser assignRolesToUser)
        {
            bool result = false;
            try
            {
                if (assignRolesToUser != null && assignRolesToUser.UserId > 0 && assignRolesToUser.RoleIds.Count > 0)
                {
                    List<int> rolesInDB = GetRolesByUserId(assignRolesToUser.UserId);
                    List<int> rolesToRemoveFromDB = new List<int>();
                    List<int> rolesToAddInDB = new List<int>();

                    if (rolesInDB.Count > 0)
                    {
                        rolesToRemoveFromDB = rolesInDB.Except(assignRolesToUser.RoleIds).ToList();
                        rolesToAddInDB = assignRolesToUser.RoleIds.Except(rolesInDB).ToList();
                        //AssignRolesToUser assignRolesToUser1 = new AssignRolesToUser();
                        //assignRolesToUser1.UserId = assignRolesToUser.UserId;
                        if (rolesToRemoveFromDB.Count > 0)
                        {
                            //assignRolesToUser1.RoleIds = rolesToRemoveFromDB;
                            //RemoveRolesFromUser(assignRolesToUser1);
                            userRepository.RemoveRoleFromUser(assignRolesToUser.UserId, rolesToRemoveFromDB);
                        }
                        if (rolesToAddInDB.Count > 0)
                        {
                            foreach (var item in rolesToAddInDB)
                            {
                                try
                                {
                                    TblUserRole tblUserRole = new TblUserRole();
                                    tblUserRole.UserId = assignRolesToUser.UserId;
                                    tblUserRole.RoleId = item;
                                    tblUserRole = userRepository.AssignRoleToUser(tblUserRole);
                                    if (tblUserRole != null && tblUserRole.Id > 0)
                                        result = true;
                                }
                                catch (Exception ex)
                                {
                                    _methodName = MethodBase.GetCurrentMethod().Name;
                                    logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                                    continue;
                                }
                            }
                        }
                        result = true;
                    }
                    else
                    {
                        foreach (var item in assignRolesToUser.RoleIds)
                        {
                            try
                            {
                                TblUserRole tblUserRole = new TblUserRole();
                                tblUserRole.UserId = assignRolesToUser.UserId;
                                tblUserRole.RoleId = item;
                                tblUserRole = userRepository.AssignRoleToUser(tblUserRole);
                                if (tblUserRole != null && tblUserRole.Id > 0)
                                    result = true;
                            }
                            catch (Exception ex)
                            {
                                _methodName = MethodBase.GetCurrentMethod().Name;
                                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                                continue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                result = false;
            }
            return result;
        }

        public bool RemoveRolesFromUser(AssignRolesToUser assignRolesToUser)
        {
            bool result = false;
            try
            {
                if (assignRolesToUser != null && assignRolesToUser.UserId > 0 && assignRolesToUser.RoleIds.Count > 0)
                {
                    result = userRepository.RemoveRoleFromUser(assignRolesToUser.UserId, assignRolesToUser.RoleIds);
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return result;
        }

        public List<int> GetRolesByUserId(int userId)
        {
            List<int> roles = new List<int>();
            try
            {
                if (userId > 0)
                {
                    roles = userRepository.GetRolesByUserId(userId);
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return roles;
        }

        public bool AddProviderUser(int providerId, int userId)
        {
            try
            {
                if (providerId > 0 && userId > 0)
                {
                    TblProviderUser tblProviderUser = new TblProviderUser();
                    tblProviderUser.ProviderId = providerId;
                    tblProviderUser.UserId = userId;
                    tblProviderUser = userRepository.AddProviderUser(tblProviderUser);
                    if (tblProviderUser != null && tblProviderUser.Id > 0)
                        return true;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return false;
        }

        public PaginatedList<User> GetUserListByClient(int clientId, string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<User> usersPaginatedList = new PaginatedList<User>();
            try
            {
                int pageSize = limit ?? 10;
                int pageNumber = page ?? 1;
                string sortBy = string.Empty, sortDirection = string.Empty;
                if (!string.IsNullOrEmpty(sort))
                {
                    string[] sorting = SortDirection.SortDir(sort);
                    if (sorting.Length > 1)
                    {
                        sortBy = sorting[0];
                        sortDirection = sorting[1];
                    }
                }
                usersPaginatedList = Pagination.ConvertDalToBl(userRepository.GetUserListByClient(clientId, search, pageSize, pageNumber, sortBy, sortDirection));
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return usersPaginatedList;
        }
    }
}
