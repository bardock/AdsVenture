using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Commons.Pagination.Interfaces
{
    public interface IPageParams
    {
        int Offset { get; set; }
        int Limit { get; set; }
    }
}
