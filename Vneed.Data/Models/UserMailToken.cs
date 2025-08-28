using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vneed.Data.Models;

[Table("user_mail_token")]
public class UserMailToken
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ResetKey { get; set; }
    public bool IsUsed { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }

    public virtual User User { get; set; }
}
