using ds.pms.dal.CustomModels;
using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ds.pms.dal.SortFields
{
    public enum PropertySortFields
    {
        None = 0,
        Id = 1,
        Address1 = 2,
        Address2 = 3,
        City = 4,
        Postcode = 5,
        NumberofRooms = 6,
        ProviderName = 7,
        ClientName = 8,
    }

    public static class PropertySortFieldsExtension
    {
        public static PropertySortFields GetPropertyField(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == PropertySortFields.Id.ToString().ToLower())
                    return PropertySortFields.Id;
                else if (sortBy.ToLower() == PropertySortFields.Address1.ToString().ToLower())
                    return PropertySortFields.Address1;
                else if (sortBy.ToLower() == PropertySortFields.Address2.ToString().ToLower())
                    return PropertySortFields.Address2;
                else if (sortBy.ToLower() == PropertySortFields.City.ToString().ToLower())
                    return PropertySortFields.City;
                else if (sortBy.ToLower() == PropertySortFields.Postcode.ToString().ToLower())
                    return PropertySortFields.Postcode;
                else if (sortBy.ToLower() == PropertySortFields.NumberofRooms.ToString().ToLower())
                    return PropertySortFields.NumberofRooms;
                else if (sortBy.ToLower() == PropertySortFields.ProviderName.ToString().ToLower())
                    return PropertySortFields.ProviderName;
                else if (sortBy.ToLower() == PropertySortFields.ClientName.ToString().ToLower())
                    return PropertySortFields.ClientName;
            }
            return PropertySortFields.None;
        }
        public static string GetFieldName(this PropertySortFields sortField)
        {
            if (sortField != PropertySortFields.None)
                return sortField.ToString();
            return string.Empty;
        }
        public static Expression<Func<PropertyList, object>> GetColumn(this PropertySortFields sortField)
        {
            switch (sortField)
            {
                case PropertySortFields.Id:
                    return x => x.Id;
                case PropertySortFields.Address1:
                    return x => x.Address1;
                case PropertySortFields.Address2:
                    return x => x.Address2;
                case PropertySortFields.City:
                    return x => x.City;
                case PropertySortFields.Postcode:
                    return x => x.PostCode;
                case PropertySortFields.NumberofRooms:
                    return x => x.NumberOfRooms;
                case PropertySortFields.ProviderName:
                    return x => x.ProviderName;
                case PropertySortFields.ClientName:
                    return x => x.ClientName;
                default:
                    return x => x.Id;
            }
        }
    }
}
