using System.ComponentModel.DataAnnotations;

namespace Vneed.Services.Dto;

public class ProductDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ürün adı zorunludur.")]
    [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
    public string Name { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Max miktar 1 veya daha büyük olmalıdır.")]
    public int? MaxQuantity { get; set; }

    [StringLength(500, ErrorMessage = "Not en fazla 500 karakter olabilir.")]
    public string? Note { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori ID'si giriniz.")]
    public int CategoryId { get; set; }

    public bool? IsActive { get; set; }
}