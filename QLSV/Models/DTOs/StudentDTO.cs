using System.ComponentModel.DataAnnotations;

namespace QLSV.Models.DTOs;

public class SinhVienInfoResponse
{
    public int Id { get; set; }
    public string MaSv { get; set; } = null!;
    public string HoTen { get; set; } = null!;
    public DateOnly? NgaySinh { get; set; }
    public string? GioiTinh { get; set; }
    public string? Email { get; set; }
    public string? Sdt { get; set; }
}

public class LopHocPhanSinhVienResponse
{
    public int Id { get; set; }
    public string MaLhp { get; set; } = null!;
    public string TenMh { get; set; } = null!;
    public string TenGiangVien { get; set; } = null!;
    public string? HocKy { get; set; }
    public string? NamHoc { get; set; }
    public int? SoTinChi { get; set; }
}

public class ThoiKhoaBieuSinhVienResponse
{
    public int Id { get; set; }
    public string MaLhp { get; set; } = null!;
    public string TenMh { get; set; } = null!;
    public string Thu { get; set; } = null!;
    public int TietBatDau { get; set; }
    public int TietKetThuc { get; set; }
    public string? PhongHoc { get; set; }
}

public class DiemResponse
{
    public int Id { get; set; }
    public string MaLhp { get; set; } = null!;
    public string TenMh { get; set; } = null!;
    public double? DiemTp { get; set; }
    public double? DiemGk { get; set; }
    public double? DiemCk { get; set; }
    public double? DiemTong { get; set; }
}

public class ThongBaoResponse
{
    public int Id { get; set; }
    public string TieuDe { get; set; } = null!;
    public string? NoiDung { get; set; }
    public DateTime? NgayTao { get; set; }
    public string? LoaiTb { get; set; }
    public string? TenGiangVien { get; set; }
    public string? TenLopHocPhan { get; set; }
}
