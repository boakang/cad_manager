# Mini CAD Object Manager

## Mục tiêu project
Xây dựng một ứng dụng Desktop (Windows) đơn giản với tên gọi **Mini CAD Object Manager** để quản lý các đối tượng CAD (như Point, Line, Circle...). 
Ứng dụng cho phép người dùng nạp dữ liệu từ các file bên ngoài (JSON, CSV), hiển thị lên giao diện, lọc, tìm kiếm, cũng như áp dụng thuật toán tối ưu hóa hoặc loại bỏ trùng lặp trước khi xuất lại dữ liệu.

## Công nghệ sử dụng
- **Ngôn ngữ lập trình:** C#
- **Framework hiển thị (UI):** WPF (Windows Presentation Foundation)
- **Framework nền tảng:** .NET Core / .NET 7.0 (trở lên)
- **Kiến trúc thiết kế:** MVVM (Model - View - ViewModel)
- **Kiểm thử (Testing):** xUnit / MSTest

## Kiến trúc project
Dự án được chia thành 3 phần chính nằm trong cùng một Solution để phân tách mối quan tâm (Separation of Concerns):
1. **`MiniCadManager.Core`**: Lớp chứa logic cốt lõi. Gồm các mô hình đối tượng (`Models`), các dịch vụ thao tác dữ liệu (`Services`) và các thuật toán toán học (`Algorithms`).
2. **`MiniCadManager.Wpf`**: Lớp chứa giao diện người dùng Desktop. Áp dụng chuẩn MVVM gồm các view (`MainWindow`), logic điều hướng giao diện (`ViewModels`), và các tài nguyên giao diện như Theme (Sáng/Tối).
3. **`MiniCadManager.Tests`**: Lớp chứa các bài kiểm thử tự động (Unit Tests) để đảm bảo các dịch vụ và thuật toán chạy đúng.

## Tính năng chính
- **Nạp dữ liệu:** Đọc dữ liệu từ file `.json` hoặc `.csv`.
- **Hiển thị & Tìm kiếm:** Hiển thị danh sách các đối tượng CAD lên DataGrid. Hỗ trợ tìm kiếm theo Tên (Name) và lọc theo phân loại đối tượng (Type: All, Line, Circle,...).
- **Hoán đổi giao diện:** Thay đổi linh hoạt giữa giao diện Sáng (Light Theme) và Tối (Dark Theme).
- **Tối ưu hóa đường đi:** Áp dụng thuật toán tìm đường đi ngắn nhất đi qua toàn bộ các đối tượng CAD (Route Optimization).
- **Loại bỏ trùng lặp:** Loại bỏ các đối tượng CAD bị trùng lặp trong dữ liệu trước khi xử lý.
- **Xuất dữ liệu:** Cho phép lưu và xuất dữ liệu sau khi chỉnh sửa ra file `.json` hoặc `.txt`.

## Thuật toán sử dụng
- **Tối ưu hóa (Route Optimization):** Sử dụng thuật toán **Nearest Neighbor** (Láng giềng gần nhất - Thuật toán tham lam). Nó tính toán khoảng cách vector tuyến tính giữa tọa độ các điểm X, Y của đối tượng để ưu tiên nối đối tượng gần nhất, từ đó giảm thiểu di chuyển lãng phí.
- **Loại bỏ trùng lặp (Duplicate Detection):** Sử dụng `EqualityComparer` để so sánh và làm phẳng danh sách dựa trên tọa độ X, Y và kiểu (Type) đối tượng.

## Cách chạy project

### Cách 1: Chạy bằng Terminal (Command Line / PowerShell)
1. Mở Terminal và trỏ vào thư mục ứng dụng WPF:
   `cd MiniCadManager.Wpf`
2. Chạy ứng dụng bằng lệnh:
   `dotnet run`
 *(Để chạy Unit Test, trỏ vào `MiniCadManager.Tests` và dùng lệnh `dotnet test`)*

### Cách 2: Chạy bằng Visual Studio (Khuyên dùng)
1. Mở file `MiniCadManager.sln` bằng **Visual Studio 2022**.
2. Tại bảng Solution Explorer (bên phải), ấn chuột phải vào dự án **`MiniCadManager.Wpf`** và chọn **`Set as Startup Project`**.
3. Bấm nút **[Start]** màu xanh lá cây (hoặc phím F5) ở menu trên cùng để chạy ứng dụng.

## Hình minh họa
*(Nếu bạn muốn đính kèm hình minh họa, hãy lưu hình ảnh dưới dạng file ảnh như `screenshot.png` vào thư mục gốc của dự án và thay thế dòng này bằng đường dẫn `![Minh Họa](screenshot.png)`)*
