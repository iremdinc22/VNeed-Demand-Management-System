using System.ComponentModel.DataAnnotations;

namespace Vneed.Services.Dto;

public class UserTokenDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir Id giriniz.")]
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kullanıcı ID'si giriniz.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Token zorunludur.")]
    public string Token { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;
}