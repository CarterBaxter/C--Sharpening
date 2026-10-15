using TestApp1.Models;

namespace TestApp1.Services;

public interface ILineItemsCollection
{
    List<LineItem> GetLineItems();
    LineItem? GetLineItemById(int itemId);

    void AddLineItem(LineItem lineItem);

    void UpdateLineItem(LineItem lineItem);

    void DeleteLineItem(int itemId);

    decimal CalculateTotalCost();

    decimal CalculateTotalTax();

    decimal CalculateTotalCostWithTax();
}