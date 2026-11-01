
using Enhanzer.Api.Data;
using Enhanzer.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enhanzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseBillsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PurchaseBillsController(AppDbContext db)
    {
        _db = db;
    }

    private bool IsAuthenticated()
    {
        return HttpContext.Session.GetString("IsAuthenticated") == "true";
    }

    [HttpGet("access-check")]
    public IActionResult AccessCheck()
    {
        if (!IsAuthenticated())
        {
            return Unauthorized(new { message = "Please log in first." });
        }

        return Ok(new { message = "You are authenticated." });
    }

    [HttpPost]
    public async Task<IActionResult> CreatePurchaseBill(
        [FromBody] CreatePurchaseBillRequest request)
    {
        if (!IsAuthenticated())
        {
            return Unauthorized(new { message = "Please log in first." });
        }

        if (request.Items == null || request.Items.Count == 0)
        {
            return BadRequest(new { message = "Please add at least one item." });
        }

        // Validate item names and ensure batches are saved locations.
        var validLocationNames = await _db.Locations
            .Select(x => x.Location_Name)
            .ToListAsync();

        foreach (var item in request.Items)
        {
            if (string.IsNullOrWhiteSpace(item.ItemName) ||
                string.IsNullOrWhiteSpace(item.BatchName))
            {
                return BadRequest(new { message = "Item and batch are required." });
            }

            if (!validLocationNames.Contains(item.BatchName))
            {
                return BadRequest(new
                {
                    message = $"Invalid batch/location: {item.BatchName}"
                });
            }

            if (item.Quantity <= 0 ||
                item.FreeQuantity < 0 ||
                item.StandardCost < 0 ||
                item.StandardPrice < 0 ||
                item.Discount < 0 ||
                item.Discount > 100)
            {
                return BadRequest(new { message = "One or more item values are invalid." });
            }
        }

        await using var transaction =
            await _db.Database.BeginTransactionAsync();

        try
        {
            // Create the bill header first.
            var bill = new PurchaseBill
            {
                Created_At = DateTime.UtcNow
            };

            _db.PurchaseBills.Add(bill);
            await _db.SaveChangesAsync();

            // Save all items using the generated bill ID.
            var billItems = request.Items.Select(item =>
            {
                decimal totalCost =
                    item.StandardCost * item.Quantity *
                    (1 - item.Discount / 100);

                decimal totalSelling =
                    item.StandardPrice * item.Quantity;

                return new PurchaseBillItem
                {
                    Purchase_Bill_Id = bill.Id,
                    Item_Name = item.ItemName,
                    Batch_Name = item.BatchName,
                    Standard_Cost = item.StandardCost,
                    Standard_Price = item.StandardPrice,
                    Margin = item.StandardPrice - item.StandardCost,
                    Quantity = item.Quantity,
                    Free_Quantity = item.FreeQuantity,
                    Discount = item.Discount,
                    Total_Cost = totalCost,
                    Total_Selling = totalSelling
                };
            }).ToList();

            _db.PurchaseBillItems.AddRange(billItems);
            await _db.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(new
            {
                message = "Purchase bill saved successfully.",
                billId = bill.Id,
                totalItems = billItems.Count,
                totalQuantity = billItems.Sum(x => x.Quantity)
            });
        }
        catch
        {
            await transaction.RollbackAsync();

            return StatusCode(500, new
            {
                message = "Unable to save the purchase bill."
            });
        }
    }
}
