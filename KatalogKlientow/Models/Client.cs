using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KatalogKlientow.Models
{
    public class Client
    {
        [Browsable(false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nazwa jest wymagana")]
        public string Nazwa { get; set; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = "NIP musi zawierać dokładnie 10 znaków")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Tylko cyfry są dozwolone")]
        public string Nip { get; set; }
        [StringLength(254, ErrorMessage = "Adres może mieć maksymalnie 254 znaki")]
        public string Adres { get; set; }
        [DisplayName("Numer Telefonu")]
        public string NrTel { get; set; }
        [EmailAddress(ErrorMessage = "Nie poprawny adres email")]
        [Required(ErrorMessage = "Email jest wymagany")]
        public string Email { get; set; }
    }
}
