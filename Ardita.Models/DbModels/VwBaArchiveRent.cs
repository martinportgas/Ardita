using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class VwBaArchiveRent
{
    public Guid Id { get; set; }

    public string? NoBa { get; set; }

    public string? DdMmmmyyyy { get; set; }

    public string? Dd { get; set; }

    public string? Dddd { get; set; }

    public string? DdString { get; set; }

    public string? Mmstring { get; set; }

    public string ArchiveUnit { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string? CompanyAddress { get; set; }

    public string? BorrowerName { get; set; }

    public string? BorrowerNik { get; set; }

    public string? BorrowerPosition { get; set; }

    public string? BorrowerUnit { get; set; }

    public string? BorrowerCompany { get; set; }

    public string ApproverName { get; set; } = null!;

    public string ApproverNik { get; set; } = null!;

    public string ApproverPosition { get; set; } = null!;

    public string? Description { get; set; }

    public string? CompanyCity { get; set; }

    public string CustomField1 { get; set; } = null!;

    public string CustomField2 { get; set; } = null!;

    public string CustomField3 { get; set; } = null!;

    public string CustomField4 { get; set; } = null!;

    public string CustomField5 { get; set; } = null!;
}
