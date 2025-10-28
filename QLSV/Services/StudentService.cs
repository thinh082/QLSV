using Microsoft.EntityFrameworkCore;
using QLSV.Models.Entities;
using QLSV.Models.DTOs;

namespace QLSV.Services;

public interface IStudentService
{
    Task<SinhVienInfoResponse?> GetSinhVienInfoAsync(int sinhVienId);
    Task<List<LopHocPhanSinhVienResponse>> GetLopHocPhanCuaSinhVienAsync(int sinhVienId);
    Task<List<ThoiKhoaBieuSinhVienResponse>> GetThoiKhoaBieuAsync(int sinhVienId);
    Task<List<DiemResponse>> GetDiemSoAsync(int sinhVienId);
    Task<List<ThongBaoResponse>> GetThongBaoAsync(int sinhVienId);
}

public class StudentService : IStudentService
{
    private readonly DKSVContext _context;
    private readonly ILogger<StudentService> _logger;

    public StudentService(DKSVContext context, ILogger<StudentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<SinhVienInfoResponse?> GetSinhVienInfoAsync(int sinhVienId)
    {
        try
        {
            var sinhVien = await _context.SinhViens
                .FirstOrDefaultAsync(sv => sv.Id == sinhVienId);

            if (sinhVien == null)
                return null;

            return new SinhVienInfoResponse
            {
                Id = sinhVien.Id,
                MaSv = sinhVien.MaSv,
                HoTen = sinhVien.HoTen,
                NgaySinh = sinhVien.NgaySinh,
                GioiTinh = sinhVien.GioiTinh,
                Email = sinhVien.Email,
                Sdt = sinhVien.Sdt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thông tin sinh viên: {SinhVienId}", sinhVienId);
            return null;
        }
    }

    public async Task<List<LopHocPhanSinhVienResponse>> GetLopHocPhanCuaSinhVienAsync(int sinhVienId)
    {
        try
        {
            var lopHocPhans = await _context.DangKyHocs
                .Where(dk => dk.SinhVienId == sinhVienId)
                .Include(dk => dk.LopHocPhan)
                    .ThenInclude(lhp => lhp.MonHoc)
                .Include(dk => dk.LopHocPhan)
                    .ThenInclude(lhp => lhp.GiangVien)
                .Select(dk => new LopHocPhanSinhVienResponse
                {
                    Id = dk.LopHocPhan.Id,
                    MaLhp = dk.LopHocPhan.MaLhp,
                    TenMh = dk.LopHocPhan.MonHoc.TenMh,
                    TenGiangVien = dk.LopHocPhan.GiangVien.HoTen,
                    HocKy = dk.LopHocPhan.HocKy,
                    NamHoc = dk.LopHocPhan.NamHoc,
                    SoTinChi = dk.LopHocPhan.MonHoc.SoTinChi
                })
                .ToListAsync();

            return lopHocPhans;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách lớp học phần của sinh viên: {SinhVienId}", sinhVienId);
            return new List<LopHocPhanSinhVienResponse>();
        }
    }

    public async Task<List<ThoiKhoaBieuSinhVienResponse>> GetThoiKhoaBieuAsync(int sinhVienId)
    {
        try
        {
            var thoiKhoaBieu = await _context.DangKyHocs
                .Where(dk => dk.SinhVienId == sinhVienId)
                .Include(dk => dk.LopHocPhan)
                    .ThenInclude(lhp => lhp.ThoiKhoaBieus)
                .Include(dk => dk.LopHocPhan)
                    .ThenInclude(lhp => lhp.MonHoc)
                .SelectMany(dk => dk.LopHocPhan.ThoiKhoaBieus.Select(tkb => new ThoiKhoaBieuSinhVienResponse
                {
                    Id = tkb.Id,
                    MaLhp = dk.LopHocPhan.MaLhp,
                    TenMh = dk.LopHocPhan.MonHoc.TenMh,
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
            _logger.LogError(ex, "Lỗi khi lấy thời khóa biểu của sinh viên: {SinhVienId}", sinhVienId);
            return new List<ThoiKhoaBieuSinhVienResponse>();
        }
    }

    public async Task<List<DiemResponse>> GetDiemSoAsync(int sinhVienId)
    {
        try
        {
            var diems = await _context.DangKyHocs
                .Where(dk => dk.SinhVienId == sinhVienId)
                .Include(dk => dk.Diem)
                .Include(dk => dk.LopHocPhan)
                    .ThenInclude(lhp => lhp.MonHoc)
                .Select(dk => new DiemResponse
                {
                    Id = dk.Diem != null ? dk.Diem.Id : 0,
                    MaLhp = dk.LopHocPhan.MaLhp,
                    TenMh = dk.LopHocPhan.MonHoc.TenMh,
                    DiemTp = dk.Diem != null ? dk.Diem.DiemTp : null,
                    DiemGk = dk.Diem != null ? dk.Diem.DiemGk : null,
                    DiemCk = dk.Diem != null ? dk.Diem.DiemCk : null,
                    DiemTong = dk.Diem != null ? dk.Diem.DiemTong : null
                })
                .ToListAsync();

            return diems;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy điểm số của sinh viên: {SinhVienId}", sinhVienId);
            return new List<DiemResponse>();
        }
    }

    public async Task<List<ThongBaoResponse>> GetThongBaoAsync(int sinhVienId)
    {
        try
        {
            // Lấy danh sách lớp học phần của sinh viên
            var lopHocPhanIds = await _context.DangKyHocs
                .Where(dk => dk.SinhVienId == sinhVienId)
                .Select(dk => dk.LopHocPhanId)
                .ToListAsync();

            var thongBaos = await _context.ThongBaos
                .Where(tb => tb.LopHocPhanId == null || lopHocPhanIds.Contains(tb.LopHocPhanId.Value))
                .Include(tb => tb.GiangVien)
                .Include(tb => tb.LopHocPhan)
                    .ThenInclude(lhp => lhp!.MonHoc)
                .Select(tb => new ThongBaoResponse
                {
                    Id = tb.Id,
                    TieuDe = tb.TieuDe,
                    NoiDung = tb.NoiDung,
                    NgayTao = tb.NgayTao,
                    LoaiTb = tb.LoaiTb,
                    TenGiangVien = tb.GiangVien != null ? tb.GiangVien.HoTen : null,
                    TenLopHocPhan = tb.LopHocPhan != null ? tb.LopHocPhan.MonHoc.TenMh : null
                })
                .OrderByDescending(tb => tb.NgayTao)
                .ToListAsync();

            return thongBaos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thông báo của sinh viên: {SinhVienId}", sinhVienId);
            return new List<ThongBaoResponse>();
        }
    }
}
