using MasterOfEnglish.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterOfEnglish.Models
{
    public class SelectTestParametersViewModel
    {
        public List<Category> categorys { get; set; }
        public List<int> selectedCategorys { get; set; }
        public LanguageOfWordsInTest languageOfWordsInTest { get; set; }
    }
}