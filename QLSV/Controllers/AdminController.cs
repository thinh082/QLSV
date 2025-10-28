using Microsoft.AspNetCore.Mvc;
using QLSV.Models.DTOs;
using QLSV.Services;

namespace QLSV.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IAdminService adminService, ILogger<AdminController> logger)
    {
        _adminService = adminService;
        _logger = logger;
    }

    #region Quản lý môn học

    /// <summary>
    /// Lấy danh sách tất cả môn học
    /// </summary>
    [HttpGet("mon-hoc")]
    public async Task<ActionResult<List<MonHocResponse>>> GetAllMonHoc()
    {
        try
        {
            var monHocs = await _adminService.GetAllMonHocAsync();
            return Ok(monHocs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách môn học");
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy danh sách môn học" });
        }
    }

    /// <summary>
    /// Lấy thông tin môn học theo ID
    /// </summary>
    [HttpGet("mon-hoc/{id}")]
    public async Task<ActionResult<MonHocResponse>> GetMonHocById(int id)
    {
        try
        {
            var monHoc = await _adminService.GetMonHocByIdAsync(id);

            if (monHoc == null)
            {
                return NotFound(new { Message = "Không tìm thấy môn học" });
            }

            return Ok(monHoc);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy môn học: {Id}", id);
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy môn học" });
        }
    }

    /// <summary>
    /// Tạo môn học mới
    /// </summary>
    [HttpPost("mon-hoc")]
    public async Task<ActionResult> CreateMonHoc([FromBody] MonHocRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dữ liệu đầu vào không hợp lệ" });
            }

            var success = await _adminService.CreateMonHocAsync(request);

            if (success)
            {
                return Ok(new { Success = true, Message = "Tạo môn học thành công" });
            }

            return BadRequest(new { Success = false, Message = "Tạo môn học thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo môn học");
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi tạo môn học" });
        }
    }

    /// <summary>
    /// Cập nhật môn học
    /// </summary>
    [HttpPut("mon-hoc/{id}")]
    public async Task<ActionResult> UpdateMonHoc(int id, [FromBody] MonHocRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dữ liệu đầu vào không hợp lệ" });
            }

            var success = await _adminService.UpdateMonHocAsync(id, request);

            if (success)
            {
                return Ok(new { Success = true, Message = "Cập nhật môn học thành công" });
            }

            return BadRequest(new { Success = false, Message = "Cập nhật môn học thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật môn học: {Id}", id);
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi cập nhật môn học" });
        }
    }

    /// <summary>
    /// Xóa môn học
    /// </summary>
    [HttpDelete("mon-hoc/{id}")]
    public async Task<ActionResult> DeleteMonHoc(int id)
    {
        try
        {
            var success = await _adminService.DeleteMonHocAsync(id);

            if (success)
            {
                return Ok(new { Success = true, Message = "Xóa môn học thành công" });
            }

            return BadRequest(new { Success = false, Message = "Xóa môn học thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa môn học: {Id}", id);
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi xóa môn học" });
        }
    }

    #endregion

    #region Quản lý lớp học phần

    /// <summary>
    /// Lấy danh sách tất cả lớp học phần
    /// </summary>
    [HttpGet("lop-hoc-phan")]
    public async Task<ActionResult<List<LopHocPhanResponse>>> GetAllLopHocPhan()
    {
        try
        {
            var lopHocPhans = await _adminService.GetAllLopHocPhanAsync();
            return Ok(lopHocPhans);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách lớp học phần");
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy danh sách lớp học phần" });
        }
    }

    /// <summary>
    /// Tạo lớp học phần mới
    /// </summary>
    [HttpPost("lop-hoc-phan")]
    public async Task<ActionResult> CreateLopHocPhan([FromBody] LopHocPhanRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dữ liệu đầu vào không hợp lệ" });
            }

            var success = await _adminService.CreateLopHocPhanAsync(request);

            if (success)
            {
                return Ok(new { Success = true, Message = "Tạo lớp học phần thành công" });
            }

            return BadRequest(new { Success = false, Message = "Tạo lớp học phần thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo lớp học phần");
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi tạo lớp học phần" });
        }
    }

    /// <summary>
    /// Cập nhật lớp học phần
    /// </summary>
    [HttpPut("lop-hoc-phan/{id}")]
    public async Task<ActionResult> UpdateLopHocPhan(int id, [FromBody] LopHocPhanRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dữ liệu đầu vào không hợp lệ" });
            }

            var success = await _adminService.UpdateLopHocPhanAsync(id, request);

            if (success)
            {
                return Ok(new { Success = true, Message = "Cập nhật lớp học phần thành công" });
            }

            return BadRequest(new { Success = false, Message = "Cập nhật lớp học phần thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật lớp học phần: {Id}", id);
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi cập nhật lớp học phần" });
        }
    }

    /// <summary>
    /// Xóa lớp học phần
    /// </summary>
    [HttpDelete("lop-hoc-phan/{id}")]
    public async Task<ActionResult> DeleteLopHocPhan(int id)
    {
        try
        {
            var success = await _adminService.DeleteLopHocPhanAsync(id);

            if (success)
            {
                return Ok(new { Success = true, Message = "Xóa lớp học phần thành công" });
            }

            return BadRequest(new { Success = false, Message = "Xóa lớp học phần thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa lớp học phần: {Id}", id);
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi xóa lớp học phần" });
        }
    }

    #endregion

    #region Quản lý thời khóa biểu

    /// <summary>
    /// Lấy danh sách tất cả thời khóa biểu
    /// </summary>
    [HttpGet("thoi-khoa-bieu")]
    public async Task<ActionResult<List<ThoiKhoaBieuResponse>>> GetAllThoiKhoaBieu()
    {
        try
        {
            var thoiKhoaBieu = await _adminService.GetAllThoiKhoaBieuAsync();
            return Ok(thoiKhoaBieu);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách thời khóa biểu");
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy danh sách thời khóa biểu" });
        }
    }

    /// <summary>
    /// Lấy thời khóa biểu theo lớp học phần
    /// </summary>
    [HttpGet("thoi-khoa-bieu/lop-hoc-phan/{lopHocPhanId}")]
    public async Task<ActionResult<List<ThoiKhoaBieuResponse>>> GetThoiKhoaBieuByLopHocPhan(int lopHocPhanId)
    {
        try
        {
            var thoiKhoaBieu = await _adminService.GetThoiKhoaBieuByLopHocPhanAsync(lopHocPhanId);
            return Ok(thoiKhoaBieu);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thời khóa biểu theo lớp học phần: {LopHocPhanId}", lopHocPhanId);
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy thời khóa biểu" });
        }
    }

    /// <summary>
    /// Tạo thời khóa biểu mới
    /// </summary>
    [HttpPost("thoi-khoa-bieu")]
    public async Task<ActionResult> CreateThoiKhoaBieu([FromBody] ThoiKhoaBieuRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dữ liệu đầu vào không hợp lệ" });
            }

            var success = await _adminService.CreateThoiKhoaBieuAsync(request);

            if (success)
            {
                return Ok(new { Success = true, Message = "Tạo thời khóa biểu thành công" });
            }

            return BadRequest(new { Success = false, Message = "Tạo thời khóa biểu thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo thời khóa biểu");
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi tạo thời khóa biểu" });
        }
    }

    /// <summary>
    /// Cập nhật thời khóa biểu
    /// </summary>
    [HttpPut("thoi-khoa-bieu/{id}")]
    public async Task<ActionResult> UpdateThoiKhoaBieu(int id, [FromBody] ThoiKhoaBieuRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dữ liệu đầu vào không hợp lệ" });
            }

            var success = await _adminService.UpdateThoiKhoaBieuAsync(id, request);

            if (success)
            {
                return Ok(new { Success = true, Message = "Cập nhật thời khóa biểu thành công" });
            }

            return BadRequest(new { Success = false, Message = "Cập nhật thời khóa biểu thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật thời khóa biểu: {Id}", id);
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi cập nhật thời khóa biểu" });
        }
    }

    /// <summary>
    /// Xóa thời khóa biểu
    /// </summary>
    [HttpDelete("thoi-khoa-bieu/{id}")]
    public async Task<ActionResult> DeleteThoiKhoaBieu(int id)
    {
        try
        {
            var success = await _adminService.DeleteThoiKhoaBieuAsync(id);

            if (success)
            {
                return Ok(new { Success = true, Message = "Xóa thời khóa biểu thành công" });
            }

            return BadRequest(new { Success = false, Message = "Xóa thời khóa biểu thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa thời khóa biểu: {Id}", id);
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi xóa thời khóa biểu" });
        }
    }

    #endregion

    #region Quản lý đăng ký học

    /// <summary>
    /// Lấy danh sách tất cả đăng ký học
    /// </summary>
    [HttpGet("dang-ky-hoc")]
    public async Task<ActionResult<List<DangKyHocResponse>>> GetAllDangKyHoc()
    {
        try
        {
            var dangKyHocs = await _adminService.GetAllDangKyHocAsync();
            return Ok(dangKyHocs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách đăng ký học");
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy danh sách đăng ký học" });
        }
    }

    /// <summary>
    /// Gán sinh viên vào lớp học phần
    /// </summary>
    [HttpPost("dang-ky-hoc")]
    public async Task<ActionResult> CreateDangKyHoc([FromBody] DangKyHocRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dữ liệu đầu vào không hợp lệ" });
            }

            var success = await _adminService.CreateDangKyHocAsync(request);

            if (success)
            {
                return Ok(new { Success = true, Message = "Gán sinh viên vào lớp học phần thành công" });
            }

            return BadRequest(new { Success = false, Message = "Gán sinh viên vào lớp học phần thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo đăng ký học");
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi tạo đăng ký học" });
        }
    }

    /// <summary>
    /// Xóa đăng ký học
    /// </summary>
    [HttpDelete("dang-ky-hoc/{id}")]
    public async Task<ActionResult> DeleteDangKyHoc(int id)
    {
        try
        {
            var success = await _adminService.DeleteDangKyHocAsync(id);

            if (success)
            {
                return Ok(new { Success = true, Message = "Xóa đăng ký học thành công" });
            }

            return BadRequest(new { Success = false, Message = "Xóa đăng ký học thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa đăng ký học: {Id}", id);
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi xóa đăng ký học" });
        }
    }

    #endregion

    #region Quản lý thông báo chung

    /// <summary>
    /// Tạo thông báo chung cho hệ thống
    /// </summary>
    [HttpPost("thong-bao-chung")]
    public async Task<ActionResult> CreateThongBaoChung([FromBody] ThongBaoChungRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dữ liệu đầu vào không hợp lệ" });
            }

            var success = await _adminService.CreateThongBaoChungAsync(request);

            if (success)
            {
                return Ok(new { Success = true, Message = "Tạo thông báo chung thành công" });
            }

            return BadRequest(new { Success = false, Message = "Tạo thông báo chung thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo thông báo chung");
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi tạo thông báo chung" });
        }
    }

    #endregion
}
