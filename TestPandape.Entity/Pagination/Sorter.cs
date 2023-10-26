using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPandape.Entity.Pagination
{
    public class Sorter
    {
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public Sorter()
        {
            SortBy = "id";
            SortOrder = "asc";
        }
        public Sorter(string _sortBy, string _sortOrder)
        {
            SortBy = _sortBy;
            SortOrder = _sortOrder;
        }
    }
}
