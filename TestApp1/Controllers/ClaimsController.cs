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

    public IActionResult Index()
    {   
        List<Claim> Claims = _claimsCollection.GetClaims();
        return View(Claims);
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

}