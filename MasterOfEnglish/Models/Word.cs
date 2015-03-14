using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MasterOfEnglish.Models
{
    public enum WordStatus
    {
        znane,
        nieznane,
        nauczone
    }

    public class Word
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WordId { get; set; }

        [Required]
        [Display(Name = "Polskie słowo: ")]
        public string PolishWord { get; set; }

        [Required]
        [Display(Name = "Angielskie słowo: ")]
        public string EnglishWord { get; set; }

        [Display(Name = "Status: ")]
        public WordStatus Status { get; set; }

        [ScaffoldColumn(false)]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int CategoryId { get; set; }

        [Display(Name = "Kategoria: ")]
        public virtual Category Category { get; set; }
    }
}