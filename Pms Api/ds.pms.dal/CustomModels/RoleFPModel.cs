using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
    public class RoleFPModel
    {
        public IEnumerable<RoleFeatureModel> features;
        public IEnumerable<RolePermissionModel> permissions;
        public long Id { get; set; }
    }
}
