using ds.pms.apicommon.Models;
using ds.pms.bl.roles.Models;
using ds.pms.dal.CustomModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.roles.IServices
{
    public interface IRoleService
    {
        PaginatedList<Role> GetActiveRoleList(int clientId, string search, int? limit = 10, int? page = 1, string sort = "");
        Role GetRoleById(int roleId);
        RoleCommonResponse AddRole(Role addRole, string userName);
        string AddPermission(List<String> lines);
        RoleCommonResponse UpdateRole(UpdateRole updateRole, string userName);
        bool IsValidRoleName(int clientId, string username);
        bool IsValidRoleName(int roleId, int clientId, string roleName);
        List<Role> GetRolesByClientId(int clientId);
        RoleCommonResponse SoftDelete(int roleId, string userName);
        List<RoleFeatureModel> LoadPermission(int roleId, int clinetId);
    }
}
