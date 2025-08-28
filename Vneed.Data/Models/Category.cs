using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vneed.Common.Exceptions.Interfaces;

namespace Vneed.Data.Models;

[Table("category")]
public class Category : ISoftDeletable
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual ICollection<Product>? Products { get; set; }
    public virtual ICollection<ProductSuggestion>? ProductSuggestions { get; set; }
    
    public bool IsActive { get; set; } = true;
    public DateTimeOffset? DeletedAt { get; set; }
}