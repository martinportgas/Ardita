using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstCompanyLog
{
    public Guid CompanyIdLog { get; set; }

    public Guid CompanyId { get; set; }

    public string? CompanyCode { get; set; }

    public string? CompanyName { get; set; }

    public string? Address { get; set; }

    public string? Telepone { get; set; }

    public string? Email { get; set; }

    public bool? IsActive { get; set; }

    public string? Action { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstCompany Company { get; set; } = null!;
}
