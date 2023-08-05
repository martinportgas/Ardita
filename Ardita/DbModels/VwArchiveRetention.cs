using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class VwArchiveRetention
{
    public Guid CreatorId { get; set; }

    public Guid ArchiveUnitId { get; set; }

    public Guid? GmdDetailId { get; set; }

    public string? ArchiveCode { get; set; }

    public Guid SubSubjectClassificationId { get; set; }

    public Guid? SubjectClassificationId { get; set; }

    public Guid ClassificationId { get; set; }

    public Guid RowId { get; set; }

    public Guid LevelId { get; set; }

    public Guid RackId { get; set; }

    public Guid RoomId { get; set; }

    public Guid? FloorId { get; set; }

    public DateTime CreatedDateArchive { get; set; }

    public Guid ArchiveId { get; set; }

    public string? ArchiveNumber { get; set; }

    public string TitleArchive { get; set; } = null!;

    public DateTime? RetentionDateArchive { get; set; }

    public string? ArchiveType { get; set; }

    public string CreatorName { get; set; } = null!;

    public string? Status { get; set; }
}
