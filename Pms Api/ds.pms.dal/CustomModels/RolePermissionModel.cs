using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.dal.CustomModels
{
    public sealed class RolePermissionModel
    {
        public long Id { get; set; }
        public string Feature_Id { get; set; }
        public string Feature { get; set; }
        public long Parent_Feature_Id { get; set; }
        public bool is_checked { get; set; }
    }
}
