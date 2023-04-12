﻿using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstCompany
{
    public Guid CompanyId { get; set; }

    public string? CompanyCode { get; set; }

    public string? CompanyName { get; set; }

    public string? Address { get; set; }

    public string? Telepone { get; set; }

    public string? Email { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<MstCompanyLog> MstCompanyLogs { get; } = new List<MstCompanyLog>();

    public virtual ICollection<TrxArchiveUnit> TrxArchiveUnits { get; } = new List<TrxArchiveUnit>();
}
