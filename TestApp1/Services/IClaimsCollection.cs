using TestApp1.Models;

namespace TestApp1.Services;

public interface IClaimsCollection
{
    List<Claim> GetClaims();

    Claim? GetClaimById(int claimId);

    void AddClaim(Claim claim);

    void UpdateClaim(Claim claim);

    void DeleteClaim(int claimId);

}