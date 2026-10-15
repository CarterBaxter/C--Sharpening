using System.ComponentModel.DataAnnotations;
using TestApp1.Validation;

namespace TestApp1.Models;

// The fields a user is allowed to type into the create/edit claim form.
// Binding to this instead of Claim means a form post can't set things like
// Status, FiledDate or LineItems (this is called preventing "overposting").
public class ClaimFormViewModel
{
    public int ClaimId { get; set; }

    [Required]
    [Display(Name = "Policyholder name")]
    public string PolicyholderName { get; set; } = "";

    [Required]
    [Display(Name = "Policy address")]
    public string PolicyAddress { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "Policy number must be a positive number.")]
    [Display(Name = "Policy number")]
    public int PolicyNumber { get; set; }

    // Enum values are numbers underneath, so this rejects None (0)
    [Range(1, int.MaxValue, ErrorMessage = "Choose a loss type.")]
    [Display(Name = "Loss type")]
    public LossType LossType { get; set; } = LossType.None;

    [Required]
    public string Description { get; set; } = "";

    // Nullable so an empty date shows "required" instead of silently becoming 01/01/0001
    [Required(ErrorMessage = "Date of loss is required.")]
    [NotInFuture]
    [DataType(DataType.Date)]
    [Display(Name = "Date of loss")]
    public DateTime? LossDate { get; set; }

    public static ClaimFormViewModel FromClaim(Claim claim)
    {
        return new ClaimFormViewModel
        {
            ClaimId = claim.ClaimId,
            PolicyholderName = claim.PolicyholderName,
            PolicyAddress = claim.PolicyAddress,
            PolicyNumber = claim.PolicyNumber,
            LossType = claim.LossType,
            Description = claim.Description,
            LossDate = claim.LossDate,
        };
    }

    public void ApplyTo(Claim claim)
    {
        claim.PolicyholderName = PolicyholderName;
        claim.PolicyAddress = PolicyAddress;
        claim.PolicyNumber = PolicyNumber;
        claim.LossType = LossType;
        claim.Description = Description;
        claim.LossDate = LossDate!.Value;
    }
}
