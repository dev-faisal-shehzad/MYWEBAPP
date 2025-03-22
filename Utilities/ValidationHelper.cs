using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Utilities
{
    public static class ValidationHelper
    {
        public static bool ValidateEntity(object entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, context, results, true);

            if (!isValid)
            {
                Console.WriteLine($"❌ Validation failed for {entity.GetType().Name}:");
                foreach (var error in results)
                {
                    Console.WriteLine($"   🔹 {error.ErrorMessage}");
                }
            }

            return isValid;
        }
    }
}
