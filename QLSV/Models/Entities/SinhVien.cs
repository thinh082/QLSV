using System;
using System.Collections.Generic;

namespace QLSV.Models.Entities;

public partial class SinhVien
{
    public int Id { get; set; }

    public string MaSv { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public string? Email { get; set; }

    public string? Sdt { get; set; }

    public int? TaiKhoanId { get; set; }

    public virtual ICollection<DangKyHoc> DangKyHocs { get; set; } = new List<DangKyHoc>();

    public virtual TaiKhoan? TaiKhoan { get; set; }
}
