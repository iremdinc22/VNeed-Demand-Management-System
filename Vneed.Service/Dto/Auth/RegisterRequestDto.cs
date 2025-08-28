using System.ComponentModel.DataAnnotations;

namespace Vneed.Services.Dto
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Ad Soyad en az 3, en fazla 100 karakter olabilir.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; }

        [Range(1, 10, ErrorMessage = "Geçerli bir rol seçiniz.")]
        public int RoleId { get; set; } = 1;
    }
}