using System.ComponentModel.DataAnnotations;

namespace QLSV.Models.DTOs;

public class MonHocRequest
{
    [Required(ErrorMessage = "Mã môn học là bắt buộc")]
    [MaxLength(10, ErrorMessage = "Mã môn học không được quá 10 ký tự")]
    public string MaMh { get; set; } = null!;

    [Required(ErrorMessage = "Tên môn học là bắt buộc")]
    [MaxLength(50, ErrorMessage = "Tên môn học không được quá 50 ký tự")]
    public string TenMh { get; set; } = null!;

    [Range(1, 10, ErrorMessage = "Số tín chỉ phải từ 1 đến 10")]
    public int? SoTinChi { get; set; }
}

public class MonHocResponse
{
    public int Id { get; set; }
    public string MaMh { get; set; } = null!;
    public string TenMh { get; set; } = null!;
    public int? SoTinChi { get; set; }
}

public class LopHocPhanRequest
{
    [Required(ErrorMessage = "Mã lớp học phần là bắt buộc")]
    [MaxLength(10, ErrorMessage = "Mã lớp học phần không được quá 10 ký tự")]
    public string MaLhp { get; set; } = null!;

    [Required(ErrorMessage = "Môn học là bắt buộc")]
    public int MonHocId { get; set; }

    [Required(ErrorMessage = "Giảng viên là bắt buộc")]
    public int GiangVienId { get; set; }

    [MaxLength(20, ErrorMessage = "Học kỳ không được quá 20 ký tự")]
    public string? HocKy { get; set; }

    [MaxLength(10, ErrorMessage = "Năm học không được quá 10 ký tự")]
    public string? NamHoc { get; set; }
}

public class LopHocPhanResponse
{
    public int Id { get; set; }
    public string MaLhp { get; set; } = null!;
    public string TenMh { get; set; } = null!;
    public string TenGiangVien { get; set; } = null!;
    public string? HocKy { get; set; }
    public string? NamHoc { get; set; }
    public int? SoTinChi { get; set; }
}

public class ThoiKhoaBieuRequest
{
    [Required(ErrorMessage = "Lớp học phần là bắt buộc")]
    public int LopHocPhanId { get; set; }

    [Required(ErrorMessage = "Thứ là bắt buộc")]
    [MaxLength(10, ErrorMessage = "Thứ không được quá 10 ký tự")]
    public string Thu { get; set; } = null!;

    [Required(ErrorMessage = "Tiết bắt đầu là bắt buộc")]
    [Range(1, 12, ErrorMessage = "Tiết bắt đầu phải từ 1 đến 12")]
    public int TietBatDau { get; set; }

    [Required(ErrorMessage = "Tiết kết thúc là bắt buộc")]
    [Range(1, 12, ErrorMessage = "Tiết kết thúc phải từ 1 đến 12")]
    public int TietKetThuc { get; set; }

    [MaxLength(20, ErrorMessage = "Phòng học không được quá 20 ký tự")]
    public string? PhongHoc { get; set; }
}

public class ThoiKhoaBieuResponse
{
    public int Id { get; set; }
    public string MaLhp { get; set; } = null!;
    public string TenMh { get; set; } = null!;
    public string Thu { get; set; } = null!;
    public int TietBatDau { get; set; }
    public int TietKetThuc { get; set; }
    public string? PhongHoc { get; set; }
}

public class DangKyHocRequest
{
    [Required(ErrorMessage = "Sinh viên là bắt buộc")]
    public int SinhVienId { get; set; }

    [Required(ErrorMessage = "Lớp học phần là bắt buộc")]
    public int LopHocPhanId { get; set; }
}

public class DangKyHocResponse
{
    public int Id { get; set; }
    public string MaSv { get; set; } = null!;
    public string HoTenSinhVien { get; set; } = null!;
    public string MaLhp { get; set; } = null!;
    public string TenMh { get; set; } = null!;
    public string TenGiangVien { get; set; } = null!;
}

public class ThongBaoChungRequest
{
    [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
    [MaxLength(50, ErrorMessage = "Tiêu đề không được quá 50 ký tự")]
    public string TieuDe { get; set; } = null!;

    [MaxLength(500, ErrorMessage = "Nội dung không được quá 500 ký tự")]
    public string? NoiDung { get; set; }

    [MaxLength(20, ErrorMessage = "Loại thông báo không được quá 20 ký tự")]
    public string? LoaiTb { get; set; }

    [Required(ErrorMessage = "Đối tượng nhận thông báo là bắt buộc")]
    public string DoiTuong { get; set; } = null!; // "SinhVien" hoặc "GiangVien"
}
