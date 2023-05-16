using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class VwArchiveApprovalInActive
{
    public Guid ApprovalId { get; set; }

    public string? ApprovalCode { get; set; }

    public Guid? TransId { get; set; }

    public int? ApprovalLevel { get; set; }

    public Guid? EmployeeId { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public long? StatusId { get; set; }

    public string? Note { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Title { get; set; }

    public string? RegistrationNumber { get; set; }

    public string ApprovalType { get; set; } = null!;
}
