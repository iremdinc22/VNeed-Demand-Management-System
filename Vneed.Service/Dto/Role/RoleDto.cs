using System.ComponentModel.DataAnnotations;

namespace Vneed.Services.Dto;

public class RoleDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Rol adı zorunludur.")]
    [StringLength(50, ErrorMessage = "Rol adı en fazla 50 karakter olabilir.")]
    public string Name { get; set; } 
}