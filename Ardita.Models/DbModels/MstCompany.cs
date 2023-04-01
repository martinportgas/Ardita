using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstCompany
{
    public int CompanyId { get; set; }

    public string? CompanyCode { get; set; }

    public string? CompanyName { get; set; }

    public string? Address { get; set; }

    public string? Telepone { get; set; }

    public string? Email { get; set; }
}
