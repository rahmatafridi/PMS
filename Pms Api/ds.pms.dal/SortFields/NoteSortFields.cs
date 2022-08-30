using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ds.pms.dal.SortFields
{
    public enum NoteSortFields
    {
        None = 0,
        Id = 1,
        Note = 2,
        Date = 3
    }

    public static class NoteSortFieldsExtension
    {
        public static NoteSortFields GetNoteField(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == NoteSortFields.Id.ToString().ToLower())
                    return NoteSortFields.Id;
                else if (sortBy.ToLower() == NoteSortFields.Note.ToString().ToLower())
                    return NoteSortFields.Note;
                else if (sortBy.ToLower() == NoteSortFields.Date.ToString().ToLower())
                    return NoteSortFields.Date;
            }
            return NoteSortFields.None;
        }
        public static string GetFieldName(this NoteSortFields sortField)
        {
            if (sortField != NoteSortFields.None)
                return sortField.ToString();
            return string.Empty;
        }
        public static Expression<Func<TblNote, object>> GetColumn(this NoteSortFields sortField)
        {
            switch (sortField)
            {
                case NoteSortFields.Id:
                    return x => x.Id;
                case NoteSortFields.Note:
                    return x => x.Note;
                case NoteSortFields.Date:
                    return x => x.AddedDate;
                default:
                    return x => x.Id;
            }
        }
    }
}
