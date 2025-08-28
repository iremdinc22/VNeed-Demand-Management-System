using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vneed.Common.Exceptions.Interfaces;

namespace Vneed.Data.Models;

[Table("product_suggestion")]
public class ProductSuggestion : ISoftDeletable
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string SuggestedName { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }
    public virtual User User { get; set; }

    public int CategoryId { get; set; }
    public virtual Category? Category { get; set; }

    public bool? IsApproved { get; set; }
    
    public bool IsActive { get; set; } = true;
    public DateTimeOffset? DeletedAt { get; set; }
}