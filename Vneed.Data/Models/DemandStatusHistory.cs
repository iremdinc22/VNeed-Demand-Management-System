using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vneed.Data.Enum;

namespace Vneed.Data.Models;

[Table("demand_status_history")]
public class DemandStatusHistory
{
    [Key]
    public int Id { get; set; }

    public int DemandId { get; set; }
       
    [ForeignKey(nameof(DemandId))]
    public virtual Demand Demand { get; set; }

    public DemandStatus Status { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    public int ChangedByUserId { get; set; }
}