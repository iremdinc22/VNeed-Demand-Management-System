using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vneed.Data.Enum;
using Vneed.Common.Exceptions.Interfaces;

namespace Vneed.Data.Models;

[Table("demand")]
public class Demand : ISoftDeletable
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public virtual User? User { get; set; }
    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }
    public PriorityLevel Priority { get; set; } = PriorityLevel.Low;

    [MaxLength(100)]
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DemandStatus Status { get; set; } = DemandStatus.PendingTeamLeadApproval;
    public virtual ICollection<DemandStatusHistory> StatusHistories { get; set; } = new List<DemandStatusHistory>();
    // Koşullar bunlar : beklemede, onaylandı, reddedildi, sipariş verildi
    
    public bool IsActive { get; set; } = true;
    public DateTimeOffset? DeletedAt { get; set; }
}