using ds.pms.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ds.pms.dal.SortFields
{
    public enum RoomSortFields
    {
        None = 0,
        Id = 1,
        RoomName = 2,
        Date = 3
    }

    public static class RoomSortFieldsExtension
    {
        public static RoomSortFields GetRoomField(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == RoomSortFields.Id.ToString().ToLower())
                    return RoomSortFields.Id;
                else if (sortBy.ToLower() == RoomSortFields.RoomName.ToString().ToLower())
                    return RoomSortFields.RoomName;
                else if (sortBy.ToLower() == RoomSortFields.Date.ToString().ToLower())
                    return RoomSortFields.Date;
            }
            return RoomSortFields.None;
        }
        public static string GetFieldName(this RoomSortFields sortField)
        {
            if (sortField != RoomSortFields.None)
                return sortField.ToString();
            return string.Empty;
        }
        public static Expression<Func<TblRoom, object>> GetColumn(this RoomSortFields sortField)
        {
            switch (sortField)
            {
                case RoomSortFields.Id:
                    return x => x.Id;
                case RoomSortFields.RoomName:
                    return x => x.RoomName;
                case RoomSortFields.Date:
                    return x => x.AddedDate;
                default:
                    return x => x.Id;
            }
        }
    }
}
