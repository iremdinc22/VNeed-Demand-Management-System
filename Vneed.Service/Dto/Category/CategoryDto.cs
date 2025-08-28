using System.ComponentModel.DataAnnotations;

namespace Vneed.Services.Dto;

public class CategoryDto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Kategori adı zorunludur.")]
    [StringLength(25, ErrorMessage = "Kategori adı en fazla 25 karakter olabilir.")]
    public string Name { get; set; }
}