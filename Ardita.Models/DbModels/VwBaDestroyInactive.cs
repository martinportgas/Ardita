using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class VwBaDestroyInactive
{
    public Guid Id { get; set; }

    public string DocumentNo { get; set; } = null!;

    public string? DayName { get; set; }

    public string? MonthName { get; set; }

    public int? Year { get; set; }

    public string? CompanyAddress { get; set; }

    public string ArchiveUnit { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string DestroyCode { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? DateFull { get; set; }

    public string? Date { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string CustomField1 { get; set; } = null!;

    public string CustomField2 { get; set; } = null!;

    public string CustomField3 { get; set; } = null!;

    public string CustomField4 { get; set; } = null!;

    public string CustomField5 { get; set; } = null!;
}
