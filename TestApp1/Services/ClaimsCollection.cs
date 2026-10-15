using TestApp1.Models;

namespace TestApp1.Services;

public class ClaimsCollection : IClaimsCollection
{
    private List<Claim> _claims;
    
    public ClaimsCollection()
    {
        Claim Claim1 = new()
        {
            ClaimId = 1,
            PolicyholderName = "Jerry Larry",
            PolicyAddress = "123 Street, Lehi, UT",
            PolicyNumber = 1000,
            LossType = LossType.Fire,
            Description = "There was a fire and my dog died.",
            LossDate = new DateTime(2026, 10, 13),
            FiledDate = new DateTime(2026, 10, 14),
            Status = StatusType.Open,
            LineItems = new LineItemsCollection("example 1")
        };
        Claim Claim2 = new() 
        {
            ClaimId = 2,
            PolicyholderName = "Mike Wazowski",
            PolicyAddress = "456 Street, Denver, CO",
            PolicyNumber = 2000,
            LossType = LossType.Theft,
            Description = "My house was stolen.",
            LossDate = new DateTime(2026, 10, 14),
            FiledDate = new DateTime(2026, 10, 14),
            Status = StatusType.Denied,
            LineItems = new LineItemsCollection("example 2")
        };

        _claims = new List<Claim>() { Claim1, Claim2 };
    }

    public List<Claim> GetClaims(StatusType? statusFilter = null, LossType? lossFilter = null)
    {
        IEnumerable<Claim> filteredClaims = _claims;

        if (statusFilter.HasValue)
        {
            filteredClaims = filteredClaims.Where(c => c.Status == statusFilter.Value);
        }

        if (lossFilter.HasValue)
        {
            filteredClaims = filteredClaims.Where(c => c.LossType == lossFilter.Value);
        }

        return filteredClaims.ToList();
    }

    public Claim? GetClaimById(int claimId)
    {
        return _claims.FirstOrDefault(c => c.ClaimId == claimId);
    }

    public void AddClaim(Claim claim)
    {
        // The collection owns ID generation, the way a database would
        claim.ClaimId = _claims.Count == 0 ? 1 : _claims.Max(c => c.ClaimId) + 1;
        _claims.Add(claim);
    }

    public void UpdateClaim(Claim claim)
    {
        var existingClaim = GetClaimById(claim.ClaimId);
        if (existingClaim != null)
        {
            existingClaim.PolicyholderName = claim.PolicyholderName;
            existingClaim.PolicyAddress = claim.PolicyAddress;
            existingClaim.PolicyNumber = claim.PolicyNumber;
            existingClaim.LossType = claim.LossType;
            existingClaim.Description = claim.Description;
            existingClaim.LossDate = claim.LossDate;
            existingClaim.FiledDate = claim.FiledDate;
            existingClaim.Status = claim.Status;
            existingClaim.LineItems = claim.LineItems;
        }
    }

    public void DeleteClaim(int claimId)
    {
        var claimToDelete = GetClaimById(claimId);
        if (claimToDelete != null)
        {
            _claims.Remove(claimToDelete);
        }
    }
}