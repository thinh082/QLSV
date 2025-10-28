using Microsoft.AspNetCore.Mvc;
using QLSV.Models.DTOs;
using QLSV.Services;

namespace QLSV.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Đăng nhập hệ thống
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Dữ liệu đầu vào không hợp lệ"
                });
            }

            var result = await _authService.LoginAsync(request);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi đăng nhập");
            return StatusCode(500, new LoginResponse
            {
                Success = false,
                Message = "Có lỗi xảy ra khi đăng nhập"
            });
        }
    }

    /// <summary>
    /// Đăng xuất hệ thống
    /// </summary>
    [HttpPost("logout")]
    public async Task<ActionResult> Logout([FromBody] LogoutRequest request)
    {
        try
        {
            var success = await _authService.LogoutAsync(request.Token);

            if (success)
            {
                return Ok(new { Success = true, Message = "Đăng xuất thành công" });
            }

            return BadRequest(new { Success = false, Message = "Đăng xuất thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi đăng xuất");
            return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra khi đăng xuất" });
        }
    }

    /// <summary>
    /// Lấy thông tin người dùng hiện tại
    /// </summary>
    [HttpGet("user-info")]
    public async Task<ActionResult<UserInfo>> GetUserInfo([FromQuery] int userId, [FromQuery] string userType)
    {
        try
        {
            var userInfo = await _authService.GetUserInfoAsync(userId, userType);

            if (userInfo == null)
            {
                return NotFound(new { Message = "Không tìm thấy thông tin người dùng" });
            }

            return Ok(userInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thông tin người dùng");
            return StatusCode(500, new { Message = "Có lỗi xảy ra khi lấy thông tin người dùng" });
        }
    }
}
