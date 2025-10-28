using System;
using System.Collections.Generic;

namespace QLSV.Models.Entities;

public partial class MonHoc
{
    public int Id { get; set; }

    public string MaMh { get; set; } = null!;

    public string TenMh { get; set; } = null!;

    public int? SoTinChi { get; set; }

    public virtual ICollection<LopHocPhan> LopHocPhans { get; set; } = new List<LopHocPhan>();
}
