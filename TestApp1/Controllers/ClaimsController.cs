using Microsoft.AspNetCore.Mvc;
using TestApp1.Models;
using TestApp1.Services;

namespace TestApp1.Controllers;

public class ClaimsController : Controller
{
    private readonly IClaimsCollection _claimsCollection;

    public ClaimsController(IClaimsCollection claimsCollection)
    {
        this._claimsCollection = claimsCollection;
    }

    public IActionResult Index(StatusType? status = null, LossType? lossType = null)
    {
        ClaimsIndexViewModel viewModel = new()
        {
            Claims = _claimsCollection.GetClaims(status, lossType),
            Status = status,
            LossType = lossType,
        };
        return View(viewModel);
    }

    public IActionResult Details(int id)
    {
        Claim? claim = _claimsCollection.GetClaimById(id);
        if (claim == null)
        {
            return NotFound();
        }
        return View(claim);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new ClaimFormViewModel { LossDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ClaimFormViewModel form)
    {
        if (!ModelState.IsValid)
        {
            // Redisplay the form with the user's input and the error messages
            return View(form);
        }

        Claim claim = new()
        {
            Status = StatusType.Open,
            FiledDate = DateTime.Today,
        };
        form.ApplyTo(claim);
        _claimsCollection.AddClaim(claim);

        return RedirectToAction(nameof(Details), new { id = claim.ClaimId });
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Claim? claim = _claimsCollection.GetClaimById(id);
        if (claim == null)
        {
            return NotFound();
        }
        if (!claim.CanEdit)
        {
            return LockedRedirect(claim);
        }
        return View(ClaimFormViewModel.FromClaim(claim));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, ClaimFormViewModel form)
    {
        Claim? claim = _claimsCollection.GetClaimById(id);
        if (claim == null)
        {
            return NotFound();
        }
        // Checked again on the post because the button being hidden
        // doesn't stop someone from sending the request anyway
        if (!claim.CanEdit)
        {
            return LockedRedirect(claim);
        }
        if (!ModelState.IsValid)
        {
            form.ClaimId = id;
            return View(form);
        }

        form.ApplyTo(claim);
        _claimsCollection.UpdateClaim(claim);

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        Claim? claim = _claimsCollection.GetClaimById(id);
        if (claim == null)
        {
            return NotFound();
        }
        if (!claim.CanDelete)
        {
            TempData["DeleteError"] = $"Only Open claims can be deleted. This claim is {claim.Status}.";
            return RedirectToAction(nameof(Details), new { id });
        }

        _claimsCollection.DeleteClaim(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ChangeStatus(int id, StatusType newStatus)
    {
        Claim? claim = _claimsCollection.GetClaimById(id);
        if (claim == null)
        {
            return NotFound();
        }

        if (claim.TryTransitionTo(newStatus, out string error))
        {
            _claimsCollection.UpdateClaim(claim);
        }
        else
        {
            TempData["StatusError"] = error;
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    private IActionResult LockedRedirect(Claim claim)
    {
        TempData["Error"] = $"This claim can't be edited because it is {claim.Status}.";
        return RedirectToAction(nameof(Details), new { id = claim.ClaimId });
    }
}
