using ds.pms.dal.Models;
using System;
using System.Linq.Expressions;

namespace ds.pms.dal.SortFields
{
    public enum UserSortFields
    {
        None = 0,
        Id = 1,
        UserName = 2,
        Email = 3,
        DisplayName = 4,
        Date = 5
    }

    public static class UserSortFieldsExtension
    {
        public static UserSortFields GetUserField(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == UserSortFields.Id.ToString().ToLower())
                    return UserSortFields.Id;
                else if (sortBy.ToLower() == UserSortFields.UserName.ToString().ToLower())
                    return UserSortFields.UserName;
                else if (sortBy.ToLower() == UserSortFields.Email.ToString().ToLower())
                    return UserSortFields.Email;
                else if (sortBy.ToLower() == UserSortFields.DisplayName.ToString().ToLower())
                    return UserSortFields.DisplayName;
                else if (sortBy.ToLower() == UserSortFields.Date.ToString().ToLower())
                    return UserSortFields.Date;
            }
            return UserSortFields.None;
        }
        public static string GetFieldName(this UserSortFields sortField)
        {
            if (sortField != UserSortFields.None)
                return sortField.ToString();
            return string.Empty;
        }
        public static Expression<Func<TblUser, object>> GetColumn(this UserSortFields sortField)
        {
            switch (sortField)
            {
                case UserSortFields.Id:
                    return x => x.Id;
                case UserSortFields.UserName:
                    return x => x.Username;
                case UserSortFields.Email:
                    return x => x.Email;
                case UserSortFields.DisplayName:
                    return x => x.Displayname;
                case UserSortFields.Date:
                    return x => x.AddedDate;
                default:
                    return x => x.Id;
            }
        }
    }
}
