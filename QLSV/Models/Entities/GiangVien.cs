using System;
using System.Collections.Generic;

namespace QLSV.Models.Entities;

public partial class GiangVien
{
    public int Id { get; set; }

    public string MaGv { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string? Email { get; set; }

    public string? Sdt { get; set; }

    public int? TaiKhoanId { get; set; }

    public virtual ICollection<LopHocPhan> LopHocPhans { get; set; } = new List<LopHocPhan>();

    public virtual TaiKhoan? TaiKhoan { get; set; }

    public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
}
