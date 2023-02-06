using IdentityModel;
using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;

using static RecipesDotNet.Shared.Infrastructure.Authentication;

#nullable disable

namespace RecipesDotNet.Shared.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets the <see cref="ClaimsPrincipal"/>'s Id.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>The <see cref="ClaimsPrincipal"/>'s Id.</returns>
        public static string UserId(this ClaimsPrincipal principal)
            => principal.GetUserId<string>();

        /// <summary>
        /// Get's the <see cref="ClaimsPrincipal"/>'s image claim value.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>The <see cref="ClaimsPrincipal"/>'s image claim value.</returns>
        public static string Image(this ClaimsPrincipal principal)
            => principal.FindFirst(CustomClaimTypes.Image)?.Value;

        /// <summary>
        /// Get's the <see cref="ClaimsPrincipal"/>'s first name claim value.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>The <see cref="ClaimsPrincipal"/>'s first name claim value.</returns>
        public static string FirstName(this ClaimsPrincipal principal)
            => principal.FindFirst(CustomClaimTypes.FirstName)?.Value;

        /// <summary>
        /// Get's the <see cref="ClaimsPrincipal"/>'s email claim value.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>The <see cref="ClaimsPrincipal"/>'s email claim value.</returns>
        public static string Email(this ClaimsPrincipal principal)
            => principal.FindFirst(JwtClaimTypes.Name)?.Value;

        /// <summary>
        /// Get's the <see cref="ClaimsPrincipal"/>'s username claim value.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>The <see cref="ClaimsPrincipal"/>'s username claim value.</returns>
        public static string UserName(this ClaimsPrincipal principal)
            => principal.FindFirst(JwtClaimTypes.PreferredUserName)?.Value;

        /// <summary>
        /// Gets the <see cref="ClaimsPrincipal"/>'s Id and converts it to the type <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>
        /// Supported types are <see cref="string"/>, <see cref="int"/>, <see cref="long"/>, and <see cref="Guid"/>.
        /// </remarks>
        /// <param name="principal"></param>
        /// <returns>The <see cref="ClaimsPrincipal"/>'s Id converted to the specified type.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static T GetUserId<T>(this ClaimsPrincipal principal)
        {
            if (principal?.Identity is null)
                throw new ArgumentNullException(nameof(principal));

            if (!principal.Identity.IsAuthenticated)
                throw new ArgumentException("Principal is not authenticated.", nameof(principal));

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
                throw new Exception($"`{ClaimTypes.NameIdentifier}` claim not found for user: {principal.Identity.Name}");

            if (typeof(T) == typeof(string) ||
                typeof(T) == typeof(int) ||
                typeof(T) == typeof(long) ||
                typeof(T) == typeof(Guid))
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));

                try
                {
                    return (T)converter.ConvertFromInvariantString(userId);
                }
                catch (NotSupportedException)
                {
                    throw new InvalidOperationException($"Could not convert user Id to type {typeof(T)}");
                }
            }

            throw new NotSupportedException($"Unable to convert user Id to type {typeof(T)}. Supported types are `string`, `int`, `long`, and `Guid`");
        }

        /// <summary>
        /// Attempts to get the <see cref="ClaimsPrincipal"/>'s Id and convert it to the type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="principal">The claims principal.</param>
        /// <param name="userId">This will contain the user Id in the desired type if the conversion was successful; otherwise, the parameter remains uninitialized.</param>
        /// <returns>Returns true if the <see cref="ClaimsPrincipal"/>'s Id was able to be converted to the specified type; otherwise, false.</returns>
        public static bool TryGetUserId<T>(this ClaimsPrincipal principal, out T userId)
        {
            try
            {
                userId = principal.GetUserId<T>();
                return true;
            }
            catch (Exception)
            {
                userId = default;
                return false;
            }
        }

        /// <summary>
        /// Takes an image stream, encodes it to a base64 data URL, and adds it to the <see cref="ClaimsPrincipal"/>'s claims.
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <param name="stream"></param>
        /// <param name="imageType"></param>
        public static void AddBase64ImageClaim(this ClaimsPrincipal claimsPrincipal, Stream stream, string imageType = "jpeg")
        {
            if (stream is not null && claimsPrincipal.Identity is ClaimsIdentity identity)
            {
                // Create a memory stream and copy incoming stream into it.
                using var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);

                // Encode the binary photo data into a base64 string and add the claim to the user.
                var photoUri = $"data:image/{imageType};base64,{Convert.ToBase64String(memoryStream.GetBuffer())}";

                // Add the base64 image data to the user's 'Image" claim.
                identity.AddClaim(new Claim(CustomClaimTypes.Image, photoUri));
            }
        }
    }
}
