using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BillsPaymentSystem.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExpirationDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentDateTime = DateTime.Now;

            if (currentDateTime > (DateTime)value)
            {
                return new ValidationResult("The card is expired!");
            }

            return ValidationResult.Success;
        }
    }
}
