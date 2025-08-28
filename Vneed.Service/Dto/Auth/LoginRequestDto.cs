using System.ComponentModel.DataAnnotations;

namespace Vneed.Services.Dto;

public class LoginRequestDto
{
    [Required(ErrorMessage = "E-posta boş olamaz.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Şifre boş olamaz.")]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    public string Password { get; set; }
}