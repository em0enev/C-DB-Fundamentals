using BillsPaymentSystem.Data;
using BillsPaymentSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BillsPaymentSystem.App
{
    public static class UserValidator
    {
        public static bool IsValidUser(object entity)
        {
            ValidationContext validationContext = new ValidationContext(entity);

            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true);

            return isValid;
        }

        public static bool ChekForDublicateInDb(User user, BillsPaymentSystemContext context)
        {
           bool isEmailExist = context
                .Users
                .Any(u => u.Email == user.Email);

            bool isUsernameExist = context
                .Users
                .Any(u => u.Username == user.Username);

            if (isEmailExist == false && 
                isUsernameExist == false)
            {
                return true;
            }

            return false;
        }
    }
}
