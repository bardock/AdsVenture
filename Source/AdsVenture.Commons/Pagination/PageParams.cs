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
        public int StartIndex { get; set; }
        public int Length { get; set; }
        public string[] SortBy { get; set; }
        public SortDirs[] SortDir { get; set; }

        private string _search = null;
        public string Search
        {
            get { return _search; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _search = null;
                }
                else
                {
                    _search = value.ToLower();
                }
            }
        }

    }
}
