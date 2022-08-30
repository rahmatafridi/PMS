using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ds.pms.dal.SortFields
{
    public enum ConfigSortFields
    {
        None = 0,
        Id = 1,
        Key = 2,
        Value = 3,
        Description = 4,
        Date = 5
    }

    public static class ConfigSortFieldsExtension
    {
        public static ConfigSortFields GetConfigField(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == ConfigSortFields.Id.ToString().ToLower())
                    return ConfigSortFields.Id;
                else if (sortBy.ToLower() == ConfigSortFields.Key.ToString().ToLower())
                    return ConfigSortFields.Key;
                else if (sortBy.ToLower() == ConfigSortFields.Value.ToString().ToLower())
                    return ConfigSortFields.Value;
                else if (sortBy.ToLower() == ConfigSortFields.Description.ToString().ToLower())
                    return ConfigSortFields.Description;
                else if (sortBy.ToLower() == ConfigSortFields.Date.ToString().ToLower())
                    return ConfigSortFields.Date;
            }
            return ConfigSortFields.None;
        }
        public static string GetFieldName(this ConfigSortFields sortField)
        {
            if (sortField != ConfigSortFields.None)
                return sortField.ToString();
            return string.Empty;
        }
        public static Expression<Func<TblConfig, object>> GetColumn(this ConfigSortFields sortField)
        {
            switch (sortField)
            {
                case ConfigSortFields.Id:
                    return x => x.Id;
                case ConfigSortFields.Key:
                    return x => x.Key;
                case ConfigSortFields.Value:
                    return x => x.Value;
                case ConfigSortFields.Description:
                    return x => x.Description;
                case ConfigSortFields.Date:
                    return x => x.AddedDate;
                default:
                    return x => x.Id;
            }
        }
    }
}
