using Microsoft.AspNetCore.Mvc;
using QLSV.Models.DTOs;
using QLSV.Services;

namespace QLSV.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeacherController : ControllerBase
{
    private readonly ITeacherService _teacherService;
    private readonly ILogger<TeacherController> _logger;

    public TeacherController(ITeacherService teacherService, ILogger<TeacherController> logger)
    {
        _teacherService = teacherService;
        _logger = logger;
    }

    /// <summary>
    /// Xem thông tin cá nhân của giảng viên
    /// </summary>
    [HttpGet("{giangVienId}/info")]
    public async Task<ActionResult<GiangVienInfoResponse>> GetGiangVienInfo(int giangVienId)
    {
        try
        {
            var giangVienInfo = await _teacherService.GetGiangVienInfoAsync(giangVienId);

            if (giangVienInfo == null)
            {
                return NotFound(new { Message = "Không tìm thấy thông tin giảng viên" });
            }

            return Ok(giangVienInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thông tin giảng viên: {GiangVienId}", giangVienId);
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy thông tin giảng viên" });
        }
    }

    /// <summary>
    /// Xem danh sách lớp học phần được phân công
    /// </summary>
    [HttpGet("{giangVienId}/lop-hoc-phan")]
    public async Task<ActionResult<List<LopHocPhanGiangVienResponse>>> GetLopHocPhan(int giangVienId)
    {
        try
        {
            var lopHocPhans = await _teacherService.GetLopHocPhanCuaGiangVienAsync(giangVienId);
            return Ok(lopHocPhans);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách lớp học phần: {GiangVienId}", giangVienId);
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy danh sách lớp học phần" });
        }
    }

    /// <summary>
    /// Xem thời khóa biểu các lớp giảng dạy
    /// </summary>
    [HttpGet("{giangVienId}/thoi-khoa-bieu")]
    public async Task<ActionResult<List<ThoiKhoaBieuGiangVienResponse>>> GetThoiKhoaBieu(int giangVienId)
    {
        try
        {
            var thoiKhoaBieu = await _teacherService.GetThoiKhoaBieuAsync(giangVienId);
            return Ok(thoiKhoaBieu);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thời khóa biểu: {GiangVienId}", giangVienId);
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy thời khóa biểu" });
        }
    }

    /// <summary>
    /// Xem danh sách sinh viên và điểm trong lớp học phần
    /// </summary>
    [HttpGet("{giangVienId}/lop-hoc-phan/{lopHocPhanId}/sinh-vien-diem")]
    public async Task<ActionResult<List<SinhVienDiemResponse>>> GetSinhVienDiem(int giangVienId, int lopHocPhanId)
    {
        try
        {
            var sinhVienDiems = await _teacherService.GetSinhVienDiemAsync(giangVienId, lopHocPhanId);
            return Ok(sinhVienDiems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy điểm sinh viên: {GiangVienId}, {LopHocPhanId}", giangVienId, lopHocPhanId);
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy điểm sinh viên" });
        }
    }

    /// <summary>
    /// Nhập/cập nhật điểm cho sinh viên
    /// </summary>
    [HttpPut("cap-nhat-diem")]
    public async Task<ActionResult> CapNhatDiem([FromBody] CapNhatDiemRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dữ liệu đầu vào không hợp lệ" });
            }

            var success = await _teacherService.CapNhatDiemAsync(request);

            if (success)
            {
                return Ok(new { Success = true, Message = "Cập nhật điểm thành công" });
            }

            return BadRequest(new { Success = false, Message = "Cập nhật điểm thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật điểm");
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi cập nhật điểm" });
        }
    }

    /// <summary>
    /// Tạo thông báo cho lớp học phần
    /// </summary>
    [HttpPost("{giangVienId}/thong-bao")]
    public async Task<ActionResult> TaoThongBao(int giangVienId, [FromBody] TaoThongBaoRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dữ liệu đầu vào không hợp lệ" });
            }

            var success = await _teacherService.TaoThongBaoAsync(request, giangVienId);

            if (success)
            {
                return Ok(new { Success = true, Message = "Tạo thông báo thành công" });
            }

            return BadRequest(new { Success = false, Message = "Tạo thông báo thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo thông báo: {GiangVienId}", giangVienId);
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi tạo thông báo" });
        }
    }
}
