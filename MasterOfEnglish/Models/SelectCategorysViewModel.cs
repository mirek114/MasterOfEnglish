using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterOfEnglish.Models
{
    public class SelectCategorysViewModel
    {
        public List<Category> categorys {get; set;}
        public List<int> selectedCategorys { get; set; }
    }
}