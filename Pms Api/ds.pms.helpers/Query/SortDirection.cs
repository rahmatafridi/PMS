using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.helpers.Query
{
    public static class SortDirection
    {
        public static string[] SortDir(string sort)
        {
            string sortBy = string.Empty, sortDirection = string.Empty;

            string[] sReturn = new string[2];
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.ToLower().Contains("asc") || sort.ToLower().Contains("ascending"))
                {
                    sortDirection = "asc";
                    sortBy = sort.ToLower().Replace("ascending", "").Replace("asc", "").Trim(' ');
                }
                else if (sort.ToLower().Contains("desc") || sort.ToLower().Contains("descending"))
                {
                    sortDirection = "desc";
                    sortBy = sort.ToLower().Replace("descending", "").Replace("desc", "").Trim(' ');
                }
                sReturn[0] = sortBy;
                sReturn[1] = sortDirection;
            }
            return sReturn;
        }
    }
}
