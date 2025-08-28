namespace Vneed.Services.Dto;

public class LoginDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public string Token { get; set; } = null!;
}