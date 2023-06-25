using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstBorrower
{
    public Guid BorrowerId { get; set; }

    public string? BorrowerName { get; set; }

    public string? BorrowerCompany { get; set; }

    public string? BorrowerArchiveUnit { get; set; }

    public string? BorrowerPosition { get; set; }

    public string? BorrowerIdentityNumber { get; set; }

    public string? BorrowerPhone { get; set; }

    public string? BorrowerEmail { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<TrxRentHistory> TrxRentHistories { get; } = new List<TrxRentHistory>();
}
