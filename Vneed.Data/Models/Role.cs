using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vneed.Common.Exceptions.Interfaces;

namespace Vneed.Data.Models;

[Table("role")]
public class Role : ISoftDeletable
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<User>? Users { get; set; }
    
    public bool IsActive { get; set; } = true;
    public DateTimeOffset? DeletedAt { get; set; }

}