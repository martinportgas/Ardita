using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxRentHistory
{
    public Guid RentHistoryId { get; set; }

    public Guid BorrowerId { get; set; }

    public Guid TrxArchiveRentId { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public virtual MstBorrower Borrower { get; set; } = null!;

    public virtual TrxArchiveRent TrxArchiveRent { get; set; } = null!;
}
