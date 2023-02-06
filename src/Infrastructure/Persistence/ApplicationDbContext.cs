using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Options;
using RecipesDotNet.Infrastructure.Identity;

namespace RecipesDotNet.Infrastructure.Persistence;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
    }
}
