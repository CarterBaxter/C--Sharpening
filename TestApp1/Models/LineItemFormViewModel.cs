using System.ComponentModel.DataAnnotations;

namespace TestApp1.Models;

// The fields a user is allowed to type into the add/edit line item form.
public class LineItemFormViewModel
{
    public int ClaimId { get; set; }

    public int ItemId { get; set; }

    [Required]
    public string Description { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "Choose a category.")]
    public CategoryType Category { get; set; } = CategoryType.None;

    [Range(0, double.MaxValue, ErrorMessage = "Cost per unit can't be negative.")]
    [Display(Name = "Cost per unit")]
    public decimal CostPerUnit { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Units must be greater than zero.")]
    public decimal Units { get; set; }

    public static LineItemFormViewModel FromLineItem(LineItem item)
    {
        return new LineItemFormViewModel
        {
            ClaimId = item.ClaimId,
            ItemId = item.ItemId,
            Description = item.Description,
            Category = item.Category,
            CostPerUnit = item.CostPerUnit,
            Units = item.Units,
        };
    }

    public void ApplyTo(LineItem item)
    {
        item.Description = Description;
        item.Category = Category;
        item.CostPerUnit = CostPerUnit;
        item.Units = Units;
    }
}
