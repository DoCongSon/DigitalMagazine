# DigitalMagazine - Hệ Thống Quản Trị Nội Dung Đa Lớp

**DigitalMagazine** là một hệ thống quản trị nội dung (CMS) mạnh mẽ được xây dựng dựa trên nền tảng **Piranha CMS** và được thiết kế theo chuẩn **Kiến Trúc 4 Lớp (Clean Architecture)**. Hệ thống được tối ưu hóa cho việc phát triển các trang tin tức, blog với hiệu suất cao, khả năng bảo trì tốt và dễ dàng mở rộng.

---

## 🛠 Công Nghệ Sử Dụng

- **Framework:** .NET 8.0 (ASP.NET Core MVC)
- **Core CMS:** Piranha CMS v12.0
- **Database ORM:** Entity Framework Core 8.0
- **Database Engine:** Microsoft SQL Server (Hỗ trợ LocalDB cho phát triển)
- **Identity:** ASP.NET Core Identity tích hợp sẵn của Piranha.
- **Frontend Admin:** Vue.js, Bootstrap 4 (Tích hợp trong Piranha Manager)

---

## 📂 Cấu Trúc Dự Án (Clean Architecture)

Dự án được chia làm 4 layer độc lập nằm trong thư mục `src/`, đảm bảo tính đóng gói và không phụ thuộc chéo:

1. **`DigitalMagazine.CMS` (Tầng Domain/Core CMS):**
   Chứa các cấu trúc định nghĩa cốt lõi nhất như `PageTypes`, `PostTypes` của Piranha CMS (Ví dụ: `StandardPage`, `StandardArchive`, `StandardPost`). Tầng này không phụ thuộc vào bất kỳ tầng nào khác.

2. **`DigitalMagazine.Application` (Tầng Ứng Dụng):**
   Chứa các Interface quy định các hợp đồng xử lý nghiệp vụ (Ví dụ: `IHomePageService`, `IAnalyticsService`) và các đối tượng dữ liệu truyền tải (DTOs).

3. **`DigitalMagazine.Infrastructure` (Tầng Hạ Tầng):**
   Triển khai thực tế các Interface từ tầng Application. Phụ trách giao tiếp với Cơ sở dữ liệu thông qua Entity Framework Core (chứa `AppDbContext`, Repositories, Services) và gọi các API của Piranha để lấy dữ liệu CMS.

4. **`DigitalMagazine.Web` (Tầng Giao Diện/Entry Point):**
   Dự án ASP.NET Core MVC (Frontend & Backend). Nơi cấu hình `Program.cs`, khai báo Dependency Injection, kết nối Database, cấu hình Middleware của Piranha và chứa các HTML Views (Views/Controllers/Areas).

---

## 🚀 Hướng Dẫn Cài Đặt & Chạy Dự Án

### Yêu Cầu Hệ Thống

- Đã cài đặt [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
- SQL Server (Hoặc SQL Server Express / LocalDB `(localdb)\mssqllocaldb`).

### Bước 1: Cấu hình Connection String

Mở file `src/DigitalMagazine.Web/appsettings.json` và kiểm tra lại chuỗi kết nối Database. Mặc định dự án sử dụng `LocalDB`:

```json
"ConnectionStrings": {
  "piranha": "Server=(localdb)\\mssqllocaldb;Database=DigitalMagazine;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

### Bước 2: Cập Nhật Database (Migrations)

Dự án sử dụng cơ chế EF Core Migrations song song (Một cho Identity của Piranha, một cho Custom DB của ứng dụng). Chạy lần lượt các lệnh sau tại thư mục gốc:

```bash
# Di chuyển vào thư mục Web
cd src/DigitalMagazine.Web

# Seed dữ liệu Identity của Piranha
dotnet run --seed
```

_Lưu ý: Quá trình khởi động `dotnet run` đã được tích hợp sẵn lệnh `db.Database.Migrate()` cho `AppDbContext` tự động chạy._

### Bước 3: Chạy Ứng Dụng

Trong thư mục `src/DigitalMagazine.Web`, chạy lệnh:

```bash
dotnet run
```

Sau khi ứng dụng chạy:

- 🌍 **Trang Khách (Frontend):** `http://localhost:5000/`
- ⚙️ **Trang Quản Trị (Backend):** `http://localhost:5000/manager`
- 🔑 **Tài khoản mặc định:**
  - Username: `admin`
  - Password: `password`

---

_Developed with Clean Architecture standards._
