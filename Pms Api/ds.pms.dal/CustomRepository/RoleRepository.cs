using ds.pms.apicommon.Models;
using ds.pms.dal.CustomModels;
using ds.pms.dal.GenericRepository;
using ds.pms.dal.Models;
using ds.pms.dal.SortFields;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ds.pms.dal.CustomRepository
{
    public class RoleRepository : BaseCustomRepository
    {
        private GenericRepository<TblRole> rolesGenericRepository;
        private GenericRepository<TblSecurityRolePermission> permissionGenericRepository;
        public RoleRepository(string databaseProvider, string connectionString) : base(databaseProvider, connectionString)
        {
            rolesGenericRepository = new GenericRepository<TblRole>(databaseProvider, connectionString);
            permissionGenericRepository = new GenericRepository<TblSecurityRolePermission>(databaseProvider, connectionString);
        }

        public PaginatedList<RoleList> GetActiveRoleList(int clientId, string search, int limit = 10, int page = 1, string sortBy = "", string sortDir = "")
        {
            PaginatedList<RoleList> paginatedRoles = new PaginatedList<RoleList>();
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                RoleSortFields sortField = sortBy.GetRoleField();
                SortDirection sortDirection = sortDir.GetSortDirectionByName();


                var query = (from role in dataContext.TblRoles
                             from client in dataContext.TblClients.LeftJoin(c => c.Id == role.ClientId && c.IsActive && !c.IsDeleted)
                             where role.IsActive
                             select new RoleList
                             {
                                 Id = role.Id,
                                 Name = role.Name,
                                 Description = role.Description,
                                 ClientId = role.ClientId,
                                 ClientName = client.Name ?? string.Empty,
                             });

                if (clientId > 0)
                {
                    query = query.Where(r => r.ClientId == clientId);
                }

                // Search - go deeper
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(r => (r.Name != null && r.Name.ToLower().Contains(search))
                                        || (r.Description != null && r.Description.ToLower().Contains(search))
                                        || (r.ClientName != null && r.ClientName.ToLower().Contains(search))
                                        );

                }

                // Sorting
                if (sortField != RoleSortFields.None && sortDirection != SortDirection.None)
                {
                    if (sortDirection == SortDirection.Asc)
                        query = query.OrderBy(sortField.GetColumn());
                    else if (sortDirection == SortDirection.Desc)
                        query = query.OrderByDescending(sortField.GetColumn());
                }

                // Pagination
                paginatedRoles.TotalCount = query.LongCount();
                paginatedRoles.PageSize = limit;
                paginatedRoles.CurrentPage = page;
                if (limit != -1)
                    query = query.Skip((page - 1) * limit).Take(limit);

                paginatedRoles.Items = query.ToList();

                return paginatedRoles;
            }
        }

        public TblRole GetRoleById(int roleId)
        {
            return rolesGenericRepository.GetById(roleId);
        }

        public List<TblRole> GetAdminRoles()
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                return dataContext.TblRoles.Where(r => r.ClientId == 0 && r.IsTemplate == true).ToList();
            }
        }
        public List<TblRole> GetRolesByClientId(int clientId)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                return dataContext.TblRoles.Where(r => r.ClientId == clientId && r.IsActive).ToList();
            }
        }

        public TblRole Add(TblRole addRole)
        {
            return rolesGenericRepository.Insert(addRole);
        }
        public string AddPermission (List<string> lines)
            {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                bool isDelete = false;
                foreach (var item in lines)
                {
                    string[] values = item.Split(',');
                    if(isDelete == false)
                    {
                        var listAssignPermission = dataContext.TblSecurityRolePermissions
                            .Where(x => x.RoleId == Convert.ToInt32(values[0])).ToList();
                        if (listAssignPermission.Count > 0)
                        {
                            foreach (var pItem in listAssignPermission)
                            {
                                isDelete = permissionGenericRepository.DeleteById(pItem.Id);
                               
                            }
                            
                        }
                        else
                        {
                            isDelete = true;
                        }
                        

                    }
                    var tblRolePermission = new TblSecurityRolePermission();
                    tblRolePermission.PermissionId = values[2];
                    tblRolePermission.RoleId = Convert.ToInt32(values[0]);

                     permissionGenericRepository.Insert(tblRolePermission);

                }
                return "success";

            }
            }
        public TblRole Update(TblRole updateRole)
        {
            return rolesGenericRepository.Update(updateRole);
        }
        public List<RoleFeatureModel> GetRolePermission(int roleId, int clientId)
        {
            //var data = new Dashboard();
            RoleFPModel fpModel = new RoleFPModel();

            using (dataContext = new PmsDB(providerName, connectionString))
            {
                var restul1 = dataContext.UpWebUserRoleGetListFeaturesPermissionsForRole(roleId, 1).ToList();
                //if(clientId != 0)
                //{
                //    restul1.Where(x => x.is_system_only == false);
                //}

                var roleFeature = new List<RoleFeatureModel>();
                if(clientId > 0)
                {
                    restul1 = restul1.Where(x => x.is_system_only != true).ToList();
                }
                foreach (var item in restul1)
                {
                    var list = new List<RolePermissionModel>();


                    var perList = restul1.Where(x => x.feature_id == item.feature_id);
                    foreach (var item1 in perList)
                    {
                        bool isChecked = false;
                        var isRole = dataContext.TblSecurityRolePermissions.
                            Where(x => x.RoleId == roleId && x.PermissionId == item1.Column4).FirstOrDefault();
                        if(isRole != null)
                        {
                            isChecked = true;
                        }
                        list.Add(new RolePermissionModel
                        {
                            Feature_Id = item1.Column4.ToString(),
                            Parent_Feature_Id = (long)item1.feature_id,
                            Feature = item1.description,
                            is_checked = isChecked
                        });
                    }
                    roleFeature.Add(new RoleFeatureModel()
                    {
                        Feature = item.feature,
                        Feature_Id = (long)item.feature_id,
                        Permissions = list
                    });

                }

                var finalData = roleFeature.GroupBy(x => x.Feature_Id).Select(y => y.FirstOrDefault()).ToList();


                return finalData;
                //var result2 = dataContext.

            }

            
        }

        public bool DoesRoleNameExists(int clientId, string roleName)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                var query = dataContext.TblRoles.Any(r => r.ClientId == clientId && r.Name.ToLower() == roleName.ToLower());
                return query;
            }
        }

        public bool DoesAnyOtherRoleUseThisRoleName(int? roleId, int clientId, string username)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                if (roleId != null && roleId.HasValue && roleId.Value > 0)
                    return dataContext.TblRoles.Any(r => r.Id != roleId.Value && r.ClientId == clientId && r.Name.ToLower() == username.ToLower());
                else
                    return dataContext.TblRoles.Any(r => r.ClientId == clientId && r.Name.ToLower() == username.ToLower());
            }
        }
        public int GetRoleIdByUser(int userId)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                return dataContext.TblUserRoles.Where(x => x.UserId == userId).FirstOrDefault().RoleId;
            }
        }

        public List<TblSecurityRolePermission> RolePermission(int roleId)
        {
            using (dataContext = new PmsDB(providerName, connectionString))
            {
                return dataContext.TblSecurityRolePermissions.Where(x => x.RoleId == roleId).ToList();
            }

            }
    }
}
