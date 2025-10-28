using System;
using System.Collections.Generic;

namespace QLSV.Models.Entities;

public partial class Diem
{
    public int Id { get; set; }

    public int DangKyHocId { get; set; }

    public double? DiemTp { get; set; }

    public double? DiemGk { get; set; }

    public double? DiemCk { get; set; }

    public double? DiemTong { get; set; }

    public virtual DangKyHoc DangKyHoc { get; set; } = null!;
}
