using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vneed.Common.Exceptions.Interfaces;

namespace Vneed.Data.Models;

[Table("user")]
public class User 
{
    [Key]
    public int Id { get; set; }

    public int RoleId { get; set; }
    public virtual Role? Role { get; set; }

    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTimeOffset? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Demand>? Demands { get; set; }
    public virtual ICollection<UserToken>? UserTokens { get; set; }
    public virtual ICollection<ProductSuggestion>? ProductSuggestions { get; set; }
    public virtual ICollection<UserMailToken>? UserMailTokens { get; set; }

}