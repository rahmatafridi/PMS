using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Linq.Expressions;

namespace ds.pms.dal.SortFields
{
    public enum RoleSortFields
    {
        None = 0,
        Id = 1,
        Name = 2,
        Description = 3,
        ClientName = 4
    }

    public static class RoleSortFieldsExtension
    {
        public static RoleSortFields GetRoleField(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == RoleSortFields.Id.ToString().ToLower())
                    return RoleSortFields.Id;
                else if (sortBy.ToLower() == RoleSortFields.Name.ToString().ToLower())
                    return RoleSortFields.Name;
                else if (sortBy.ToLower() == RoleSortFields.Description.ToString().ToLower())
                    return RoleSortFields.Description;
                else if (sortBy.ToLower() == RoleSortFields.ClientName.ToString().ToLower())
                    return RoleSortFields.ClientName;
            }
            return RoleSortFields.None;
        }

        public static string GetFieldName(this RoleSortFields sortField)
        {
            if (sortField != RoleSortFields.None)
                return sortField.ToString();
            return string.Empty;
        }

        public static Expression<Func<RoleList, object>> GetColumn(this RoleSortFields sortField)
        {
            switch (sortField)
            {
                case RoleSortFields.Id:
                    return x => x.Id;
                case RoleSortFields.Name:
                    return x => x.Name;
                case RoleSortFields.Description:
                    return x => x.Description;
                case RoleSortFields.ClientName:
                    return x => x.ClientName;
                default:
                    return x => x.Id;
            }
        }
    }
}
