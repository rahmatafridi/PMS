using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ds.pms.dal.SortFields
{
    public enum TenantSortFields
    {
        None = 0,
        Id = 1,
        FirstName = 2,
        LastName = 3
    }

    public static class TenantSortFieldsExtension
    {
        public static TenantSortFields GetTenantField(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == TenantSortFields.Id.ToString().ToLower())
                    return TenantSortFields.Id;
                else if (sortBy.ToLower() == TenantSortFields.FirstName.ToString().ToLower())
                    return TenantSortFields.FirstName;
                else if (sortBy.ToLower() == TenantSortFields.LastName.ToString().ToLower())
                    return TenantSortFields.LastName;
            }
            return TenantSortFields.None;
        }
        public static string GetFieldName(this TenantSortFields sortField)
        {
            if (sortField != TenantSortFields.None)
                return sortField.ToString();
            return string.Empty;
        }
        public static Expression<Func<TblTenant, object>> GetColumn(this TenantSortFields sortField)
        {
            switch (sortField)
            {
                case TenantSortFields.Id:
                    return x => x.Id;
                case TenantSortFields.FirstName:
                    return x => x.FirstName;
                case TenantSortFields.LastName:
                    return x => x.LastName;
                default:
                    return x => x.Id;
            }
        }
    }
}
