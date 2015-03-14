using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MasterOfEnglish.Models
{
    public class EditEmailViewModel
    {
        [EmailAddress]
        [StringLength(100)]
        public string NewEmail { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string ConfirmNewEmail { get; set; }
    }
}