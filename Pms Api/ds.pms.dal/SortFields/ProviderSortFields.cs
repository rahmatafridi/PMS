using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ds.pms.dal.SortFields
{
    public enum ProviderSortFields
    {
        None = 0,
        Id = 1,
        Name = 2,
        Email = 3,
        Address1 = 4,
        Address2 = 5,
        City = 6,
        Postcode = 7,
        Date = 8,
        ClientName = 9,
    }

    public static class ProviderSortFieldsExtension
    {
        public static ProviderSortFields GetProviderField(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == ProviderSortFields.Id.ToString().ToLower())
                    return ProviderSortFields.Id;
                else if (sortBy.ToLower() == ProviderSortFields.Name.ToString().ToLower())
                    return ProviderSortFields.Name;
                else if (sortBy.ToLower() == ProviderSortFields.Email.ToString().ToLower())
                    return ProviderSortFields.Email;
                else if (sortBy.ToLower() == ProviderSortFields.Address1.ToString().ToLower())
                    return ProviderSortFields.Address1;
                else if (sortBy.ToLower() == ProviderSortFields.Address2.ToString().ToLower())
                    return ProviderSortFields.Address2;
                else if (sortBy.ToLower() == ProviderSortFields.City.ToString().ToLower())
                    return ProviderSortFields.City;
                else if (sortBy.ToLower() == ProviderSortFields.Postcode.ToString().ToLower())
                    return ProviderSortFields.Postcode;
                else if (sortBy.ToLower() == ProviderSortFields.Date.ToString().ToLower())
                    return ProviderSortFields.Date;
                else if (sortBy.ToLower() == ProviderSortFields.ClientName.ToString().ToLower())
                    return ProviderSortFields.ClientName;
            }
            return ProviderSortFields.None;
        }
        public static string GetFieldName(this ProviderSortFields sortField)
        {
            if (sortField != ProviderSortFields.None)
                return sortField.ToString();
            return string.Empty;
        }
        public static Expression<Func<ProviderList, object>> GetColumn(this ProviderSortFields sortField)
        {
            switch (sortField)
            {
                case ProviderSortFields.Id:
                    return x => x.Id;
                case ProviderSortFields.Name:
                    return x => x.Name;
                case ProviderSortFields.Email:
                    return x => x.Email;
                case ProviderSortFields.Address1:
                    return x => x.Address1;
                case ProviderSortFields.Address2:
                    return x => x.Address2;
                case ProviderSortFields.City:
                    return x => x.City;
                case ProviderSortFields.Postcode:
                    return x => x.PostCode;
                case ProviderSortFields.ClientName:
                    return x => x.ClientName;
                default:
                    return x => x.Id;
            }
        }
    }
}
