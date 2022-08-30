using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ds.pms.dal.SortFields
{
    public enum DocumentSortFields
    {
        None = 0,
        Id = 1,
        Name = 2,
        Date = 3
    }

    public static class DocumentSortFieldsExtension
    {
        public static DocumentSortFields GetDocumentField(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == DocumentSortFields.Id.ToString().ToLower())
                    return DocumentSortFields.Id;
                else if (sortBy.ToLower() == DocumentSortFields.Name.ToString().ToLower())
                    return DocumentSortFields.Name;
                else if (sortBy.ToLower() == DocumentSortFields.Date.ToString().ToLower())
                    return DocumentSortFields.Date;
            }
            return DocumentSortFields.None;
        }
        public static string GetFieldName(this DocumentSortFields sortField)
        {
            if (sortField != DocumentSortFields.None)
                return sortField.ToString();
            return string.Empty;
        }
        public static Expression<Func<TblDocument, object>> GetColumn(this DocumentSortFields sortField)
        {
            switch (sortField)
            {
                case DocumentSortFields.Id:
                    return x => x.Id;
                case DocumentSortFields.Name:
                    return x => x.Name;
                case DocumentSortFields.Date:
                    return x => x.AddedDate;
                default:
                    return x => x.Id;
            }
        }
    }
}
