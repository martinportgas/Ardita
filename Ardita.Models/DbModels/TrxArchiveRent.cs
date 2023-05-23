﻿using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxArchiveRent
{
    public Guid TrxArchiveRentId { get; set; }

    public Guid UserId { get; set; }

    public Guid ArchiveId { get; set; }

    public DateTime? RequestedDate { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public DateTime? RequestedReturnDate { get; set; }

    public DateTime? ApprovalReturnDate { get; set; }

    public string? Description { get; set; }

    public long? StatusId { get; set; }

    public Guid? ApprovedBy { get; set; }

    public string? ApprovalNotes { get; set; }

    public Guid? RejectedBy { get; set; }

    public DateTime? RetrievalDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxArchive Archive { get; set; } = null!;

    public virtual MstStatus? Status { get; set; }

    public virtual MstUser User { get; set; } = null!;
}