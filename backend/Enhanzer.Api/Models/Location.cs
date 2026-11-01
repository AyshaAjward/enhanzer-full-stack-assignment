namespace Enhanzer.Api.Models;

public class Location
{
    public int Id { get; set; }
    public string Location_Code { get; set; } = "";
    public string Location_Name { get; set; } = "";
    public DateTime Created_At { get; set; }
}