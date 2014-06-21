using AdsVenture.Commons.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Core.Validation.Entities
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email)
                .Length(0,255)
                .EmailAddress();
        }
    }
}
