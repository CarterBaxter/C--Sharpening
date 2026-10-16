using TestApp1.Models;

namespace TestApp1.UnitTests;

// Reqs Claims 4, Line items 2, Status workflow 4:
// a claim and its line items can only be edited before it reaches InReview
public class TestClaimCanEdit
{
    [TestCase(StatusType.None)]
    [TestCase(StatusType.Open)]
    public void CanEdit_BeforeInReview_ReturnsTrue(StatusType status)
    {
        Claim claim = new() { Status = status };

        bool canEdit = claim.CanEdit;

        Assert.That(canEdit, Is.False);
    }

    [TestCase(StatusType.InReview)]
    [TestCase(StatusType.Approved)]
    [TestCase(StatusType.Denied)]
    public void CanEdit_InReviewOrLater_ReturnsFalse(StatusType status)
    {
        Claim claim = new() { Status = status };

        bool canEdit = claim.CanEdit;

        Assert.That(canEdit, Is.False);
    }
}
