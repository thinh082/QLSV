using Microsoft.EntityFrameworkCore;
using QLSV.Models.Entities;
using QLSV.Models.DTOs;

namespace QLSV.Services;

public interface IAdminService
{
    // Quản lý môn học
    Task<List<MonHocResponse>> GetAllMonHocAsync();
    Task<MonHocResponse?> GetMonHocByIdAsync(int id);
    Task<bool> CreateMonHocAsync(MonHocRequest request);
    Task<bool> UpdateMonHocAsync(int id, MonHocRequest request);
    Task<bool> DeleteMonHocAsync(int id);

    // Quản lý lớp học phần
    Task<List<LopHocPhanResponse>> GetAllLopHocPhanAsync();
    Task<LopHocPhanResponse?> GetLopHocPhanByIdAsync(int id);
    Task<bool> CreateLopHocPhanAsync(LopHocPhanRequest request);
    Task<bool> UpdateLopHocPhanAsync(int id, LopHocPhanRequest request);
    Task<bool> DeleteLopHocPhanAsync(int id);

    // Quản lý thời khóa biểu
    Task<List<ThoiKhoaBieuResponse>> GetAllThoiKhoaBieuAsync();
    Task<List<ThoiKhoaBieuResponse>> GetThoiKhoaBieuByLopHocPhanAsync(int lopHocPhanId);
    Task<bool> CreateThoiKhoaBieuAsync(ThoiKhoaBieuRequest request);
    Task<bool> UpdateThoiKhoaBieuAsync(int id, ThoiKhoaBieuRequest request);
    Task<bool> DeleteThoiKhoaBieuAsync(int id);

    // Quản lý đăng ký học
    Task<List<DangKyHocResponse>> GetAllDangKyHocAsync();
    Task<bool> CreateDangKyHocAsync(DangKyHocRequest request);
    Task<bool> DeleteDangKyHocAsync(int id);

    // Quản lý thông báo chung
    Task<bool> CreateThongBaoChungAsync(ThongBaoChungRequest request);
}

public class AdminService : IAdminService
{
    private readonly DKSVContext _context;
    private readonly ILogger<AdminService> _logger;

