using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace AdsVenture.Core.Validation
{
    public static class Validator
    {
        public static void Validate<T>(T instance)
        {
            var entityType = typeof(T);
            if ((!ValidatorsBinding.Defaults.ContainsKey(entityType)))
            {
                throw new Exception("ValidatorsBinding does not contain a default validator associated with " + entityType.FullName);
            }
            var validatorType = ValidatorsBinding.Defaults[entityType];
            var validator = (AbstractValidator<T>)Activator.CreateInstance(validatorType);
            Validate(validator, instance);
        }

        public static void Validate<T>(AbstractValidator<T> validator, T instance)
        {
            var results = validator.Validate(instance);
            if ((results != null && results.IsValid == false))
            {
                throw new Exceptions.EntityValidationException(results);
            }
        }

    }

}
