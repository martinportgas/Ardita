using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxFileArchiveDetail
{
    public Guid? FileArchiveDetailId { get; set; }

    public Guid? ArchiveId { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public string? FileType { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxArchive? Archive { get; set; }
}