    public AdminService(DKSVContext context, ILogger<AdminService> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region Quản lý môn học

    public async Task<List<MonHocResponse>> GetAllMonHocAsync()
    {
        try
        {
            var monHocs = await _context.MonHocs
                .Select(mh => new MonHocResponse
                {
                    Id = mh.Id,
                    MaMh = mh.MaMh,
                    TenMh = mh.TenMh,
                    SoTinChi = mh.SoTinChi
                })
                .OrderBy(mh => mh.MaMh)
                .ToListAsync();

            return monHocs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách môn học");
            return new List<MonHocResponse>();
        }
    }

    public async Task<MonHocResponse?> GetMonHocByIdAsync(int id)
    {
        try
        {
            var monHoc = await _context.MonHocs
                .FirstOrDefaultAsync(mh => mh.Id == id);

            if (monHoc == null)
                return null;

            return new MonHocResponse
            {
                Id = monHoc.Id,
                MaMh = monHoc.MaMh,
                TenMh = monHoc.TenMh,
                SoTinChi = monHoc.SoTinChi
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy môn học: {Id}", id);
            return null;
        }
    }

    public async Task<bool> CreateMonHocAsync(MonHocRequest request)
    {
        try
        {
            var monHoc = new MonHoc
            {
                MaMh = request.MaMh,
                TenMh = request.TenMh,
                SoTinChi = request.SoTinChi
            };

            _context.MonHocs.Add(monHoc);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo môn học");
            return false;
        }
    }

    public async Task<bool> UpdateMonHocAsync(int id, MonHocRequest request)
    {
        try
        {
            var monHoc = await _context.MonHocs.FirstOrDefaultAsync(mh => mh.Id == id);

            if (monHoc == null)
                return false;

            monHoc.MaMh = request.MaMh;
            monHoc.TenMh = request.TenMh;
            monHoc.SoTinChi = request.SoTinChi;

            _context.MonHocs.Update(monHoc);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật môn học: {Id}", id);
            return false;
        }
    }

    public async Task<bool> DeleteMonHocAsync(int id)
    {
        try
        {
            var monHoc = await _context.MonHocs.FirstOrDefaultAsync(mh => mh.Id == id);

            if (monHoc == null)
                return false;

            _context.MonHocs.Remove(monHoc);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa môn học: {Id}", id);
            return false;
        }
    }

    #endregion

    #region Quản lý lớp học phần

    public async Task<List<LopHocPhanResponse>> GetAllLopHocPhanAsync()
    {
        try
        {
            var lopHocPhans = await _context.LopHocPhans
                .Include(lhp => lhp.MonHoc)
                .Include(lhp => lhp.GiangVien)
                .Select(lhp => new LopHocPhanResponse
                {
                    Id = lhp.Id,
                    MaLhp = lhp.MaLhp,
                    TenMh = lhp.MonHoc.TenMh,
                    TenGiangVien = lhp.GiangVien.HoTen,
                    HocKy = lhp.HocKy,
                    NamHoc = lhp.NamHoc,
                    SoTinChi = lhp.MonHoc.SoTinChi
                })
                .OrderBy(lhp => lhp.MaLhp)
                .ToListAsync();

            return lopHocPhans;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách lớp học phần");
            return new List<LopHocPhanResponse>();
        }
    }

    public async Task<LopHocPhanResponse?> GetLopHocPhanByIdAsync(int id)
    {
        try
        {
            var lopHocPhan = await _context.LopHocPhans
                .Include(lhp => lhp.MonHoc)
                .Include(lhp => lhp.GiangVien)
                .FirstOrDefaultAsync(lhp => lhp.Id == id);

            if (lopHocPhan == null)
                return null;

            return new LopHocPhanResponse
            {
                Id = lopHocPhan.Id,
                MaLhp = lopHocPhan.MaLhp,
                TenMh = lopHocPhan.MonHoc.TenMh,
                TenGiangVien = lopHocPhan.GiangVien.HoTen,
                HocKy = lopHocPhan.HocKy,
                NamHoc = lopHocPhan.NamHoc,
                SoTinChi = lopHocPhan.MonHoc.SoTinChi
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy lớp học phần: {Id}", id);
            return null;
        }
    }

    public async Task<bool> CreateLopHocPhanAsync(LopHocPhanRequest request)
    {
        try
        {
            var lopHocPhan = new LopHocPhan
            {
                MaLhp = request.MaLhp,
                MonHocId = request.MonHocId,
                GiangVienId = request.GiangVienId,
                HocKy = request.HocKy,
                NamHoc = request.NamHoc
            };

            _context.LopHocPhans.Add(lopHocPhan);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo lớp học phần");
            return false;
        }
    }

    public async Task<bool> UpdateLopHocPhanAsync(int id, LopHocPhanRequest request)
    {
        try
        {
            var lopHocPhan = await _context.LopHocPhans.FirstOrDefaultAsync(lhp => lhp.Id == id);

            if (lopHocPhan == null)
                return false;

            lopHocPhan.MaLhp = request.MaLhp;
            lopHocPhan.MonHocId = request.MonHocId;
            lopHocPhan.GiangVienId = request.GiangVienId;
            lopHocPhan.HocKy = request.HocKy;
            lopHocPhan.NamHoc = request.NamHoc;

            _context.LopHocPhans.Update(lopHocPhan);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật lớp học phần: {Id}", id);
            return false;
        }
    }

    public async Task<bool> DeleteLopHocPhanAsync(int id)
    {
        try
        {
            var lopHocPhan = await _context.LopHocPhans.FirstOrDefaultAsync(lhp => lhp.Id == id);

            if (lopHocPhan == null)
                return false;

            _context.LopHocPhans.Remove(lopHocPhan);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa lớp học phần: {Id}", id);
            return false;
        }
    }

    #endregion

    #region Quản lý thời khóa biểu

    public async Task<List<ThoiKhoaBieuResponse>> GetAllThoiKhoaBieuAsync()
    {
        try
        {
            var thoiKhoaBieu = await _context.ThoiKhoaBieus
                .Include(tkb => tkb.LopHocPhan)
                    .ThenInclude(lhp => lhp.MonHoc)
                .Select(tkb => new ThoiKhoaBieuResponse
                {
                    Id = tkb.Id,
                    MaLhp = tkb.LopHocPhan.MaLhp,
                    TenMh = tkb.LopHocPhan.MonHoc.TenMh,
                    Thu = tkb.Thu,
                    TietBatDau = tkb.TietBatDau,
                    TietKetThuc = tkb.TietKetThuc,
                    PhongHoc = tkb.PhongHoc
                })
                .OrderBy(tkb => tkb.Thu)
                .ThenBy(tkb => tkb.TietBatDau)
                .ToListAsync();

            return thoiKhoaBieu;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách thời khóa biểu");
            return new List<ThoiKhoaBieuResponse>();
        }
    }

    public async Task<List<ThoiKhoaBieuResponse>> GetThoiKhoaBieuByLopHocPhanAsync(int lopHocPhanId)
    {
        try
        {
            var thoiKhoaBieu = await _context.ThoiKhoaBieus
                .Where(tkb => tkb.LopHocPhanId == lopHocPhanId)
                .Include(tkb => tkb.LopHocPhan)
                    .ThenInclude(lhp => lhp.MonHoc)
                .Select(tkb => new ThoiKhoaBieuResponse
                {
                    Id = tkb.Id,
                    MaLhp = tkb.LopHocPhan.MaLhp,
                    TenMh = tkb.LopHocPhan.MonHoc.TenMh,
                    Thu = tkb.Thu,
                    TietBatDau = tkb.TietBatDau,
                    TietKetThuc = tkb.TietKetThuc,
                    PhongHoc = tkb.PhongHoc
                })
                .OrderBy(tkb => tkb.Thu)
                .ThenBy(tkb => tkb.TietBatDau)
                .ToListAsync();

            return thoiKhoaBieu;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thời khóa biểu theo lớp học phần: {LopHocPhanId}", lopHocPhanId);
            return new List<ThoiKhoaBieuResponse>();
        }
    }

    public async Task<bool> CreateThoiKhoaBieuAsync(ThoiKhoaBieuRequest request)
    {
        try
        {
            var thoiKhoaBieu = new ThoiKhoaBieu
            {
                LopHocPhanId = request.LopHocPhanId,
                Thu = request.Thu,
                TietBatDau = request.TietBatDau,
                TietKetThuc = request.TietKetThuc,
                PhongHoc = request.PhongHoc
            };

            _context.ThoiKhoaBieus.Add(thoiKhoaBieu);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo thời khóa biểu");
            return false;
        }
    }

    public async Task<bool> UpdateThoiKhoaBieuAsync(int id, ThoiKhoaBieuRequest request)
    {
        try
        {
            var thoiKhoaBieu = await _context.ThoiKhoaBieus.FirstOrDefaultAsync(tkb => tkb.Id == id);

            if (thoiKhoaBieu == null)
                return false;

            thoiKhoaBieu.LopHocPhanId = request.LopHocPhanId;
            thoiKhoaBieu.Thu = request.Thu;
            thoiKhoaBieu.TietBatDau = request.TietBatDau;
            thoiKhoaBieu.TietKetThuc = request.TietKetThuc;
            thoiKhoaBieu.PhongHoc = request.PhongHoc;

            _context.ThoiKhoaBieus.Update(thoiKhoaBieu);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật thời khóa biểu: {Id}", id);
            return false;
        }
    }

    public async Task<bool> DeleteThoiKhoaBieuAsync(int id)
    {
        try
        {
            var thoiKhoaBieu = await _context.ThoiKhoaBieus.FirstOrDefaultAsync(tkb => tkb.Id == id);

            if (thoiKhoaBieu == null)
                return false;

            _context.ThoiKhoaBieus.Remove(thoiKhoaBieu);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa thời khóa biểu: {Id}", id);
            return false;
        }
    }

    #endregion

    #region Quản lý đăng ký học

    public async Task<List<DangKyHocResponse>> GetAllDangKyHocAsync()
    {
        try
        {
            var dangKyHocs = await _context.DangKyHocs
                .Include(dk => dk.SinhVien)
                .Include(dk => dk.LopHocPhan)
                    .ThenInclude(lhp => lhp.MonHoc)
                .Include(dk => dk.LopHocPhan)
                    .ThenInclude(lhp => lhp.GiangVien)
                .Select(dk => new DangKyHocResponse
                {
                    Id = dk.Id,
                    MaSv = dk.SinhVien.MaSv,
                    HoTenSinhVien = dk.SinhVien.HoTen,
                    MaLhp = dk.LopHocPhan.MaLhp,
                    TenMh = dk.LopHocPhan.MonHoc.TenMh,
                    TenGiangVien = dk.LopHocPhan.GiangVien.HoTen
                })
                .OrderBy(dk => dk.MaSv)
                .ToListAsync();

            return dangKyHocs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách đăng ký học");
            return new List<DangKyHocResponse>();
        }
    }

    public async Task<bool> CreateDangKyHocAsync(DangKyHocRequest request)
    {
        try
        {
            // Kiểm tra sinh viên đã đăng ký lớp này chưa
            var existingDangKy = await _context.DangKyHocs
                .FirstOrDefaultAsync(dk => dk.SinhVienId == request.SinhVienId && dk.LopHocPhanId == request.LopHocPhanId);

            if (existingDangKy != null)
                return false;

            var dangKyHoc = new DangKyHoc
            {
                SinhVienId = request.SinhVienId,
                LopHocPhanId = request.LopHocPhanId
            };

            _context.DangKyHocs.Add(dangKyHoc);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo đăng ký học");
            return false;
        }
    }

    public async Task<bool> DeleteDangKyHocAsync(int id)
    {
        try
        {
            var dangKyHoc = await _context.DangKyHocs.FirstOrDefaultAsync(dk => dk.Id == id);

            if (dangKyHoc == null)
                return false;

            _context.DangKyHocs.Remove(dangKyHoc);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa đăng ký học: {Id}", id);
            return false;
        }
    }

    #endregion

    #region Quản lý thông báo chung

    public async Task<bool> CreateThongBaoChungAsync(ThongBaoChungRequest request)
    {
        try
        {
            var thongBao = new ThongBao
            {
                TieuDe = request.TieuDe,
                NoiDung = request.NoiDung,
                LoaiTb = request.LoaiTb,
                NgayTao = DateTime.Now,
                LopHocPhanId = null, // Thông báo chung không liên kết lớp cụ thể
                GiangVienId = null   // Thông báo chung không liên kết giảng viên cụ thể
            };

            _context.ThongBaos.Add(thongBao);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo thông báo chung");
            return false;
        }
    }

    #endregion
}
