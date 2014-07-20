using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Commons.Pagination.Interfaces
{
    public interface IPageParams
    {
        int StartIndex { get; set; }

        int Length { get; set; }
    }
}
