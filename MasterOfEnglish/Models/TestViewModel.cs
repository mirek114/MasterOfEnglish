using MasterOfEnglish.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterOfEnglish.Models
{
    public class TestViewModel
    {
        public Word word { get; set; }
        public LanguageOfWordsInTest languageOfWordsInTest { get; set; }
    }
}