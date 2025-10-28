using System.ComponentModel.DataAnnotations;

namespace QLSV.Models.DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
    public string TenDangNhap { get; set; } = null!;

    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    public string MatKhau { get; set; } = null!;
}

public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public UserInfo? UserInfo { get; set; }
    public string? Token { get; set; }
}

public class UserInfo
{
    public int Id { get; set; }
    public string TenDangNhap { get; set; } = null!;
    public string LoaiTk { get; set; } = null!;
    public string HoTen { get; set; } = null!;
    public string? Email { get; set; }
    public string? Sdt { get; set; }
}

public class LogoutRequest
{
    public string Token { get; set; } = null!;
}
