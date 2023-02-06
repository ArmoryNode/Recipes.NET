using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RecipesDotNet.Infrastructure.Identity;
using System.Security.Claims;
using static RecipesDotNet.Shared.Infrastructure.Authentication;

namespace RecipesDotNet.Server.Infrastructure
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomClaimsPrincipalFactory`2"/> class.
        /// </summary>
        /// <param name="userManager">The <see cref="Microsoft.AspNetCore.Identity.UserManager`1"/> to retrieve user information from.</param>
        /// <param name="roleManager">The <see cref="Microsoft.AspNetCore.Identity.RoleManager`1"/> to retrieve a user's roles from.</param>
        /// <param name="options">The configured <see cref="Microsoft.AspNetCore.Identity.IdentityOptions"/>.</param>
        public CustomClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }

        protected async override Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            if (!string.IsNullOrWhiteSpace(user.ImageUrl))
            {
                var imageUrlClaim = identity.FindFirst(c => c.Type == CustomClaimTypes.Image);

                if (imageUrlClaim is not null)
                    identity.RemoveClaim(imageUrlClaim);

                identity.AddClaim(new(CustomClaimTypes.Image, user.ImageUrl));
            }

            if (!string.IsNullOrWhiteSpace(user.FirstName))
            {
                var firstNameClaim = identity.FindFirst(c => c.Type == ClaimTypes.GivenName);

                if (firstNameClaim is not null)
                    identity.RemoveClaim(firstNameClaim);

                identity.AddClaim(new(CustomClaimTypes.FirstName, user.FirstName));
            }

            return identity;
        }
    }
}
