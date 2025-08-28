using System.ComponentModel.DataAnnotations;

namespace Vneed.Data.Enum;

public enum DemandStatus
{
    [Display(Name = "TeamLead Onayı Bekleniyor")]
    PendingTeamLeadApproval = 0,

    [Display(Name = "TeamLead Tarafından Onaylandı")]
    ApprovedByTeamLead = 1,

    [Display(Name = "TeamLead Tarafından Reddedildi")]
    RejectedByTeamLead = 2,

    [Display(Name = "TeamLead Tarafından Onaylandı")]
    Approved = 3,

    [Display(Name = "TeamLead Tarafından Reddedildi")]
    Rejected = 4,

    [Display(Name = "Sipariş Verildi")]
    Completed = 5
}