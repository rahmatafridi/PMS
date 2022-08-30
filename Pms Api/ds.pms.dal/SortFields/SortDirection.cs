namespace ds.pms.dal.SortFields
{
    public enum SortDirection
    {
        None = 0,
        Asc = 1,
        Ascending = 2,
        Desc = 3,
        Descending = 4
    }
    public static class SortDirectionExtension
    {
        public static SortDirection GetSortDirectionByName(this string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == SortDirection.Ascending.ToString().ToLower() ||
                    sortBy.ToLower() == SortDirection.Asc.ToString().ToLower())
                    return SortDirection.Asc;
                else if (sortBy.ToLower() == SortDirection.Descending.ToString().ToLower() ||
                    sortBy.ToLower() == SortDirection.Desc.ToString().ToLower())
                    return SortDirection.Desc;
            }
            return SortDirection.None;
        }
    }
}
