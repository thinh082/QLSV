using Microsoft.EntityFrameworkCore;
using QLSV.Models.Entities;
using QLSV.Models.DTOs;

namespace QLSV.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<bool> LogoutAsync(string token);
    Task<UserInfo?> GetUserInfoAsync(int userId, string userType);
}

public class AuthService : IAuthService
{
    private readonly DKSVContext _context;
    private readonly ILogger<AuthService> _logger;

    public AuthService(DKSVContext context, ILogger<AuthService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var taiKhoan = await _context.TaiKhoans
                .FirstOrDefaultAsync(tk => tk.TenDangNhap == request.TenDangNhap 
                                        && tk.MatKhau == request.MatKhau);

            if (taiKhoan == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Tên đăng nhập hoặc mật khẩu không đúng"
                };
            }

            UserInfo userInfo = null!;

            if (taiKhoan.LoaiTk == "SinhVien")
            {
                var sinhVien = await _context.SinhViens
                    .FirstOrDefaultAsync(sv => sv.TaiKhoanId == taiKhoan.Id);

                if (sinhVien != null)
                {
                    userInfo = new UserInfo
                    {
                        Id = sinhVien.Id,
                        TenDangNhap = taiKhoan.TenDangNhap,
                        LoaiTk = taiKhoan.LoaiTk,
                        HoTen = sinhVien.HoTen,
                        Email = sinhVien.Email,
                        Sdt = sinhVien.Sdt
                    };
                }
            }
            else if (taiKhoan.LoaiTk == "GiangVien")
            {
                var giangVien = await _context.GiangViens
                    .FirstOrDefaultAsync(gv => gv.TaiKhoanId == taiKhoan.Id);

                if (giangVien != null)
                {
                    userInfo = new UserInfo
                    {
                        Id = giangVien.Id,
                        TenDangNhap = taiKhoan.TenDangNhap,
                        LoaiTk = taiKhoan.LoaiTk,
                        HoTen = giangVien.HoTen,
                        Email = giangVien.Email,
                        Sdt = giangVien.Sdt
                    };
                }
            }
            else if (taiKhoan.LoaiTk == "Admin")
            {
                userInfo = new UserInfo
                {
                    Id = taiKhoan.Id,
                    TenDangNhap = taiKhoan.TenDangNhap,
                    LoaiTk = taiKhoan.LoaiTk,
                    HoTen = "Administrator",
                    Email = null,
                    Sdt = null
                };
            }

            if (userInfo == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Không tìm thấy thông tin người dùng"
                };
            }

            // Tạo token đơn giản (trong thực tế nên dùng JWT)
            var token = GenerateSimpleToken(userInfo);

            return new LoginResponse
            {
                Success = true,
                Message = "Đăng nhập thành công",
                UserInfo = userInfo,
                Token = token
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi đăng nhập: {TenDangNhap}", request.TenDangNhap);
            return new LoginResponse
            {
                Success = false,
                Message = "Có lỗi xảy ra khi đăng nhập"
            };
        }
    }

    public async Task<bool> LogoutAsync(string token)
    {
        try
        {
            // Trong thực tế nên có blacklist token hoặc invalidate token
            // Ở đây chỉ return true để đơn giản
            await Task.Delay(1); // Simulate async operation
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi đăng xuất");
            return false;
        }
    }

    public async Task<UserInfo?> GetUserInfoAsync(int userId, string userType)
    {
        try
        {
            if (userType == "SinhVien")
            {
                var sinhVien = await _context.SinhViens
                    .Include(sv => sv.TaiKhoan)
                    .FirstOrDefaultAsync(sv => sv.Id == userId);

                if (sinhVien?.TaiKhoan != null)
                {
                    return new UserInfo
                    {
                        Id = sinhVien.Id,
                        TenDangNhap = sinhVien.TaiKhoan.TenDangNhap,
                        LoaiTk = sinhVien.TaiKhoan.LoaiTk,
                        HoTen = sinhVien.HoTen,
                        Email = sinhVien.Email,
                        Sdt = sinhVien.Sdt
                    };
                }
            }
            else if (userType == "GiangVien")
            {
                var giangVien = await _context.GiangViens
                    .Include(gv => gv.TaiKhoan)
                    .FirstOrDefaultAsync(gv => gv.Id == userId);

                if (giangVien?.TaiKhoan != null)
                {
                    return new UserInfo
                    {
                        Id = giangVien.Id,
                        TenDangNhap = giangVien.TaiKhoan.TenDangNhap,
                        LoaiTk = giangVien.TaiKhoan.LoaiTk,
                        HoTen = giangVien.HoTen,
                        Email = giangVien.Email,
                        Sdt = giangVien.Sdt
                    };
                }
            }
            else if (userType == "Admin")
            {
                var taiKhoan = await _context.TaiKhoans
                    .FirstOrDefaultAsync(tk => tk.Id == userId && tk.LoaiTk == "Admin");

                if (taiKhoan != null)
                {
                    return new UserInfo
                    {
                        Id = taiKhoan.Id,
                        TenDangNhap = taiKhoan.TenDangNhap,
                        LoaiTk = taiKhoan.LoaiTk,
                        HoTen = "Administrator",
                        Email = null,
                        Sdt = null
                    };
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thông tin người dùng: {UserId}, {UserType}", userId, userType);
            return null;
        }
    }

    private string GenerateSimpleToken(UserInfo userInfo)
    {
        // Tạo token đơn giản bằng cách encode thông tin user
        var tokenData = $"{userInfo.Id}|{userInfo.LoaiTk}|{DateTime.UtcNow:yyyyMMddHHmmss}";
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tokenData));
    }
}
