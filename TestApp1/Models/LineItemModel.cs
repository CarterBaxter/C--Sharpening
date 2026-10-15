namespace TestApp1.Models;

public class LineItem
{
    public int ItemId { get; set; }
    public int ClaimId { get; set; }

    public string Description { get; set; } = "";

    public CategoryType Category { get; set; } = CategoryType.None;

    // Using Decimal vs Double/Float because its financial data
    // meaning the extra percision is nice
    public decimal CostPerUnit { get; set; } = 0.0m;

    // units might not always be whole either so using decimal again
    public decimal Units { get; set; } = 0.0m;

    // Only materials are taxed
    public const decimal MaterialsTaxRate = 0.065m;

    public decimal TotalCost()
    {
        return Math.Round(CostPerUnit * Units, 2);
    }

    public decimal TaxAmount()
    {
        return Category == CategoryType.Materials
            ? Math.Round(TotalCost() * MaterialsTaxRate, 2)
            : 0m;
    }

    public decimal TotalCostWithTax()
    {
        return TotalCost() + TaxAmount();
    }
}


public enum CategoryType
{
    None,
    Labor,
    Materials,
    Equipment,
    PersonalProperty,
}