using AdsVenture.Commons.Pagination.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Commons.Pagination
{
    public class PageData<T>
    {
        public IEnumerable<T> Results { get; set; }
        public long Total { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }

        public PageData(IEnumerable<T> results, long total, int offset, int limit)
        {
            this.Results = results;
            this.Total = total;
            this.Offset = offset;
            this.Limit = limit;
        }
    }
}
