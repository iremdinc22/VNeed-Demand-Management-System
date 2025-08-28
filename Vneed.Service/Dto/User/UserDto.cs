using System.ComponentModel.DataAnnotations;

namespace Vneed.Services.Dto;

public class UserDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Ad Soyad 3 ile 100 karakter arasında olmalıdır.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; }

    public string PasswordHash { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir rol ID'si giriniz.")]
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public bool IsActive { get; set; }
    public string? Token { get; set; }
}