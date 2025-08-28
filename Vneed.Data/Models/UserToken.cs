using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vneed.Data.Models;

[Table("user_token")]
public class UserToken
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public virtual User? User { get; set; }
    public string Token { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddHours(1);
    public bool IsActive { get; set; } = true;
}