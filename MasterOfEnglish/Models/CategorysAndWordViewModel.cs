using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterOfEnglish.Models
{
    public class CategorysAndWordViewModel
    {
        public Word Word { get; set; }
        public List<Category> Categorys { get; set; }
    }
}