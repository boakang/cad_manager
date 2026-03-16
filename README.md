# M.C.A.D - Mini CAD Object Manager

## Mục tiêu project
Xây dựng một ứng dụng Desktop chuyên nghiệp để quản lý và thao tác với các đối tượng hình học CAD 2D (Đường thẳng, Đường tròn, Text). Dự án tập trung trình diễn khả năng ứng dụng Hình học giải tích, cấu trúc dữ liệu, và tư duy phát triển phần mềm theo tiêu chuẩn ngành (MVVM Pattern, OOP, Design Pattern).

## Công nghệ sử dụng
- **Ngôn ngữ:** C# (.NET 7.0)
- **Framework Giao diện:** WPF (XAML) - Giao diện Modern Dashboard, tự động thích ứng Light/Dark theme.
- **Kiến trúc:** MVVM (Model-View-ViewModel) chuẩn mực.
- **Xử lý dữ liệu:** System.Text.Json (JSON), File I/O (CSV, DXF).
- **Kiểm thử:** NUnit / Moq (Unit Testing).

## Kiến trúc project
Dự án được chia làm 3 lớp (nguyên tắc Separation of Concerns) đảm bảo dễ kiểm thử và mở rộng:
1. **`MiniCadManager.Core`**: Lõi nghiệp vụ (Domain Models, DxfExportService, FileLoaderService). Điểm tụ của toán học giải tích và không chứa code UI.
2. **`MiniCadManager.Wpf`**: Phụ trách giao diện, DataBinding và điều phối thao tác người dùng (ViewModels, Views). Cấu trúc chuẩn MVVM.
3. **`MiniCadManager.Tests`**: Tầng kiểm thử tự động để đảm bảo thuật toán Core chạy thiết thực nhất dưới mọi tình huống.

## Tính năng chính
- **Dựng hình học Vector 2D:** Vẽ bản vẽ kỹ thuật lên Canvas (Viewport) với hệ tọa độ trực quan cùng Lưới tọa độ (Grid Background).
- **Quản lý dữ liệu lớn (Data Explorer):** Hiển thị và lọc thông số tọa độ trên DataGrid với độ phản hồi tức thì.
- **Giao tiếp Cấp thấp (DXF Export):** Biên dịch các Object hiển thị màn hình thành file bản vẽ định dạng `.dxf` tiêu chuẩn công nghiệp (tương thích AutoCAD).
- **Giao diện thân thiện:** Tự động linh hoạt chuyển đổi toàn bộ mã màu UI giữa Light/Dark Theme.

## Thuật toán sử dụng
1. **Geometry Engine:** Cung cấp hàm toán hình học CAD cơ bản: tính Hộp bao (Bounding Box), tính toán diện tích tổng (Area), chu vi/độ dài và kiểm tra Giao cắt đường thẳng (Line Intersection).
2. **Tối ưu hóa hành trình (Nearest Neighbor):** Áp dụng thuật toán Tham lam (Greedy) để giải quyết bài toán tìm lộ trình duyệt các tọa độ Object ngắn nhất (Mô phỏng đường đi dao cắt CNC).

## Cách chạy project
**Yêu cầu:** Đã cài đặt .NET 7.0 SDK
1. Clone dự án về máy.
2. Mở Terminal / Command Prompt tại thư mục `MiniCadManager.Wpf`.
3. Chạy lệnh: `dotnet run`
4. Trên giao diện, bấm **"Load Data"** để vẽ mẫu.
*(Tuỳ chọn: Chạy test bằng lệnh `dotnet test` ở thư mục `MiniCadManager.Tests`).*

## Hình minh họa
### Theme sáng
![Minh Họa](https://github.com/boakang/cad_manager/blob/main/Screenshot%202026-03-16%20083114.png)
### Theme tối
![Minh Họa](https://github.com/boakang/cad_manager/blob/main/Screenshot%202026-03-16%20083151.png)
### Thêm file json vào CAD
![Minh Họa](https://github.com/boakang/cad_manager/blob/main/Screenshot%202026-03-16%20083221.png)