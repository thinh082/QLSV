using System;
using System.Collections.Generic;

namespace QLSV.Models.Entities;

public partial class ThoiKhoaBieu
{
    public int Id { get; set; }

    public int LopHocPhanId { get; set; }

    public string Thu { get; set; } = null!;

    public int TietBatDau { get; set; }

    public int TietKetThuc { get; set; }

    public string? PhongHoc { get; set; }

    public virtual LopHocPhan LopHocPhan { get; set; } = null!;
}
