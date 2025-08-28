using System.ComponentModel.DataAnnotations;

namespace Vneed.Services.Dto;

public class ProductSuggestionDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Önerilen ürün adı zorunludur.")]
    [StringLength(150, ErrorMessage = "Önerilen ürün adı en fazla 150 karakter olabilir.")]
    public string SuggestedName { get; set; }

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }
    
    public bool? IsApproved { get; set; }
}