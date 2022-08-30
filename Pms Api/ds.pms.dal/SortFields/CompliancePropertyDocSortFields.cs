using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ds.pms.dal.SortFields
{
    public enum CompliancePropertyDocSortFields
    {
        None = 0,
        Id = 1,
        Name = 2,
        Date = 3
    }

    public static class CompliancePropertyDocSortFieldsExtension
    {
        public static CompliancePropertyDocSortFields GetCompliancePropertyDocField(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == CompliancePropertyDocSortFields.Id.ToString().ToLower())
                    return CompliancePropertyDocSortFields.Id;
                else if (sortBy.ToLower() == CompliancePropertyDocSortFields.Name.ToString().ToLower())
                    return CompliancePropertyDocSortFields.Name;
                else if (sortBy.ToLower() == CompliancePropertyDocSortFields.Date.ToString().ToLower())
                    return CompliancePropertyDocSortFields.Date;
            }
            return CompliancePropertyDocSortFields.None;
        }
        public static string GetFieldName(this CompliancePropertyDocSortFields sortField)
        {
            if (sortField != CompliancePropertyDocSortFields.None)
                return sortField.ToString();
            return string.Empty;
        }
        public static Expression<Func<PropertyCompianceDocsList, object>> GetColumn(this CompliancePropertyDocSortFields sortField)
        {
            switch (sortField)
            {
                case CompliancePropertyDocSortFields.Id:
                    return x => x.Id;
                case CompliancePropertyDocSortFields.Name:
                    return x => x.Name;
                case CompliancePropertyDocSortFields.Date:
                    return x => x.UploadedDate;
                default:
                    return x => x.Id;
            }
        }
    }
}
