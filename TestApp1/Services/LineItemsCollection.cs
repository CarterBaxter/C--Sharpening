using TestApp1.Models;

namespace TestApp1.Services;

public class LineItemsCollection : ILineItemsCollection
{
    private List<LineItem> _lineItems;

    public LineItemsCollection(string example = "")
    {
        if (example == "example 1")
        {
            _lineItems = new List<LineItem>() 
            {
                new()
                {
                    ItemId = 1,
                    ClaimId = 1,
                    Description = "Dog",
                    Category = CategoryType.PersonalProperty, // maybe should make this an "other" category
                    CostPerUnit = 1000.0m, // It was a good dog
                    Units = 1.0m
                },
            };
        }
        else if (example == "example 2")
        {
            _lineItems = new List<LineItem>() 
            {
                new()
                {
                    ItemId = 2,
                    ClaimId = 2,
                    Description = "House",
                    Category = CategoryType.PersonalProperty,
                    CostPerUnit = 15000000.0m,
                    Units = 1.0m
                },
                new()
                {
                    ItemId = 3,
                    ClaimId = 2,
                    Description = "Stuff in house",
                    Category = CategoryType.PersonalProperty,
                    CostPerUnit = 10.0m,
                    Units = 1000.0m
                },
                new()
                {
                    ItemId = 4,
                    ClaimId = 2,
                    Description = "Moving team that moved my house", // insurance fraud 
                    Category = CategoryType.Labor,
                    CostPerUnit = 100.0m,
                    Units = 10.0m
                }

            };
        }
        else
        {
            _lineItems = new List<LineItem>();
        }
    }

    public List<LineItem> GetLineItems()
    {
        return _lineItems;
    }

    public LineItem? GetLineItemById(int itemId)
    {
        return _lineItems.FirstOrDefault(item => item.ItemId == itemId);
    }

    public void AddLineItem(LineItem lineItem)
    {
        lineItem.ItemId = _lineItems.Count == 0 ? 1 : _lineItems.Max(item => item.ItemId) + 1;
        _lineItems.Add(lineItem);
    }

    public void UpdateLineItem(LineItem lineItem)
    {
        var existingLineItem = GetLineItemById(lineItem.ItemId);
        if (existingLineItem != null)
        {
            existingLineItem.ClaimId = lineItem.ClaimId;
            existingLineItem.Description = lineItem.Description;
            existingLineItem.Category = lineItem.Category;
            existingLineItem.CostPerUnit = lineItem.CostPerUnit;
            existingLineItem.Units = lineItem.Units;
        }
    }

    public void DeleteLineItem(int itemId)
    {
        var lineItem = GetLineItemById(itemId);
        if (lineItem != null)
        {
            _lineItems.Remove(lineItem);
        }
    }

    public decimal CalculateTotalCost()
    {
        return _lineItems.Aggregate(0m, (acc, item) => acc + item.TotalCost());
    }

    public decimal CalculateTotalTax()
    {
        return _lineItems.Sum(item => item.TaxAmount());
    }

    public decimal CalculateTotalCostWithTax()
    {
        return CalculateTotalCost() + CalculateTotalTax();
    }
}