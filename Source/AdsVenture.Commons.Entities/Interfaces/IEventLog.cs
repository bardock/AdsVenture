using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Commons.Entities.Interfaces
{
    public interface IEventLog : IEntity
    {
        System.DateTime EventDate { get; set; }
        string FieldName { get; set; }
        string FieldValueBefore { get; set; }
        string FieldValueAfter { get; set; }
        object Entity { set; }
    }
}
