using System.ComponentModel.DataAnnotations;

namespace QLSV.Models.DTOs;

public class GiangVienInfoResponse
{
    public int Id { get; set; }
    public string MaGv { get; set; } = null!;
    public string HoTen { get; set; } = null!;
    public string? Email { get; set; }
    public string? Sdt { get; set; }
}

public class LopHocPhanGiangVienResponse
{
    public int Id { get; set; }
    public string MaLhp { get; set; } = null!;
    public string TenMh { get; set; } = null!;
    public string? HocKy { get; set; }
    public string? NamHoc { get; set; }
    public int? SoTinChi { get; set; }
    public int SoSinhVien { get; set; }
}

public class ThoiKhoaBieuGiangVienResponse
{
    public int Id { get; set; }
    public string MaLhp { get; set; } = null!;
    public string TenMh { get; set; } = null!;
    public string Thu { get; set; } = null!;
    public int TietBatDau { get; set; }
    public int TietKetThuc { get; set; }
    public string? PhongHoc { get; set; }
}

public class SinhVienDiemResponse
{
    public int Id { get; set; }
    public string MaSv { get; set; } = null!;
    public string HoTen { get; set; } = null!;
    public double? DiemTp { get; set; }
    public double? DiemGk { get; set; }
    public double? DiemCk { get; set; }
    public double? DiemTong { get; set; }
}

public class CapNhatDiemRequest
{
    [Required(ErrorMessage = "ID đăng ký học là bắt buộc")]
    public int DangKyHocId { get; set; }

    [Range(0, 10, ErrorMessage = "Điểm thành phần phải từ 0 đến 10")]
    public double? DiemTp { get; set; }

    [Range(0, 10, ErrorMessage = "Điểm giữa kỳ phải từ 0 đến 10")]
    public double? DiemGk { get; set; }

    [Range(0, 10, ErrorMessage = "Điểm cuối kỳ phải từ 0 đến 10")]
    public double? DiemCk { get; set; }
}

public class TaoThongBaoRequest
{
    [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
    [MaxLength(50, ErrorMessage = "Tiêu đề không được quá 50 ký tự")]
    public string TieuDe { get; set; } = null!;

    [MaxLength(500, ErrorMessage = "Nội dung không được quá 500 ký tự")]
    public string? NoiDung { get; set; }

    [MaxLength(20, ErrorMessage = "Loại thông báo không được quá 20 ký tự")]
    public string? LoaiTb { get; set; }

    public int? LopHocPhanId { get; set; }
}
