using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Internal;
using AdsVenture.Commons;
using AdsVenture.Core.Validation.Entities;
using AdsVenture.Commons.Entities;

namespace AdsVenture.Core.Validation
{

    public class ValidatorsBinding
    {

        /// <summary>
        /// Add default validator for entities that need validation at presentation layer
        /// </summary>
        public static Dictionary<Type, Type> Defaults
        {
            get { 
                return new Dictionary<Type, Type> 
                {  
                    {typeof(User), typeof(UserValidator)},
                }; 
            }
        }

        /// <summary>
        /// Returns validator type defined at ValidatorsBinding.Defaults Property
        /// </summary>
        /// <remarks></remarks>
        public class ValidatorFactory : IValidatorFactory
        {
            readonly InstanceCache cache = new InstanceCache();
            /// <summary>
            /// Gets a validator for the appropriate type.
            /// </summary>
            public IValidator<T> GetValidator<T>()
            {
                return (IValidator<T>)GetValidator(typeof(T));
            }

            /// <summary>
            /// Gets a validator for the appropriate type.
            /// </summary>
            public virtual IValidator GetValidator(Type type)
            {
                if ((!ValidatorsBinding.Defaults.ContainsKey(type)))
                {
                    return null;
                }
                return cache.GetOrCreateInstance(ValidatorsBinding.Defaults[type]) as IValidator;
            }
        }

    }

}
