using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipesDotNet.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "varchar(max)")]
        public string ImageUrl { get; set; } = string.Empty;

        [Column(TypeName = "varchar(200)")]
        public string FirstName { get; set; } = string.Empty;

        [Column(TypeName = "varchar(200)")]
        public string LastName { get; set; } = string.Empty;
    }
}