using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class VwBaMovement
{
    public Guid Id { get; set; }

    public string CompanyName { get; set; } = null!;

    public string DocumentNo { get; set; } = null!;

    public string? ApproveDate { get; set; }

    public string? DayName { get; set; }

    public string? MonthName { get; set; }

    public int? Year { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string PositionName { get; set; } = null!;

    public string ArchiveUnitFrom { get; set; } = null!;

    public string ReceivedBy { get; set; } = null!;

    public string ReceivedPositionName { get; set; } = null!;

    public string ArchiveUnitDes { get; set; } = null!;

    public int? CountArchive { get; set; }

    public string CustomField1 { get; set; } = null!;

    public string CustomField2 { get; set; } = null!;

    public string CustomField3 { get; set; } = null!;

    public string CustomField4 { get; set; } = null!;

    public string CustomField5 { get; set; } = null!;
}
