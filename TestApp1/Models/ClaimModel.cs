using TestApp1.Services;

namespace TestApp1.Models;

public class Claim
{
    public int ClaimId { get; set; }
    public string PolicyholderName { get; set; } = "";

    public string PolicyAddress { get; set; } = "";

    public int PolicyNumber { get; set; }

    public LossType LossType { get; set; } = LossType.None;

    public string Description { get; set; } = "";

    public DateTime LossDate { get; set; }

    public DateTime FiledDate { get; set; }

    public StatusType Status { get; set; } = StatusType.None;

    // I dont know if its too redundant to have the list and for
    // list items to have a claim Id because it would be in a database,
    // but for code its kinda nice to be able to reference either way.
    public ILineItemsCollection LineItems { get; set; } = new LineItemsCollection(); 

    public decimal CalculateTotalClaimCost()
    {
        return LineItems.CalculateTotalCostWithTax();
    }

    // The status workflow: which statuses each status is allowed to move to.
    // Anything not listed here is skipping a step or moving backward.
    private static readonly Dictionary<StatusType, StatusType[]> AllowedTransitions = new()
    {
        [StatusType.None] = [StatusType.Open],
        [StatusType.Open] = [StatusType.InReview],
        [StatusType.InReview] = [StatusType.Approved, StatusType.Denied],
        [StatusType.Approved] = [],
        [StatusType.Denied] = [],
    };

    // Once a claim reaches InReview it (and its line items) are locked
    public bool CanEdit => Status is StatusType.None or StatusType.Open;

    public bool CanDelete => Status == StatusType.Open;

    public IReadOnlyList<StatusType> GetAllowedNextStatuses()
    {
        return AllowedTransitions[Status];
    }

    // Follows the same pattern as int.TryParse: returns whether it worked
    // and hands back the reason through the out parameter when it didn't.
    public bool TryTransitionTo(StatusType newStatus, out string error)
    {
        if (!AllowedTransitions[Status].Contains(newStatus))
        {
            error = $"A claim can't move from {Status} to {newStatus}. "
                + "Claims must go Open → InReview → Approved or Denied, without skipping steps or moving backward.";
            return false;
        }

        if (newStatus == StatusType.InReview && LineItems.GetLineItems().Count == 0)
        {
            error = "A claim needs at least one line item before it can move to InReview.";
            return false;
        }

        Status = newStatus;
        error = "";
        return true;
    }
}

public enum LossType
{
    None,
    Fire,
    Water,
    Theft,
}

public enum StatusType
{
    None,
    Open,
    InReview,
    Approved,
    Denied,
}