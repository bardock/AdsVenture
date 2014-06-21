using AdsVenture.Commons.Entities.Resources;
using System.ComponentModel;

namespace AdsVenture.Commons.Entities.DataAnotations
{
    public class EntityDisplayNameAttribute : DisplayNameAttribute
    {
        private string resourceId;
        public EntityDisplayNameAttribute(string resourceId)
        {
            this.resourceId = resourceId;
        }
        public override string DisplayName
        {
            get { return Helper.GetEntityResource(this.resourceId); }
        }
    }
}
