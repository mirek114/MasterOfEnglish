using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MasterOfEnglish.Models
{
    public class User
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Adres Email: ")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Hasło: ")]
        public string Password { get; set; }

        [ScaffoldColumn(false)]
        public string PasswordSalt { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }

        public virtual List<Word> Words { get; set; }
        public virtual List<Category> Categorys { get; set; }

    }
}