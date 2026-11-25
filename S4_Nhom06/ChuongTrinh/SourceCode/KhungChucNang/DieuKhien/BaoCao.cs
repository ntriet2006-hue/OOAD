// Khung Control theo đặc tả use case. Chưa nối route hoặc giao diện web.
sealed class DieuKhienBaoCao
{
    /// <summary>UC9.1 – Báo cáo doanh thu. Chưa triển khai.</summary>
    public void BaoCaoDoanhThu(DateTime tuNgay, DateTime denNgay)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý chọn loại báo cáo “Báo cáo doanh thu”
        // TODO: 2. Quản lý chọn tiêu chí lọc (khoảng thời gian, kỳ báo cáo theo ngày/tháng/quý/năm, nguồn thu phòng/dịch vụ)
        // TODO: 3. Hệ thống tổng hợp dữ liệu từ Invoice, Payment, ServiceUsage
        // TODO: 4. Hệ thống hiển thị bảng số liệu và biểu đồ (tổng doanh thu, doanh thu phòng, doanh thu dịch vụ, so sánh các kỳ)
        // TODO: 5. Quản lý lưu báo cáo nếu muốn xem lại sau
        throw new ChuaTrienKhaiException("UC9.1 – Báo cáo doanh thu");
    }

    /// <summary>UC9.2 – Báo cáo công suất phòng. Chưa triển khai.</summary>
    public void BaoCaoCongSuatPhong(DateTime tuNgay, DateTime denNgay)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý chọn loại báo cáo “Báo cáo công suất phòng”
        // TODO: 2. Quản lý chọn tiêu chí lọc (khoảng thời gian, loại phòng)
        // TODO: 3. Hệ thống tổng hợp dữ liệu từ Room, Booking, BookingDetail
        // TODO: 4. Hệ thống hiển thị bảng số liệu và biểu đồ (tỷ lệ lấp đầy, giá phòng trung bình ADR, doanh thu trên mỗi phòng khả dụng RevPAR)
        // TODO: 5. Quản lý lưu báo cáo nếu muốn xem lại sau
        throw new ChuaTrienKhaiException("UC9.2 – Báo cáo công suất phòng");
    }

    /// <summary>UC9.3 – Báo cáo khách hàng và đặt phòng. Chưa triển khai.</summary>
    public void BaoCaoKhachHangVaDatPhong(DateTime tuNgay, DateTime denNgay)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý chọn loại báo cáo “Báo cáo khách hàng và đặt phòng”
        // TODO: 2. Quản lý chọn tiêu chí lọc (khoảng thời gian, nguồn đặt phòng, loại khách)
        // TODO: 3. Hệ thống tổng hợp dữ liệu từ Guest, Booking
        // TODO: 4. Hệ thống hiển thị bảng số liệu và biểu đồ (số lượt khách, khách mới và khách quay lại, nguồn đặt phòng, tỷ lệ hủy)
        // TODO: 5. Quản lý lưu báo cáo nếu muốn xem lại sau
        throw new ChuaTrienKhaiException("UC9.3 – Báo cáo khách hàng và đặt phòng");
    }

    /// <summary>UC9.4 – Xem báo cáo đã lưu. Chưa triển khai.</summary>
    public void XemBaoCaoDaLuu(int maBaoCao)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý chọn danh sách báo cáo đã lưu
        // TODO: 2. Quản lý tìm kiếm theo tên báo cáo, loại báo cáo hoặc ngày tạo
        // TODO: 3. Quản lý chọn báo cáo cần xem
        // TODO: 4. Hệ thống hiển thị nội dung báo cáo
        throw new ChuaTrienKhaiException("UC9.4 – Xem báo cáo đã lưu");
    }

    /// <summary>UC9.5 – Xuất và in báo cáo. Chưa triển khai.</summary>
    public void XuatVaInBaoCao(int maBaoCao, string dinhDang)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý mở một báo cáo đã tạo hoặc đã lưu
        // TODO: 2. Quản lý chọn xuất file hoặc in, và chọn định dạng (PDF hoặc Excel)
        // TODO: 3. Hệ thống tạo tệp theo định dạng đã chọn
        // TODO: 4. Hệ thống tải tệp về thiết bị hoặc gửi lệnh in
        throw new ChuaTrienKhaiException("UC9.5 – Xuất và in báo cáo");
    }

}
