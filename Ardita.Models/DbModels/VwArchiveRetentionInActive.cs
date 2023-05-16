using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class VwArchiveRetentionInActive
{
    public Guid ArchiveId { get; set; }

    public string? ArchiveNumber { get; set; }

    public string TitleArchive { get; set; } = null!;

    public DateTime? RetentionDateArchive { get; set; }

    public string? ArchiveType { get; set; }

    public string CreatorName { get; set; } = null!;

    public string? Status { get; set; }
}
