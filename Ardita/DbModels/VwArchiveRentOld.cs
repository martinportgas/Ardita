using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class VwArchiveRentOld
{
    public int Volume { get; set; }

    public int? ArchiveYear { get; set; }

    public string? ArchiveCode { get; set; }

    public Guid MediaStorageInActiveId { get; set; }

    public Guid SubSubjectClassificationId { get; set; }

    public string? SubSubjectClassificationName { get; set; }

    public Guid SubjectClassificationId { get; set; }

    public string? SubjectClassificationName { get; set; }

    public string ClassificationName { get; set; } = null!;

    public string MediaStorageInActiveCode { get; set; } = null!;

    public string? StorageName { get; set; }

    public Guid ArchiveId { get; set; }

    public string TitleArchive { get; set; } = null!;

    public string CreatorName { get; set; } = null!;

    public string ArchiveUnit { get; set; } = null!;

    public long? StatusId { get; set; }

    public string? StatusName { get; set; }

    public int Sort { get; set; }

    public DateTime? RequestedDate { get; set; }

    public DateTime? RequestedReturnDate { get; set; }
}
