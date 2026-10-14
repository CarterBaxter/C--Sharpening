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
    public List<LineItem> LineItems { get; set; } = new List<LineItem>();
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
    Closed,
    Denied,
}