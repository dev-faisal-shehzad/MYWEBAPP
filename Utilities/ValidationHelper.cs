using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyWebApp.Utilities
{
    public static class ValidationHelper
    {
        public static List<ValidationResult> ValidateEntity(object entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();

            // Perform validation
            bool isValid = Validator.TryValidateObject(entity, context, results, true);

            if (!isValid)
            {
                Console.WriteLine($"❌ Validation failed for {entity.GetType().Name}:");
                Console.WriteLine($"❌ Errors : {results.Count}");
                foreach (var error in results)
                {
                    Console.WriteLine($"   🔹 {error.MemberNames.FirstOrDefault()} - Error: {error.ErrorMessage}");
                }
            }

            return results;
        }
    }
}
