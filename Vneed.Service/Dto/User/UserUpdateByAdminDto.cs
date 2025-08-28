using System.ComponentModel.DataAnnotations;

namespace Vneed.Services.Dto;

public class UserUpdateByAdminDto
{
    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Ad Soyad 3 ile 100 karakter arasında olmalıdır.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "E-posta zorunludur.")]
    public string Email { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir rol ID'si giriniz.")]
    public int RoleId { get; set; }

    public bool IsActive { get; set; }
}