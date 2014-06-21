using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using System.Linq.Expressions;

namespace AdsVenture.Core.Exceptions
{

    public class EntityValidationException : InvalidEntityStateException
    {

        protected FluentValidation.Results.ValidationResult ValidationResult { get; set; }
        public IList<FailureData> Errors { get; set; }

        public EntityValidationException(FluentValidation.Results.ValidationResult validationResult)
            : base(Core.Resources.BusinessExceptions.EntityValidationException)
        {
            this.ValidationResult = validationResult;
            this.Errors = Mapper.Map<IList<FailureData>>(validationResult.Errors);
        }

        public class FailureData
        {
            public object AttemptedValue { get; set; }
            public string ErrorMessage { get; set; }
            public string PropertyName { get; set; }
        }

    }

}
