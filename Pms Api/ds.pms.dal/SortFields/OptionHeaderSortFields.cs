using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ds.pms.dal.SortFields
{
 
    namespace ds.pms.dal.SortFields
    {
        public enum OptionHeaderSortFields
        {
            None = 0,
            Id = 1,
            Name = 2,
            Date = 3
        }

        public static class OptionHeaderSortFieldsExtension
        {
            public static OptionHeaderSortFields OptionHeaderSortField(this string sortBy)
            {
                if (!string.IsNullOrEmpty(sortBy))
                {
                    if (sortBy.ToLower() == NotesCategorySortFields.Id.ToString().ToLower())
                        return OptionHeaderSortFields.Id;
                    else if (sortBy.ToLower() == OptionHeaderSortFields.Name.ToString().ToLower())
                        return OptionHeaderSortFields.Name;
                    else if (sortBy.ToLower() == OptionHeaderSortFields.Date.ToString().ToLower())
                        return OptionHeaderSortFields.Date;
                }
                return OptionHeaderSortFields.None;
            }
            public static string GetFieldName(this OptionHeaderSortFields sortField)
            {
                if (sortField != OptionHeaderSortFields.None)
                    return sortField.ToString();
                return string.Empty;
            }
            public static Expression<Func<TblOptionHeader, object>> GetColumn(this OptionHeaderSortFields sortField)
            {
                switch (sortField)
                {
                    case OptionHeaderSortFields.Id:
                        return x => x.Id;
                    case OptionHeaderSortFields.Name:
                        return x => x.Title;
                    default:
                        return x => x.Id;
                }
            }
        }
    }

}
