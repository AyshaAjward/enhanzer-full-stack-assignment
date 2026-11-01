
using System.ComponentModel.DataAnnotations;

namespace Enhanzer.Api.Models;

public class CreatePurchaseBillRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one item is required.")]
    public List<CreatePurchaseBillItemRequest> Items { get; set; } = new();
}

public class CreatePurchaseBillItemRequest
{
    [Required]
    public string ItemName { get; set; } = "";

    [Required]
    public string BatchName { get; set; } = "";

    [Range(0, 999999999)]
    public decimal StandardCost { get; set; }

    [Range(0, 999999999)]
    public decimal StandardPrice { get; set; }

    [Range(0, 999999999)]
    public decimal Margin { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, int.MaxValue)]
    public int FreeQuantity { get; set; }

    [Range(0, 100)]
    public decimal Discount { get; set; }
}
