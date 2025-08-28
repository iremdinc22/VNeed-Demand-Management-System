using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vneed.Common.Exceptions.Interfaces;

namespace Vneed.Data.Models;

[Table("product")]
public class Product : ISoftDeletable
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [MaxLength(100)]
    public string? Note { get; set; }
    public int CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // bak buna 
    public virtual ICollection<Demand>? Demands { get; set; }
    
    public bool IsActive { get; set; } = true;
    public DateTimeOffset? DeletedAt { get; set; }
}