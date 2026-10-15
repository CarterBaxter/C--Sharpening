using Microsoft.AspNetCore.Mvc;
using TestApp1.Models;

namespace TestApp1.Controllers;

public class ClaimsController : Controller
{
    public IActionResult Index()
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
                LineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        ItemId = 1,
                        ClaimId = 1,
                        Description = "Dog",
                        Category = CategoryType.PersonalProperty, // maybe should make this an "other" category
                        CostPerUnit = 1000.00m, // It was a good dog
                        Units = 1.0m
                    }
                }
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
                LineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        ItemId = 2,
                        ClaimId = 2,
                        Description = "House",
                        Category = CategoryType.PersonalProperty,
                        CostPerUnit = 15000000.0m,
                        Units = 1.0m
                    },
                    new LineItem
                    {
                        ItemId = 3,
                        ClaimId = 2,
                        Description = "Stuff in house",
                        Category = CategoryType.PersonalProperty,
                        CostPerUnit = 10.0m,
                        Units = 1000.0m
                    },
                    new LineItem
                    {
                        ItemId = 4,
                        ClaimId = 2,
                        Description = "Moving team that moved my house", // insurance fraud 
                        Category = CategoryType.Labor,
                        CostPerUnit = 100.0m,
                        Units = 10.0m
                    }

                }
            };

        List<Claim> Claims = new List<Claim> { Claim1, Claim2 };

        return View(Claims);
    }

}