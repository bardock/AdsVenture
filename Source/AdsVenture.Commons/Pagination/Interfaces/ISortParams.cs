using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Commons.Pagination.Interfaces
{
    public enum SortDirs
    {
        Asc = 0,
        Desc = 1
    }

    public interface ISortParams
    {

        string[] SortBy { get; set; }

        SortDirs[] SortDir { get; set; }
    }
}
