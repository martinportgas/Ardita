using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class VwLabelMediaStorageActive
{
    public Guid Id { get; set; }

    public string? Rack { get; set; }

    public string? Level { get; set; }

    public string? Row { get; set; }

    public string? SubjectClassificationCode { get; set; }

    public string Year { get; set; } = null!;

    public string CustomField1 { get; set; } = null!;

    public string CustomField2 { get; set; } = null!;

    public string CustomField3 { get; set; } = null!;

    public string CustomField4 { get; set; } = null!;

    public string CustomField5 { get; set; } = null!;
}
