using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AdsVenture.Commons.Pagination
{
    [DataContract()]
    public class PageData<T> : IEnumerable<T>
    {

        public IList<T> Data { get; set; }

        public long TotalRecords { get; set; }

        public int CurrentPage { get; set; }


        public PageData(List<T> data, long totalRecords, int currentPage)
        {
            this.Data = data;
            this.TotalRecords = totalRecords;
            this.CurrentPage = currentPage;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Data.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public PageData<T2> Convert<T2>(Func<T, T2> conversion)
        {
            return new PageData<T2>(this.Data.Select(d => conversion(d)).ToList(), this.TotalRecords, this.CurrentPage);
        }
    }
}
