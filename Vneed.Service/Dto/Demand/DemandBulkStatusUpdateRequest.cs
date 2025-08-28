namespace Vneed.Services.Dto;

public class DemandBulkStatusUpdateRequest
{
    public List<int>? DemandIds { get; set; }
    public int NewStatus { get; set; }
}