using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.apicommon.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }
        public long TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
