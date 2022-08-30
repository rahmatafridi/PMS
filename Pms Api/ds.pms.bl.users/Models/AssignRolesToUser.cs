using System.Collections.Generic;

namespace ds.pms.bl.users.Models
{
    public class AssignRolesToUser
    {
        public int UserId { get; set; }
        public List<int> RoleIds { get; set; }
    }
}
