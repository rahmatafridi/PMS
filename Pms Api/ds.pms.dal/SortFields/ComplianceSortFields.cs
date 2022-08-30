using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ds.pms.dal.SortFields
{
    public enum ComplianceSortFields
    {
        None = 0,
        Id = 1,
        Title = 2,
        Date = 3
    }

    public static class ComplianceSortFieldsExtension
    {
        public static ComplianceSortFields GetComplianceField(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == ComplianceSortFields.Id.ToString().ToLower())
                    return ComplianceSortFields.Id;
                else if (sortBy.ToLower() == ComplianceSortFields.Title.ToString().ToLower())
                    return ComplianceSortFields.Title;
                else if (sortBy.ToLower() == ComplianceSortFields.Date.ToString().ToLower())
                    return ComplianceSortFields.Date;
            }
            return ComplianceSortFields.None;
        }
        public static string GetFieldName(this ComplianceSortFields sortField)
        {
            if (sortField != ComplianceSortFields.None)
                return sortField.ToString();
            return string.Empty;
        }
        public static Expression<Func<TblCompliance, object>> GetColumn(this ComplianceSortFields sortField)
        {
            switch (sortField)
            {
                case ComplianceSortFields.Id:
                    return x => x.Id;
                case ComplianceSortFields.Title:
                    return x => x.Title;
                case ComplianceSortFields.Date:
                    return x => x.AddedDate;
                default:
                    return x => x.Id;
            }
        }
    }
}
