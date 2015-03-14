using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterOfEnglish.Models
{
    public class EditPasswordViewModel
    {
        public string ActualPassword { get; set; }
        public string NewPassword { get; set; }
        public string ReapeatNewPassword { get; set; }
    }
}