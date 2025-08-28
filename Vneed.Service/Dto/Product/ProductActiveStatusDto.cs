using System.ComponentModel.DataAnnotations;

namespace Vneed.Services.Dto;

public class ProductActiveStatusDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir ürün ID'si giriniz.")]
    public int Id { get; set; }
    public bool IsActive { get; set; }
}