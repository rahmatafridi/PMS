using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.roles.Models
{
    public class UpdateRole : Role
    {
        public bool IsActive { get; set; }

        public static implicit operator UpdateRole(TblRole dbRole)
        {
            if (dbRole != null)
            {
                UpdateRole dlRole = new UpdateRole()
                {
                    Id = dbRole.Id,
                    Name = dbRole.Name,
                    Description = dbRole.Description,
                    IsActive = dbRole.IsActive,
                };
                return dlRole;
            }
            return null;
        }

        public static implicit operator TblRole(UpdateRole dlRole)
        {
            if (dlRole != null)
            {
                TblRole dbRole = new TblRole()
                {
                    Id = dlRole.Id,
                    Name = dlRole.Name,
                    Description = dlRole.Description,
                    IsActive = dlRole.IsActive,
                };
                return dbRole;
            }
            return null;
        }
    }
}
