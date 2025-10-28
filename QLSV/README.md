# Hệ thống Quản lý Sinh viên (QLSV) - API Documentation

## Tổng quan
Hệ thống QLSV cung cấp các API để quản lý sinh viên, giảng viên, môn học, lớp học phần, điểm số và thông báo.

## Cấu trúc API

### 🔐 Authentication Controller (`/api/auth`)
- **POST** `/api/auth/login` - Đăng nhập hệ thống
- **POST** `/api/auth/logout` - Đăng xuất hệ thống  
- **GET** `/api/auth/user-info` - Lấy thông tin người dùng

### 🎓 Student Controller (`/api/student`)
- **GET** `/api/student/{sinhVienId}/info` - Xem thông tin cá nhân
- **GET** `/api/student/{sinhVienId}/lop-hoc-phan` - Xem danh sách lớp học phần đã đăng ký
- **GET** `/api/student/{sinhVienId}/thoi-khoa-bieu` - Xem thời khóa biểu chi tiết
- **GET** `/api/student/{sinhVienId}/diem-so` - Xem điểm số các môn học
- **GET** `/api/student/{sinhVienId}/thong-bao` - Xem thông báo chung và từ giảng viên

### 👩‍🏫 Teacher Controller (`/api/teacher`)
- **GET** `/api/teacher/{giangVienId}/info` - Xem thông tin cá nhân
- **GET** `/api/teacher/{giangVienId}/lop-hoc-phan` - Xem danh sách lớp học phần được phân công
- **GET** `/api/teacher/{giangVienId}/thoi-khoa-bieu` - Xem thời khóa biểu các lớp giảng dạy
- **GET** `/api/teacher/{giangVienId}/lop-hoc-phan/{lopHocPhanId}/sinh-vien-diem` - Xem danh sách sinh viên và điểm
- **PUT** `/api/teacher/cap-nhat-diem` - Nhập/cập nhật điểm cho sinh viên
- **POST** `/api/teacher/{giangVienId}/thong-bao` - Tạo thông báo cho lớp học phần

### 🧑‍💼 Admin Controller (`/api/admin`)

#### Quản lý môn học
- **GET** `/api/admin/mon-hoc` - Lấy danh sách tất cả môn học
- **GET** `/api/admin/mon-hoc/{id}` - Lấy thông tin môn học theo ID
- **POST** `/api/admin/mon-hoc` - Tạo môn học mới
- **PUT** `/api/admin/mon-hoc/{id}` - Cập nhật môn học
- **DELETE** `/api/admin/mon-hoc/{id}` - Xóa môn học

#### Quản lý lớp học phần
- **GET** `/api/admin/lop-hoc-phan` - Lấy danh sách tất cả lớp học phần
- **POST** `/api/admin/lop-hoc-phan` - Tạo lớp học phần mới
- **PUT** `/api/admin/lop-hoc-phan/{id}` - Cập nhật lớp học phần
- **DELETE** `/api/admin/lop-hoc-phan/{id}` - Xóa lớp học phần

#### Quản lý thời khóa biểu
- **GET** `/api/admin/thoi-khoa-bieu` - Lấy danh sách tất cả thời khóa biểu
- **GET** `/api/admin/thoi-khoa-bieu/lop-hoc-phan/{lopHocPhanId}` - Lấy thời khóa biểu theo lớp học phần
- **POST** `/api/admin/thoi-khoa-bieu` - Tạo thời khóa biểu mới
- **PUT** `/api/admin/thoi-khoa-bieu/{id}` - Cập nhật thời khóa biểu
- **DELETE** `/api/admin/thoi-khoa-bieu/{id}` - Xóa thời khóa biểu

#### Quản lý đăng ký học
- **GET** `/api/admin/dang-ky-hoc` - Lấy danh sách tất cả đăng ký học
- **POST** `/api/admin/dang-ky-hoc` - Gán sinh viên vào lớp học phần
- **DELETE** `/api/admin/dang-ky-hoc/{id}` - Xóa đăng ký học

#### Quản lý thông báo chung
- **POST** `/api/admin/thong-bao-chung` - Tạo thông báo chung cho hệ thống

## Các loại dữ liệu (DTOs)

### Auth DTOs
- `LoginRequest` - Dữ liệu đăng nhập
- `LoginResponse` - Kết quả đăng nhập
- `UserInfo` - Thông tin người dùng
- `LogoutRequest` - Dữ liệu đăng xuất

### Student DTOs
- `SinhVienInfoResponse` - Thông tin sinh viên
- `LopHocPhanResponse` - Thông tin lớp học phần
- `ThoiKhoaBieuResponse` - Thông tin thời khóa biểu
- `DiemResponse` - Thông tin điểm số
- `ThongBaoResponse` - Thông tin thông báo

### Teacher DTOs
- `GiangVienInfoResponse` - Thông tin giảng viên
- `LopHocPhanGiangVienResponse` - Lớp học phần của giảng viên
- `ThoiKhoaBieuGiangVienResponse` - Thời khóa biểu của giảng viên
- `SinhVienDiemResponse` - Điểm sinh viên
- `CapNhatDiemRequest` - Dữ liệu cập nhật điểm
- `TaoThongBaoRequest` - Dữ liệu tạo thông báo

### Admin DTOs
- `MonHocRequest/Response` - Môn học
- `LopHocPhanRequest/Response` - Lớp học phần
- `ThoiKhoaBieuRequest/Response` - Thời khóa biểu
- `DangKyHocRequest/Response` - Đăng ký học
- `ThongBaoChungRequest` - Thông báo chung

## Cấu hình

### Connection String
Cập nhật connection string trong `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Connection": "Server=your_server;Database=QLSV;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### Swagger UI
Truy cập Swagger UI tại: `https://localhost:port/swagger` (trong môi trường Development)

## Tính năng đã hoàn thành

### ✅ Dễ (Easy)
- **Sinh viên**: Đăng nhập/đăng xuất, xem thông tin cá nhân, xem danh sách lớp học phần
- **Giảng viên**: Đăng nhập/đăng xuất, xem danh sách lớp học phần, xem thời khóa biểu
- **Admin**: Quản lý môn học, tạo lớp học phần, tạo thông báo chung

### ✅ Trung bình (Medium)
- **Sinh viên**: Xem thời khóa biểu, xem điểm số, xem thông báo
- **Giảng viên**: Nhập/cập nhật điểm, tạo thông báo
- **Admin**: Quản lý tài khoản, gán sinh viên vào lớp, quản lý thời khóa biểu

## Lưu ý
- Tất cả API đều có xử lý lỗi và logging
- Validation dữ liệu đầu vào được thực hiện bằng Data Annotations
- Sử dụng Entity Framework Core với SQL Server
- Có thể mở rộng thêm authentication/authorization với JWT
