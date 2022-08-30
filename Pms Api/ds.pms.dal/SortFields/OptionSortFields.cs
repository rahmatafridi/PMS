using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ds.pms.dal.SortFields
{
    public enum OptionSortFields
    {
        None = 0,
        Id = 1,
        Name = 2,
        Date = 3
    }

    public static class OptionSortFieldsExtension
    {
        public static OptionSortFields OptionSortField(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == NotesCategorySortFields.Id.ToString().ToLower())
                    return OptionSortFields.Id;
                else if (sortBy.ToLower() == OptionSortFields.Name.ToString().ToLower())
                    return OptionSortFields.Name;
                else if (sortBy.ToLower() == OptionSortFields.Date.ToString().ToLower())
                    return OptionSortFields.Date;
            }
            return OptionSortFields.None;
        }
        public static string GetFieldName(this OptionSortFields sortField)
        {
            if (sortField != OptionSortFields.None)
                return sortField.ToString();
            return string.Empty;
        }
        public static Expression<Func<TblOption, object>> GetColumn(this OptionSortFields sortField)
        {
            switch (sortField)
            {
                case OptionSortFields.Id:
                    return x => x.Id;
                case OptionSortFields.Name:
                    return x => x.Title;
                default:
                    return x => x.Id;
            }
        }
    }
}
