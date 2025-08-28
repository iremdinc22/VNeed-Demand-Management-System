using Vneed.Data.Enum;

public class DemandFilterDto
{
    public DemandStatus? Status { get; set; }
    public PriorityLevel? Priority { get; set; }

    private DateTime? _startDate;
    public DateTime? StartDate
    {
        get => _startDate;
        set => _startDate = value.HasValue
            ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
            : null;
    }

    private DateTime? _endDate;
    public DateTime? EndDate
    {
        get => _endDate;
        set => _endDate = value.HasValue
            ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
            : null;
    }

    public bool SortByNewest { get; set; } = true;

    // Eğer true ise sadece giriş yapan kullanıcıya ait talepler listelenir
    public bool OnlyCurrentUser { get; set; } = false;
}