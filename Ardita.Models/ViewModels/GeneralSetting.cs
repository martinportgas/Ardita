using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels
{
    public class GeneralSetting
    {
        public Guid GeneralSettingsId { get; set; }

        public string SiteLogoContent { get; set; } = null!;

        public string SiteLogoFileType { get; set; } = null!;

        public string SiteLogoFileName { get; set; } = null!;

        public string CompanyLogoContent { get; set; } = null!;

        public string CompanyLogoFileType { get; set; } = null!;

        public string CompanyLogoFileName { get; set; } = null!;

        public string FavIconContent { get; set; } = null!;

        public string FavIconFileName { get; set; } = null!;

        public string FavIconFileType { get; set; } = null!;

        public string AplicationTitle { get; set; } = null!;

        public string TimeAndZone { get; set; } = null!;

        public string Footer { get; set; } = null!;

        public string LicenseKey { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
