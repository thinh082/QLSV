using System;
using System.Collections.Generic;

namespace QLSV.Models.Entities;

public partial class ThongBao
{
    public int Id { get; set; }

    public string TieuDe { get; set; } = null!;

    public string? NoiDung { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? LoaiTb { get; set; }

    public int? LopHocPhanId { get; set; }

    public int? GiangVienId { get; set; }

    public virtual GiangVien? GiangVien { get; set; }

    public virtual LopHocPhan? LopHocPhan { get; set; }
}
