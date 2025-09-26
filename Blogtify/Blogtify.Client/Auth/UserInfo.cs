using System.Security.Claims;

namespace PlayVerse.Web.Client.Auth;

public sealed class UserInfo
{
    public required string UserId { get; init; }
    //public required string Name { get; init; }
    public required string[] Roles { get; init; }

    public const string UserIdClaimType = "sub";
    //public const string NameClaimType = "name";
    public const string RoleClaimType = "role";

    public static UserInfo FromClaimsPrincipal(ClaimsPrincipal principal) =>
      new()
      {
          UserId = GetRequiredClaim(principal, UserIdClaimType),
          //Name = GetRequiredClaim(principal, NameClaimType),
          Roles = principal.FindAll(RoleClaimType).Select(c => c.Value)
                .ToArray()
      };

    public ClaimsPrincipal ToClaimsPrincipal() =>
      new(new ClaimsIdentity(
        [
            new(UserIdClaimType, UserId),
            //new(NameClaimType, Name),
            ..Roles.Select(role => new Claim(RoleClaimType, role))
        ],
        authenticationType: nameof(UserInfo),
        nameType: "",//NameClaimType,
        roleType: RoleClaimType));

    private static string GetRequiredClaim(ClaimsPrincipal principal, string claimType) =>
      principal.FindFirst(claimType)?.Value
      ?? throw new InvalidOperationException($"Could not find required '{claimType}' claim.");
}