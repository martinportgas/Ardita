using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels
{
    public class FormModel
    {
        public string CurrentArea { get; set; }
        public string CurrentController { get; set; }
        public string CurrentAction { get; set; }
        public string FormAction { get; set; }
        public string LastBreadcrumb { get; set; }
        public bool isInput { get; set; }
        public bool isSubmitForm { get; set; }
        public string ApprovalText { get; set; }
        public string SubmitText { get; set; }
        public string SaveText { get; set; }
        public string CancelText { get; set; }
        public string BackText { get; set; }
        public string RejectText { get; set; }
        public string ButtonSubmitClass { get; set; }
    }
}
