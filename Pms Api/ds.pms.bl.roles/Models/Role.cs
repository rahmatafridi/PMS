using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.bl.roles.Models
{
    public class Role
    {
        public int Id { get; set; } // int
        public int ClientId { get; set; } // int
        public string Name { get; set; } // nvarchar(100)
        public string Description { get; set; } // nvarchar(256)
        public string ClientName { get; set; } // nvarchar(256)


        public static implicit operator Role(TblRole dbRole)
        {
            if (dbRole != null)
            {
                Role dlRole = new Role()
                {
                    Id = dbRole.Id,
                    ClientId = dbRole.ClientId,
                    Name = dbRole.Name,
                    Description = dbRole.Description,
                };
                return dlRole;
            }
            return null;
        }

        public static implicit operator TblRole(Role dlRole)
        {
            if (dlRole != null)
            {
                TblRole dbRole = new TblRole()
                {
                    Id = dlRole.Id,
                    ClientId = dlRole.ClientId,
                    Name = dlRole.Name,
                    Description = dlRole.Description,
                };
                return dbRole;
            }
            return null;
        }

        public static implicit operator Role(RoleList dbRole)
        {
            if (dbRole != null)
            {
                Role dlRole = new Role()
                {
                    Id = dbRole.Id,
                    ClientId = dbRole.ClientId,
                    Name = dbRole.Name,
                    Description = dbRole.Description,
                    ClientName = dbRole.ClientName,
                };
                return dlRole;
            }
            return null;
        }

        public static implicit operator RoleList(Role dlRole)
        {
            if (dlRole != null)
            {
                RoleList dbRole = new RoleList()
                {
                    Id = dlRole.Id,
                    ClientId = dlRole.ClientId,
                    Name = dlRole.Name,
                    Description = dlRole.Description,
                    ClientName = dlRole.ClientName,
                };
                return dbRole;
            }
            return null;
        }
    }
}
