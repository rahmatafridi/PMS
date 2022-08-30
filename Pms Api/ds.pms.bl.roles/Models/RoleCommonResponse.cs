using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.roles.Models
{
    public class RoleCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Role Role { get; set; }
        public UpdateRole UpdateRole { get; set; }
    }
}
