using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserManager.Web.ViewModels
{
    public class UserFormViewModel
    {
        [DisplayName("Ime")]
        [Required(ErrorMessage = "{0} je obavezno upisati")]
        [StringLength(50, ErrorMessage = "{0} ne smije biti duže od 50 znakova")]
        public string FirstName { get; set; } = string.Empty;
        
        [DisplayName("Prezime")]
        [Required(ErrorMessage = "{0} je obavezno upisati")]
        [StringLength(50, ErrorMessage = "{0} ne smije biti duže od 50 znakova")]
        public string LastName { get; set; } = string.Empty;

        [DisplayName("Email")]
        [Required(ErrorMessage = "{0} je obavezno upisati")]
        [StringLength(100, ErrorMessage = "{0} ne smije biti duži od 100 znakova")]
        [EmailAddress(ErrorMessage = "{0} nije u valjanom formatu")]
        public string Email { get; set; } = string.Empty;
    }
}
