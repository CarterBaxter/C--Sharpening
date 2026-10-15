namespace TestApp1.Models;

public class ClaimsIndexViewModel
{
    public List<Claim> Claims { get; set; } = new();

    public StatusType? Status { get; set; }

    public LossType? LossType { get; set; }
}
