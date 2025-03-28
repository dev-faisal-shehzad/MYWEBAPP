using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MyWebApp.Models;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Utilities; // Include the validation helper

namespace MyWebApp.Data.Seed
{
    public static class Seed
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Ensure database is created and migrated before seeding
                context.Database.Migrate();
                Console.WriteLine("✅ Database migrated successfully.");

                // Step 1: Create Users
                var admin = EnsureUser(context, "Admin", "Admin-", "admingmail.com", UserRole.Admin, true);
                var alice = EnsureUser(context, "", "", "aliceexample.com", UserRole.Admin);
                var bob = EnsureUser(context, "Bob", "Johnson", "bob@example.com", UserRole.User);
                var charlie = EnsureUser(context, "Charlie", "Brown", "charlie@example.com", UserRole.User);

                // Step 2: Create Posts (Only after users exist)
                if (alice != null) EnsurePost(context, alice, "Alice's Post");
                
                if (bob != null) EnsurePost(context, bob, "Bob's Post");
                
                if (charlie != null) EnsurePost(context, charlie, "Charlie's Post");

                // Step 3: Perform Deletions (After everything else)
                if (bob != null) DeleteUser(context, bob, hardDelete: false);
                
                if (charlie != null) DeleteUser(context, charlie, hardDelete: true);

                if (admin != null)
                {
                    admin.LastName = "Admin"; // Update the last name
                    context.SaveChanges(); // Save changes to the database
                }

                Console.WriteLine("✅ Seeding process completed.");
            }
        }

        private static User EnsureUser(ApplicationDbContext context, string firstName, string lastName, string email, UserRole role, bool BypassValidation = false)
        {
            try
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    user = new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        PasswordHash = "123456",
                        Role = role,
                        Status = UserStatus.Active,
                        Gender = UserGender.Male,
                        BypassValidation = BypassValidation
                    };

                    // Validate before adding
                    var validationResults = ValidationHelper.ValidateEntity(user);
                    if (validationResults.Count > 0) return null;

                    context.Users.Add(user);
                    context.SaveChanges();
                }
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: Failed to create user {email}. Reason: {ex.Message}");
                return null;
            }
        }

        private static void EnsurePost(ApplicationDbContext context, User author, string postName)
        {
            try
            {
                var postCount = context.Posts.Count(p => p.AuthorId == author.Id);
                var post = new Post
                {
                    Name = $"{postName} {postCount + 1}",
                    Description = "This is a test post.",
                    AuthorId = author.Id
                };

                // Validate before adding
                var validationResults = ValidationHelper.ValidateEntity(post);
                if (validationResults.Count > 0) return;

                context.Posts.Add(post);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: Failed to create post for {author.Email}. Reason: {ex.Message}");
            }
        }

        private static void DeleteUser(ApplicationDbContext context, User user, bool hardDelete)
        {
            try
            {
                if (hardDelete)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
                else
                {
                    context.ApplySoftDelete(user);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: Failed to delete user {user.Email}. Reason: {ex.Message}");
            }
        }
    }
}
