using AdsVenture.Commons.Entities;
using AdsVenture.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AdsVenture.Core.Managers
{
    public class ContentManager : _BaseEntityManager<Content>
    {
        public ContentManager(
            Helpers.IUnitOfWork unitOfWork)
            : base(unitOfWork) 
        {
        }

        public Content FindForSlot(Guid slotID)
        {
            var slot = Db.Slots
                .AsNoTracking()
                .Include(x => x.Content)
                .FirstOrDefault(x => x.ID == slotID);

            if (slot.ContentID == null)
                throw new NotImplementedException("Slot has not a content");

            return slot.Content;
        }

        public string Render(Content e)
        {
            return string.Format("<iframe src=\"{0}\"></iframe>", e.Url);
        }
    }
}
