
namespace Enhanzer.Api.Models;

public class PurchaseBill
{
    public int Id { get; set; }

    public DateTime Created_At { get; set; } = DateTime.UtcNow;
}
