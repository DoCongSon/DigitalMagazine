# DigitalMagazine (Tạp Chí Số) - Hệ Thống Quản Trị Nội Dung Đa Lớp

**DigitalMagazine** là một hệ thống quản trị nội dung (CMS) mạnh mẽ được xây dựng dựa trên nền tảng **Piranha CMS** và được thiết kế theo chuẩn **Kiến Trúc 4 Lớp (Clean Architecture)**. Hệ thống được tối ưu hóa cho việc phát triển các trang tin tức, tạp chí điện tử với hiệu suất cao, khả năng bảo trì tốt và dễ dàng mở rộng.

---

## 🏗 1. Kiến Trúc Hệ Thống (System Architecture)

Hệ thống được thiết kế theo mô hình **Clean Architecture (Kiến trúc Sạch)** nhằm tách biệt hoàn toàn giữa Core Nghiệp Vụ, Logic Xử Lý, Tương Tác Dữ Liệu và Giao Diện Người Dùng.

Sơ đồ 4 lớp của hệ thống:

1. **`DigitalMagazine.CMS` (Tầng Domain/Core CMS):**
   - Chứa các định nghĩa cốt lõi của bài viết: `PageTypes`, `PostTypes` (Ví dụ: `StandardPage`, `StandardArchive`, `StandardPost`).
   - Tầng này hoàn toàn độc lập, không phụ thuộc vào tầng nào khác.

2. **`DigitalMagazine.Application` (Tầng Ứng Dụng):**
   - Định nghĩa các "Hợp đồng" (Interface) quy định các nghiệp vụ như `IHomePageService`, `IAnalyticsService`.
   - Chứa các đối tượng truyền tải dữ liệu `DTOs` (Data Transfer Objects). Không chứa code giao tiếp Database.

3. **`DigitalMagazine.Infrastructure` (Tầng Hạ Tầng):**
   - Nơi kết nối trực tiếp với Cơ Sở Dữ Liệu thông qua Entity Framework Core.
   - Chứa `AppDbContext`, các `Repositories`, và triển khai (Implement) các Interface từ tầng Application.
   - Chịu trách nhiệm gọi API của Piranha CMS để lấy/lưu dữ liệu bài viết.

4. **`DigitalMagazine.Web` (Tầng Giao Diện / Presentation):**
   - Dự án Web ASP.NET Core MVC. Chứa giao diện người dùng (Frontend) và giao diện Quản trị Manager (Backend).
   - Nơi cấu hình Dependency Injection (DI), Middleware, và nạp các thành phần hệ thống (`Program.cs`).

---

## 💻 2. Yêu Cầu Triển Khai (Deployment Requirements)

Để vận hành hệ thống một cách mượt mà và ổn định, môi trường Server cần đáp ứng các tiêu chuẩn sau:

### Nền tảng & Môi trường (Platform & Environment)

- **Hệ điều hành:** Hỗ trợ đa nền tảng. Khuyên dùng **Windows Server (2019/2022)** hoặc **Linux (Ubuntu 20.04/22.04 LTS, Debian)**.
- **Web Server:** IIS (trên Windows) hoặc Nginx / Apache (trên Linux chạy dạng Reverse Proxy). Hỗ trợ chạy Native trên Docker.
- **Môi trường Runtime:** Bắt buộc cài đặt **.NET 8.0 ASP.NET Core Runtime**.

### Cơ Sở Dữ Liệu (Database Engine)

- **Chính thức (Production):** **Microsoft SQL Server 2017 trở lên** (Khuyên dùng SQL Server 2022). Hệ thống cũng hỗ trợ chuyển đổi sang PostgreSQL/MySQL nếu cần (Yêu cầu thay thư viện EF Core Provider).
- **Phát triển (Development):** SQL Server Express hoặc LocalDB `(localdb)\mssqllocaldb`.

### Phần Cứng (Hardware Requirements)

_(Dành cho hệ thống tạp chí quy mô vừa - 10,000 đến 50,000 lượt truy cập/ngày)_

- **CPU:** Tối thiểu 2 Cores (Khuyên dùng 4 Cores).
- **RAM:** Tối thiểu 4GB (Khuyên dùng 8GB trở lên để tối ưu Memory Cache cho Piranha CMS).
- **Ổ cứng (Storage):** Tối thiểu 50GB SSD. Tốc độ đọc ghi (IOPS) cao để tối ưu Database và Load hình ảnh bài viết.
- **Network:** Tối thiểu 100Mbps băng thông.

---

## ⚙️ 3. Hướng Dẫn Cài Đặt & Setup (Deployment Guide)

### Môi trường Local (Dành cho Lập Trình Viên)

1. **Clone dự án & Mở bằng IDE:** Mở file `DigitalMagazine.sln` bằng Visual Studio 2022 hoặc JetBrains Rider.
2. **Cấu hình Database:** Mở file `src/DigitalMagazine.Web/appsettings.Development.json` và kiểm tra lại chuỗi kết nối (Mặc định dùng LocalDB).
3. **Chạy ứng dụng:**
   Nhấn F5 trong IDE hoặc mở Terminal tại thư mục `src/DigitalMagazine.Web` gõ lệnh:
   ```bash
   dotnet run --seed
   ```
   _Lệnh này sẽ tự động Migrate cơ sở dữ liệu và tạo tài khoản Admin mặc định._

### Môi trường Server Thực Tế (Production - Windows/IIS)

1. **Publish Code:**
   Mở terminal tại thư mục gốc và chạy lệnh đóng gói dự án:
   ```bash
   dotnet publish src/DigitalMagazine.Web/DigitalMagazine.Web.csproj -c Release -o ./publish
   ```
2. **Cấu hình Server:**
   - Cài đặt `.NET 8.0 Hosting Bundle` lên Windows Server.
   - Mở IIS, tạo một Website mới và trỏ thư mục vật lý vào thư mục `./publish` vừa tạo.
3. **Cấu hình appsettings.json:**
   - Cập nhật lại chuỗi kết nối trong `appsettings.json` thành IP và thông tin đăng nhập của SQL Server thực tế.
   - Sửa `"LogLevel"` thành `"Warning"` để giảm dung lượng file Log.
4. **Bảo mật:**
   - Cấp quyền Read/Write cho tài khoản `IIS_IUSRS` vào thư mục `wwwroot/uploads` để CMS có thể upload hình ảnh.
   - Mua và cài đặt chứng chỉ SSL (HTTPS) cho Tên miền.

---

## 🔧 4. Công Nghệ & Thư Viện Sử Dụng (Tech Stack)

- **Cốt lõi:** .NET 8.0, C# 12.
- **Quản lý Nội dung:** Piranha CMS v12.0 (Giao diện Admin viết bằng Vue.js, Bootstrap 4).
- **Truy xuất dữ liệu:** Entity Framework Core 8.0, LINQ.
- **Bảo mật & Định danh:** ASP.NET Core Identity.
- **Giao diện Frontend (Khách hàng):** ASP.NET MVC Razor Pages (HTML5, CSS3, ES6).
