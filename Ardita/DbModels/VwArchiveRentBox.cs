using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class VwArchiveRentBox
{
    public Guid TrxArchiveRentId { get; set; }

    public Guid MediaStorageInActiveId { get; set; }

    public string MediaStorageInActiveCode { get; set; } = null!;

    public int Sort { get; set; }

    public string SubTypeStorageName { get; set; } = null!;

    public string ClassificationName { get; set; } = null!;

    public string ArchiveUnitName { get; set; } = null!;
}
