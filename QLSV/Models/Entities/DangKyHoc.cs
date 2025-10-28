using System;
using System.Collections.Generic;

namespace QLSV.Models.Entities;

public partial class DangKyHoc
{
    public int Id { get; set; }

    public int SinhVienId { get; set; }

    public int LopHocPhanId { get; set; }

    public virtual Diem? Diem { get; set; }

    public virtual LopHocPhan LopHocPhan { get; set; } = null!;

    public virtual SinhVien SinhVien { get; set; } = null!;
}
