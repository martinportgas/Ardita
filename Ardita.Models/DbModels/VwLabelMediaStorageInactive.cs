using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class VwLabelMediaStorageInactive
{
    public Guid Id { get; set; }

    public string? StorageLoc { get; set; }

    public string CreatorCode { get; set; } = null!;

    public string? DocumentNumber { get; set; }

    public string StorageCode { get; set; } = null!;

    public string CustomField1 { get; set; } = null!;

    public string CustomField2 { get; set; } = null!;

    public string CustomField3 { get; set; } = null!;

    public string CustomField4 { get; set; } = null!;

    public string CustomField5 { get; set; } = null!;
}
