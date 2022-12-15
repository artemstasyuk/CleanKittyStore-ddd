using Ardalis.GuardClauses;

namespace CatStore.Domain.BasketAggregate;

public class BasketItem
{
    public decimal UnitPrice { get; set; }
    
    public Guid CatId { get; set; }
    
    public int Quantity { get; set; }
    

    public BasketItem(Guid catId, int quantity,  decimal unitPrice)
    {
        CatId = catId;
        SetQuantity(quantity);
        UnitPrice = unitPrice;
    }
    
    public void AddQuantity(int quantity)
    {
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

        Quantity += quantity;
    }
    
    public void SetQuantity(int quantity)
    {
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

        Quantity = quantity;
    }
    
}