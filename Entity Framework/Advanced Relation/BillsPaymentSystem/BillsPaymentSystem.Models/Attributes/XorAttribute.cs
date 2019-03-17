using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BillsPaymentSystem.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class XorAttribute : ValidationAttribute
    {
        private string targetProperty;

        public XorAttribute(string targetPropery)
        {
            this.targetProperty = targetPropery;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var targetPropertyValue = validationContext
                .ObjectType
                .GetProperty(this.targetProperty)
                .GetValue(validationContext.ObjectInstance);

            if (value == null && targetPropertyValue == null ||
                value != null && targetPropertyValue != null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("The Two properties must have opposite values!");
        }
    }
}
