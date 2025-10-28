using System;
using System.Collections.Generic;

namespace QLSV.Models.Entities;

public partial class LopHocPhan
{
    public int Id { get; set; }

    public string MaLhp { get; set; } = null!;

    public int MonHocId { get; set; }

    public int GiangVienId { get; set; }

    public string? HocKy { get; set; }

    public string? NamHoc { get; set; }

    public virtual ICollection<DangKyHoc> DangKyHocs { get; set; } = new List<DangKyHoc>();

    public virtual GiangVien GiangVien { get; set; } = null!;

    public virtual MonHoc MonHoc { get; set; } = null!;

    public virtual ICollection<ThoiKhoaBieu> ThoiKhoaBieus { get; set; } = new List<ThoiKhoaBieu>();

    public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
}
