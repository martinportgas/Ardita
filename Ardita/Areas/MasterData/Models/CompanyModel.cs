﻿namespace Ardita.Areas.MasterData.Models;

public class CompanyModel
{
    public Guid CompanyId { get; set; }

    public string CompanyCode { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string? Address { get; set; }

    public string? Telepone { get; set; }

    public string? Email { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }
}