namespace Vneed.Common.Models;

public class ApiValidationErrorDetail
{
    public string Field { get; set; }
    public List<string> Messages { get; set; } = new();
}