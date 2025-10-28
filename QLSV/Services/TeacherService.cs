using Microsoft.EntityFrameworkCore;
using QLSV.Models.Entities;
using QLSV.Models.DTOs;

namespace QLSV.Services;

public interface ITeacherService
{
    Task<GiangVienInfoResponse?> GetGiangVienInfoAsync(int giangVienId);
    Task<List<LopHocPhanGiangVienResponse>> GetLopHocPhanCuaGiangVienAsync(int giangVienId);
    Task<List<ThoiKhoaBieuGiangVienResponse>> GetThoiKhoaBieuAsync(int giangVienId);
    Task<List<SinhVienDiemResponse>> GetSinhVienDiemAsync(int giangVienId, int lopHocPhanId);
    Task<bool> CapNhatDiemAsync(CapNhatDiemRequest request);
    Task<bool> TaoThongBaoAsync(TaoThongBaoRequest request, int giangVienId);
}

public class TeacherService : ITeacherService
{
    private readonly DKSVContext _context;
    private readonly ILogger<TeacherService> _logger;

    public TeacherService(DKSVContext context, ILogger<TeacherService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GiangVienInfoResponse?> GetGiangVienInfoAsync(int giangVienId)
    {
        try
        {
            var giangVien = await _context.GiangViens
                .FirstOrDefaultAsync(gv => gv.Id == giangVienId);

            if (giangVien == null)
                return null;

            return new GiangVienInfoResponse
            {
                Id = giangVien.Id,
                MaGv = giangVien.MaGv,
                HoTen = giangVien.HoTen,
                Email = giangVien.Email,
                Sdt = giangVien.Sdt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thông tin giảng viên: {GiangVienId}", giangVienId);
            return null;
        }
    }

    public async Task<List<LopHocPhanGiangVienResponse>> GetLopHocPhanCuaGiangVienAsync(int giangVienId)
    {
        try
        {
            var lopHocPhans = await _context.LopHocPhans
                .Where(lhp => lhp.GiangVienId == giangVienId)
                .Include(lhp => lhp.MonHoc)
                .Include(lhp => lhp.DangKyHocs)
                .Select(lhp => new LopHocPhanGiangVienResponse
                {
                    Id = lhp.Id,
                    MaLhp = lhp.MaLhp,
                    TenMh = lhp.MonHoc.TenMh,
                    HocKy = lhp.HocKy,
                    NamHoc = lhp.NamHoc,
                    SoTinChi = lhp.MonHoc.SoTinChi,
                    SoSinhVien = lhp.DangKyHocs.Count
                })
                .ToListAsync();

            return lopHocPhans;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách lớp học phần của giảng viên: {GiangVienId}", giangVienId);
            return new List<LopHocPhanGiangVienResponse>();
        }
    }

    public async Task<List<ThoiKhoaBieuGiangVienResponse>> GetThoiKhoaBieuAsync(int giangVienId)
    {
        try
        {
            var thoiKhoaBieu = await _context.LopHocPhans
                .Where(lhp => lhp.GiangVienId == giangVienId)
                .Include(lhp => lhp.ThoiKhoaBieus)
                .Include(lhp => lhp.MonHoc)
                .SelectMany(lhp => lhp.ThoiKhoaBieus.Select(tkb => new ThoiKhoaBieuGiangVienResponse
                {
                    Id = tkb.Id,
                    MaLhp = lhp.MaLhp,
                    TenMh = lhp.MonHoc.TenMh,
                    Thu = tkb.Thu,
                    TietBatDau = tkb.TietBatDau,
                    TietKetThuc = tkb.TietKetThuc,
                    PhongHoc = tkb.PhongHoc
                }))
                .OrderBy(tkb => tkb.Thu)
                .ThenBy(tkb => tkb.TietBatDau)
                .ToListAsync();

            return thoiKhoaBieu;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thời khóa biểu của giảng viên: {GiangVienId}", giangVienId);
            return new List<ThoiKhoaBieuGiangVienResponse>();
        }
    }

    public async Task<List<SinhVienDiemResponse>> GetSinhVienDiemAsync(int giangVienId, int lopHocPhanId)
    {
        try
        {
            // Kiểm tra giảng viên có quyền truy cập lớp học phần này không
            var lopHocPhan = await _context.LopHocPhans
                .FirstOrDefaultAsync(lhp => lhp.Id == lopHocPhanId && lhp.GiangVienId == giangVienId);

            if (lopHocPhan == null)
            {
                return new List<SinhVienDiemResponse>();
            }

            var sinhVienDiems = await _context.DangKyHocs
                .Where(dk => dk.LopHocPhanId == lopHocPhanId)
                .Include(dk => dk.SinhVien)
                .Include(dk => dk.Diem)
                .Select(dk => new SinhVienDiemResponse
                {
                    Id = dk.Diem != null ? dk.Diem.Id : 0,
                    MaSv = dk.SinhVien.MaSv,
                    HoTen = dk.SinhVien.HoTen,
                    DiemTp = dk.Diem != null ? dk.Diem.DiemTp : null,
                    DiemGk = dk.Diem != null ? dk.Diem.DiemGk : null,
                    DiemCk = dk.Diem != null ? dk.Diem.DiemCk : null,
                    DiemTong = dk.Diem != null ? dk.Diem.DiemTong : null
                })
                .OrderBy(sv => sv.MaSv)
                .ToListAsync();

            return sinhVienDiems;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy điểm sinh viên: {GiangVienId}, {LopHocPhanId}", giangVienId, lopHocPhanId);
            return new List<SinhVienDiemResponse>();
        }
    }

    public async Task<bool> CapNhatDiemAsync(CapNhatDiemRequest request)
    {
        try
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var dangKyHoc = await _context.DangKyHocs
                .Include(dk => dk.Diem)
                .FirstOrDefaultAsync(dk => dk.Id == request.DangKyHocId);

            if (dangKyHoc == null)
            {
                return false;
            }

            if (dangKyHoc.Diem == null)
            {
                // Tạo mới điểm
                var diem = new Diem
                {
                    DangKyHocId = request.DangKyHocId,
                    DiemTp = request.DiemTp,
                    DiemGk = request.DiemGk,
                    DiemCk = request.DiemCk,
                    DiemTong = CalculateDiemTong(request.DiemTp, request.DiemGk, request.DiemCk)
                };

                _context.Diems.Add(diem);
            }
            else
            {
                // Cập nhật điểm hiện có
                dangKyHoc.Diem.DiemTp = request.DiemTp;
                dangKyHoc.Diem.DiemGk = request.DiemGk;
                dangKyHoc.Diem.DiemCk = request.DiemCk;
                dangKyHoc.Diem.DiemTong = CalculateDiemTong(request.DiemTp, request.DiemGk, request.DiemCk);

                _context.Diems.Update(dangKyHoc.Diem);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật điểm: {DangKyHocId}", request.DangKyHocId);
            return false;
        }
    }

    public async Task<bool> TaoThongBaoAsync(TaoThongBaoRequest request, int giangVienId)
    {
        try
        {
            var thongBao = new ThongBao
            {
                TieuDe = request.TieuDe,
                NoiDung = request.NoiDung,
                LoaiTb = request.LoaiTb,
                LopHocPhanId = request.LopHocPhanId,
                GiangVienId = giangVienId,
                NgayTao = DateTime.Now
            };

            _context.ThongBaos.Add(thongBao);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo thông báo: {GiangVienId}", giangVienId);
            return false;
        }
    }

    private double? CalculateDiemTong(double? diemTp, double? diemGk, double? diemCk)
    {
        // Công thức tính điểm tổng: TP*0.3 + GK*0.3 + CK*0.4
        if (diemTp.HasValue && diemGk.HasValue && diemCk.HasValue)
        {
            return Math.Round(diemTp.Value * 0.3 + diemGk.Value * 0.3 + diemCk.Value * 0.4, 2);
        }
        return null;
    }
}
