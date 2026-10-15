using Microsoft.AspNetCore.Mvc;
using TestApp1.Models;
using TestApp1.Services;

namespace TestApp1.Controllers;

// Line items only exist inside a claim, so every action takes the claimId
// and goes through the claim to reach its line items.
public class LineItemsController : Controller
{
    private readonly IClaimsCollection _claimsCollection;

    public LineItemsController(IClaimsCollection claimsCollection)
    {
        this._claimsCollection = claimsCollection;
    }

    [HttpGet]
    public IActionResult Create(int claimId)
    {
        Claim? claim = _claimsCollection.GetClaimById(claimId);
        if (claim == null)
        {
            return NotFound();
        }
        if (!claim.CanEdit)
        {
            return LockedRedirect(claim);
        }
        return View(new LineItemFormViewModel { ClaimId = claimId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(int claimId, LineItemFormViewModel form)
    {
        Claim? claim = _claimsCollection.GetClaimById(claimId);
        if (claim == null)
        {
            return NotFound();
        }
        if (!claim.CanEdit)
        {
            return LockedRedirect(claim);
        }
        if (!ModelState.IsValid)
        {
            form.ClaimId = claimId;
            return View(form);
        }

        LineItem item = new() { ClaimId = claimId };
        form.ApplyTo(item);
        claim.LineItems.AddLineItem(item);

        return RedirectToClaim(claimId);
    }

    [HttpGet]
    public IActionResult Edit(int claimId, int id)
    {
        Claim? claim = _claimsCollection.GetClaimById(claimId);
        LineItem? item = claim?.LineItems.GetLineItemById(id);
        if (claim == null || item == null)
        {
            return NotFound();
        }
        if (!claim.CanEdit)
        {
            return LockedRedirect(claim);
        }
        return View(LineItemFormViewModel.FromLineItem(item));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int claimId, int id, LineItemFormViewModel form)
    {
        Claim? claim = _claimsCollection.GetClaimById(claimId);
        LineItem? item = claim?.LineItems.GetLineItemById(id);
        if (claim == null || item == null)
        {
            return NotFound();
        }
        if (!claim.CanEdit)
        {
            return LockedRedirect(claim);
        }
        if (!ModelState.IsValid)
        {
            form.ClaimId = claimId;
            form.ItemId = id;
            return View(form);
        }

        form.ApplyTo(item);
        claim.LineItems.UpdateLineItem(item);

        return RedirectToClaim(claimId);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int claimId, int id)
    {
        Claim? claim = _claimsCollection.GetClaimById(claimId);
        if (claim == null)
        {
            return NotFound();
        }
        if (!claim.CanEdit)
        {
            return LockedRedirect(claim);
        }

        claim.LineItems.DeleteLineItem(id);
        return RedirectToClaim(claimId);
    }

    private IActionResult RedirectToClaim(int claimId)
    {
        return RedirectToAction("Details", "Claims", new { id = claimId });
    }

    private IActionResult LockedRedirect(Claim claim)
    {
        TempData["Error"] = $"Line items can't be changed because the claim is {claim.Status}.";
        return RedirectToClaim(claim.ClaimId);
    }
}
