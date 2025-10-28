using Microsoft.AspNetCore.Mvc;
using QLSV.Models.DTOs;
using QLSV.Services;

namespace QLSV.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentController> _logger;

    public StudentController(IStudentService studentService, ILogger<StudentController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    /// <summary>
    /// Xem thông tin cá nhân của sinh viên
    /// </summary>
    [HttpGet("{sinhVienId}/info")]
    public async Task<ActionResult<SinhVienInfoResponse>> GetSinhVienInfo(int sinhVienId)
    {
        try
        {
            var sinhVienInfo = await _studentService.GetSinhVienInfoAsync(sinhVienId);

            if (sinhVienInfo == null)
            {
                return NotFound(new { Message = "Không tìm thấy thông tin sinh viên" });
            }

            return Ok(sinhVienInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thông tin sinh viên: {SinhVienId}", sinhVienId);
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy thông tin sinh viên" });
        }
    }

    /// <summary>
    /// Xem danh sách lớp học phần đã đăng ký
    /// </summary>
    [HttpGet("{sinhVienId}/lop-hoc-phan")]
    public async Task<ActionResult<List<LopHocPhanSinhVienResponse>>> GetLopHocPhan(int sinhVienId)
    {
        try
        {
            var lopHocPhans = await _studentService.GetLopHocPhanCuaSinhVienAsync(sinhVienId);
            return Ok(lopHocPhans);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách lớp học phần: {SinhVienId}", sinhVienId);
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy danh sách lớp học phần" });
        }
    }

    /// <summary>
    /// Xem thời khóa biểu chi tiết
    /// </summary>
    [HttpGet("{sinhVienId}/thoi-khoa-bieu")]
    public async Task<ActionResult<List<ThoiKhoaBieuSinhVienResponse>>> GetThoiKhoaBieu(int sinhVienId)
    {
        try
        {
            var thoiKhoaBieu = await _studentService.GetThoiKhoaBieuAsync(sinhVienId);
            return Ok(thoiKhoaBieu);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thời khóa biểu: {SinhVienId}", sinhVienId);
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy thời khóa biểu" });
        }
    }

    /// <summary>
    /// Xem điểm số các môn học
    /// </summary>
    [HttpGet("{sinhVienId}/diem-so")]
    public async Task<ActionResult<List<DiemResponse>>> GetDiemSo(int sinhVienId)
    {
        try
        {
            var diems = await _studentService.GetDiemSoAsync(sinhVienId);
            return Ok(diems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy điểm số: {SinhVienId}", sinhVienId);
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy điểm số" });
        }
    }

    /// <summary>
    /// Xem thông báo chung và thông báo từ giảng viên
    /// </summary>
    [HttpGet("{sinhVienId}/thong-bao")]
    public async Task<ActionResult<List<ThongBaoResponse>>> GetThongBao(int sinhVienId)
    {
        try
        {
            var thongBaos = await _studentService.GetThongBaoAsync(sinhVienId);
            return Ok(thongBaos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thông báo: {SinhVienId}", sinhVienId);
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy thông báo" });
        }
    }
}
