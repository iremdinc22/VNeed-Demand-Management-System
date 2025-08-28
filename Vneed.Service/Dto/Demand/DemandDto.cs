using System;
using System.ComponentModel.DataAnnotations;
using Vneed.Data.Enum;
using System.Text.Json.Serialization;


namespace Vneed.Services.Dto
{
    public class DemandDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [JsonIgnore]
        public string? UserFullName { get; set; } // AutoMapper ile doldurulabilir
        [Required]
        public int ProductId { get; set; }

        public string? ProductName { get; set; } // Automapper
        [Required]
        public PriorityLevel Priority { get; set; } = PriorityLevel.Low;
        [Required]
        public string? Note { get; set; }
        [JsonIgnore] 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore] 
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public DemandStatus Status { get; set; } = DemandStatus.PendingTeamLeadApproval;
        [JsonIgnore] 
        public string? StatusText { get; set; }
    }
}