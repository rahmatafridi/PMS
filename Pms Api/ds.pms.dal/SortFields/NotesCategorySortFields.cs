using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ds.pms.dal.SortFields
{
    public enum NotesCategorySortFields
    {
        None = 0,
        Id = 1,
        Name = 2,
        Date = 3
    }

    public static class NotesCategorySortFieldsExtension
    {
        public static NotesCategorySortFields GetNotesCategoryField(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == NotesCategorySortFields.Id.ToString().ToLower())
                    return NotesCategorySortFields.Id;
                else if (sortBy.ToLower() == NotesCategorySortFields.Name.ToString().ToLower())
                    return NotesCategorySortFields.Name;
                else if (sortBy.ToLower() == NotesCategorySortFields.Date.ToString().ToLower())
                    return NotesCategorySortFields.Date;
            }
            return NotesCategorySortFields.None;
        }
        public static string GetFieldName(this NotesCategorySortFields sortField)
        {
            if (sortField != NotesCategorySortFields.None)
                return sortField.ToString();
            return string.Empty;
        }
        public static Expression<Func<TblNotesCategory, object>> GetColumn(this NotesCategorySortFields sortField)
        {
            switch (sortField)
            {
                case NotesCategorySortFields.Id:
                    return x => x.Id;
                case NotesCategorySortFields.Name:
                    return x => x.Name;
                case NotesCategorySortFields.Date:
                    return x => x.AddedDate;
                default:
                    return x => x.Id;
            }
        }
    }
}
