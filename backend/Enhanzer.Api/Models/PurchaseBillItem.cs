namespace Enhanzer.Api.Models;

public class PurchaseBillItem
{
    public int Id { get; set; }

    public int Purchase_Bill_Id { get; set; }

    public string Item_Name { get; set; } = "";

    public string Batch_Name { get; set; } = "";

    public decimal Standard_Cost { get; set; }

    public decimal Standard_Price { get; set; }

    public decimal Margin { get; set; }

    public int Quantity { get; set; }

    public int Free_Quantity { get; set; }

    public decimal Discount { get; set; }

    public decimal Total_Cost { get; set; }

    public decimal Total_Selling { get; set; }
}