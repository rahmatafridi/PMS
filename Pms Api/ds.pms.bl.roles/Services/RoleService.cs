using ds.pms.apicommon.Models;
using ds.pms.bl.logger;
using ds.pms.bl.roles.Converters;
using ds.pms.bl.roles.IServices;
using ds.pms.bl.roles.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.CustomRepository;
using ds.pms.dal.Models;
using ds.pms.helpers.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ds.pms.bl.roles.Services
{
    public class RoleService : IRoleService
    {
        private RoleRepository roleRepository;
        private readonly string jwtSecretKey;
        private int tokenTimeoutSeconds;
        private Logging logging;
        private readonly string _className;
        private string _methodName;

        public RoleService(string _databaseProvider, string _connectionString, ILogger<RoleService> logger)
        {
            roleRepository = new RoleRepository(_databaseProvider, _connectionString);
            logging = new Logging(logger);
            _className = this.GetType().Name;
        }

        public RoleService(string _databaseProvider, string _connectionString, string jwtSecret, int tokenTimeoutInSeconds, ILogger<RoleService> logger)
        {
            roleRepository = new RoleRepository(_databaseProvider, _connectionString);
            jwtSecretKey = jwtSecret;
            tokenTimeoutSeconds = tokenTimeoutInSeconds;
            logging = new Logging(logger);
            _className = this.GetType().Name;
        }

        public PaginatedList<Role> GetActiveRoleList(int clientId, string search, int? limit = 10, int? page = 1, string sort = "")
        {
            PaginatedList<Role> rolesPaginatedList = new PaginatedList<Role>();
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
                rolesPaginatedList = Pagination.ConvertDalToBl(roleRepository.GetActiveRoleList(clientId, search, pageSize, pageNumber, sortBy, sortDirection));

            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return rolesPaginatedList;
        }

        public Role GetRoleById(int roleId)
        {
            try
            {
                if (roleId > 0)
                {
                    Role role = roleRepository.GetRoleById(roleId);
                    if (role != null)
                        return role;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public List<Role> GetRolesByClientId(int clientId)
        {
            try
            {
                if (clientId > 0)
                {
                    List<Role> roles = Pagination.ConvertDalToBlRoleList(roleRepository.GetRolesByClientId(clientId));
                    if (roles != null && roles.Count > 0)
                        return roles;
                }
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public RoleCommonResponse AddRole(Role addRole, string userName)
        {
            RoleCommonResponse roleCommonResponse = new RoleCommonResponse();
            try
            {
                roleCommonResponse.Success = false;
                if (addRole != null)
                {
                    if (IsValidRoleName(addRole.ClientId, addRole.Name))
                    {
                        TblRole tblRole = addRole;
                        tblRole.IsActive = true;
                        tblRole.AddedBy = userName;
                        tblRole.AddedDate = DateTime.UtcNow;
                        roleCommonResponse.Role = roleRepository.Add(tblRole);
                        if (roleCommonResponse.Role != null && roleCommonResponse.Role.Id > 0)
                        {
                            roleCommonResponse.Success = true;
                            return roleCommonResponse;
                        }
                        else
                            roleCommonResponse.Message = "Unable to add role.";
                    }
                    else
                        roleCommonResponse.Message = "RoleName already in use with this client.";
                }
                else
                    roleCommonResponse.Message = "Supplied role information is not valid.";
            }
            catch (Exception ex)
            {
                roleCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return roleCommonResponse;
        }

        public RoleCommonResponse UpdateRole(UpdateRole updateRole, string userName)
        {
            RoleCommonResponse roleCommonResponse = new RoleCommonResponse();
            try
            {
                roleCommonResponse.Success = false;
                if (updateRole != null && updateRole.Id > 0)
                {
                    if (IsValidRoleName(updateRole.Id, updateRole.ClientId, updateRole.Name))
                    {
                        TblRole tblRole;
                        tblRole = roleRepository.GetRoleById(updateRole.Id);
                        if (tblRole != null)
                        {
                            tblRole.ClientId = updateRole.ClientId;
                            tblRole.Name = updateRole.Name;
                            tblRole.Description = updateRole.Description;
                            //tblRole.IsActive = updateRole.IsActive;
                            tblRole.ModifiedBy = userName;
                            tblRole.ModifiedDate = DateTime.UtcNow;
                            roleCommonResponse.UpdateRole = roleRepository.Update(tblRole);
                            if (roleCommonResponse.UpdateRole != null && roleCommonResponse.UpdateRole.Id > 0)
                            {
                                roleCommonResponse.Success = true;
                                return roleCommonResponse;
                            }
                            else
                                roleCommonResponse.Message = "Unable to update role.";
                        }
                        else
                            roleCommonResponse.Message = "Role does not exists.";
                    }
                    else
                        roleCommonResponse.Message = "RoleName already in use with this client.";
                }
                else
                    roleCommonResponse.Message = "Supplied role information is not valid.";
            }
            catch (Exception ex)
            {
                roleCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {
            }
            return roleCommonResponse;
        }

        public bool IsValidRoleName(int clientId, string roleName)
        {
            try
            {
                return !roleRepository.DoesRoleNameExists(clientId, roleName);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                return false;
            }
        }

        public bool IsValidRoleName(int roleId, int clientId, string roleName)
        {
            try
            {
                return !roleRepository.DoesAnyOtherRoleUseThisRoleName(roleId, clientId, roleName);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
                return false;
            }
        }

        public RoleCommonResponse SoftDelete(int roleId, string userName)
        {
            RoleCommonResponse clientCommonResponse = new RoleCommonResponse();
            try
            {
                clientCommonResponse.Success = false;
                if (roleId > 0)
                {
                    TblRole tblClient = roleRepository.GetRoleById(roleId);
                    if (tblClient != null && tblClient.Id > 0)
                    {
                        tblClient.ModifiedBy = userName;
                        tblClient.ModifiedDate = DateTime.UtcNow;
                        tblClient.IsActive = false;
                        clientCommonResponse.UpdateRole = roleRepository.Update(tblClient);
                        if (clientCommonResponse.UpdateRole != null && clientCommonResponse.UpdateRole.Id > 0)
                        {
                            clientCommonResponse.Success = true;
                            return clientCommonResponse;
                        }
                        else
                            clientCommonResponse.Message = "Unable to delete role.";
                    }
                    else
                        clientCommonResponse.Message = "Role does not exists.";
                }
                else
                    clientCommonResponse.Message = "Supplied role information is not valid.";
            }
            catch (Exception ex)
            {
                clientCommonResponse.Message = "Exception Occured." + ex.Message;
                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            finally
            {

            }
            return clientCommonResponse;
        }

        public List<RoleFeatureModel> LoadPermission(int roleId, int clientId)
        {
            try
            {
                return roleRepository.GetRolePermission(roleId,clientId);
            }
            catch (Exception ex)
            {

                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }

        public string AddPermission(List<string> lines)
        {
            try
            {
                return roleRepository.AddPermission(lines);
            }
            catch (Exception ex)
            {

                _methodName = MethodBase.GetCurrentMethod().Name;
                logging.LogError(ex, logging.GetExceptionMessage(_methodName, _className));
            }
            return null;
        }
    }
}
