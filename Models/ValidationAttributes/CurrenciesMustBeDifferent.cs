using Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ValidationAttributes
{
    public class CurrenciesMustBeDifferent : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
          ValidationContext validationContext)
        {
            var history = (HistoryForCreationDto)validationContext.ObjectInstance;

            if (history.OriginCode == history.DestinationCode)
            {
                return new ValidationResult(ErrorMessage,
                    new[] { nameof(HistoryForCreationDto) });
            }

            return ValidationResult.Success;
        }
    }
}
