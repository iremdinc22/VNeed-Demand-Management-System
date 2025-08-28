namespace Vneed.Common.Exceptions.Interfaces;

public interface ISoftDeletable
{
    public bool IsActive { get; set; } 
    public DateTimeOffset? DeletedAt { get; set; }
}
