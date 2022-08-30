using ds.pms.dal.Models;
using System;
using System.Linq.Expressions;

namespace ds.pms.dal.SortFields
{
    public enum ClientSortFields
    {
        None = 0,
        Id = 1,
        Name = 2,
        Email = 3,
        Address1 = 4,
        Address2 = 5,
        City = 6,
        Postcode = 7,
        Date = 8
    }

    public static class ClientSortFieldsExtension
    {
        public static ClientSortFields GetClientField(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == ClientSortFields.Id.ToString().ToLower())
                    return ClientSortFields.Id;
                else if (sortBy.ToLower() == ClientSortFields.Name.ToString().ToLower())
                    return ClientSortFields.Name;
                else if (sortBy.ToLower() == ClientSortFields.Email.ToString().ToLower())
                    return ClientSortFields.Email;
                else if (sortBy.ToLower() == ClientSortFields.Address1.ToString().ToLower())
                    return ClientSortFields.Address1;
                else if (sortBy.ToLower() == ClientSortFields.Address2.ToString().ToLower())
                    return ClientSortFields.Address2;
                else if (sortBy.ToLower() == ClientSortFields.City.ToString().ToLower())
                    return ClientSortFields.City;
                else if (sortBy.ToLower() == ClientSortFields.Postcode.ToString().ToLower())
                    return ClientSortFields.Postcode;
                else if (sortBy.ToLower() == ClientSortFields.Date.ToString().ToLower())
                    return ClientSortFields.Date;
            }
            return ClientSortFields.None;
        }
        public static string GetFieldName(this ClientSortFields sortField)
        {
            if (sortField != ClientSortFields.None)
                return sortField.ToString();
            return string.Empty;
        }
        public static Expression<Func<TblClient, object>> GetColumn(this ClientSortFields sortField)
        {
            switch (sortField)
            {
                case ClientSortFields.Id:
                    return x => x.Id;
                case ClientSortFields.Name:
                    return x => x.Name;
                case ClientSortFields.Email:
                    return x => x.Email;
                case ClientSortFields.Address1:
                    return x => x.Address1;
                case ClientSortFields.Address2:
                    return x => x.Address2;
                case ClientSortFields.City:
                    return x => x.City;
                case ClientSortFields.Postcode:
                    return x => x.PostCode;
                case ClientSortFields.Date:
                    return x => x.AddedDate;
                default:
                    return x => x.Id;
            }
        }
    }
}
