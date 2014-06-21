using AdsVenture.Commons.Pagination.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Commons.Pagination
{
    public class PageParams : IPageParams, ISortParams
    {
        public int Offset { get; set; }
        public int Limit { get; set; }

        public string[] SortBy { get; set; }
        public SortDirection[] SortDir{ get; set; }
    }
}
