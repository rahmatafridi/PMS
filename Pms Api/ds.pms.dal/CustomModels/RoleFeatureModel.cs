using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
    public sealed class RoleFeatureModel
    {
        public long Id { get; set; }
        public long Feature_Id { get; set; }
        public string Feature { get; set; }
        public bool is_checked { get; set; }
        public List<RolePermissionModel> Permissions { get; set; }
    }
}
