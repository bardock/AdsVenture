using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Core.Managers
{
    public abstract class _BaseEntityManager<TEntity> : _BaseManager
    {
	    public _BaseEntityManager(Helpers.IUnitOfWork unitOfWork)
		    : base(unitOfWork) { }

	    #region "Helper Methods"

	    /// <summary>
	    /// Checks if specified action does not throw a BusinessException
	    /// </summary>
	    protected bool Satisfies(Action f)
	    {
		    try {
			    f();
			    return true;
		    } catch (Exceptions.BusinessException) {
			    return false;
		    }
	    }

	    #endregion
    }
}
