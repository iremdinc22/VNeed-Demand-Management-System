using System.ComponentModel.DataAnnotations;

namespace Vneed.Services.Dto;

public class UserActiveStatusDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kullanıcı ID'si giriniz.")]
    public int Id { get; set; }
    public bool IsActive { get; set; }
}