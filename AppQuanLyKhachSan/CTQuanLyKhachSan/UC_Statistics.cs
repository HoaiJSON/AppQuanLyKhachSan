using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Data;
using LiveCharts.Definitions.Charts;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static TheArtOfDevHtmlRenderer.Adapters.RGraphicsPath;
using SeriesCollection = LiveCharts.SeriesCollection;
using System.Windows.Controls.Primitives;

namespace AppQuanLyKhachSan.CTQuanLyKhachSan
{
    public partial class UC_Statistics : UserControl
    {
        functions fn = new functions();
        String query;
        public UC_Statistics()
        {
            InitializeComponent();
        }


        private void btnthongke_Click(object sender, EventArgs e)
        {


            // nếu không có thằng nào được chọn thì không được thống kê 
            if (!checkAll.Checked && !checkDV.Checked && !checkPhong.Checked)
            {
                MessageBox.Show("Vui lòng chọn điều kiện lọc trước khi thống kê.", "Chú ý!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (checkDV.Checked && !checkPhong.Checked && !checkAll.Checked)
            {
                    query = @"DECLARE @ngay_bat_dau NVARCHAR(250) = '" + dtpBatDau.Value.ToString("yyyy-MM-dd") + "';" +
                        "DECLARE @ngay_ket_thuc NVARCHAR(250) = '" + dtpKetThuc.Value.ToString("yyyy-MM-dd") + "';" +
                        @"SELECT HD.MAHD AS MaHoaDon,
                        STUFF(( SELECT DISTINCT ', ' + DV.TENDICHVU
                        FROM CHITIETHOADON CTHD
                        INNER JOIN DICHVU DV ON CTHD.MADICHVU = DV.MADICHVU
                        WHERE CTHD.MAHD = HD.MAHD
                        AND DV.MADICHVU = '"+cbxThongke.Text+"'"+
                        @"FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS DanhSachDichVu,
                        SUM(CTHD.SOLUONG * CTHD.GIA) AS TongTienDichVu,
                        HD.TONGTIEN AS TongTienPhong,
                        (HD.TONGTIEN + SUM(CTHD.SOLUONG * CTHD.GIA)) AS TongDoanhThu,
                        HD.NGAYLAP AS NgayLapHoaDon
                        FROM HOADON HD
                        LEFT JOIN CHITIETHOADON CTHD ON HD.MAHD = CTHD.MAHD
                        LEFT JOIN DICHVU DV ON CTHD.MADICHVU = DV.MADICHVU
                        WHERE HD.NGAYLAP BETWEEN @ngay_bat_dau AND @ngay_ket_thuc AND DV.MADICHVU = '"+cbxThongke.Text+"'"+
                        @"GROUP BY HD.MAHD, HD.TONGTIEN,HD.NGAYLAP
                        ORDER BY HD.NGAYLAP;";

                    DataSet ds = fn.GetData(query);
                    dataGridView1.DataSource = ds.Tables[0];

                // Câu truy vấn SQL thứ hai, với điều kiện DV.MADICHVU = cbxThongke.Text
                query = @"  DECLARE @NgayBatDau DATE = '" + dtpBatDau.Value.ToString("yyyy-MM-dd") + @"';
                            DECLARE @NgayKetThuc DATE = '" + dtpKetThuc.Value.ToString("yyyy-MM-dd") + @"';
                            SELECT CONVERT(VARCHAR(10), HD.NGAYLAP, 120) AS Ngay, 
                            SUM(CTHD.SOLUONG * CTHD.GIA) AS DoanhThu
                            FROM HOADON HD
                            LEFT JOIN CHITIETHOADON CTHD ON HD.MAHD = CTHD.MAHD
                            LEFT JOIN DICHVU DV ON CTHD.MADICHVU = DV.MADICHVU
                            WHERE HD.NGAYLAP BETWEEN @NgayBatDau AND @NgayKetThuc
                            AND DV.MADICHVU = '"+cbxThongke.Text+"'"+
                            @"GROUP BY HD.NGAYLAP
                            ORDER BY HD.NGAYLAP";

                // Sử dụng đối tượng functions để thực hiện truy vấn và lấy dữ liệu
                DataSet dt = fn.GetData(query);

                if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                {
                    // Gán dữ liệu từ DataTable vào biểu đồ
                    chartBDT.DataSource = dt.Tables[0];

                    // Đặt cột "Ngay" làm trục X và "DoanhThu" làm trục Y
                    chartBDT.Series["ChartBDT"].XValueMember = "Ngay";
                    chartBDT.Series["ChartBDT"].YValueMembers = "DoanhThu";

                    // Thiết lập kiểu dữ liệu cho trục X và trục Y
                    chartBDT.Series["ChartBDT"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;  // Ngay là chuỗi
                    chartBDT.Series["ChartBDT"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;  // DoanhThu là số

                    // Định dạng lại ngày để loại bỏ dấu "/"
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        row["Ngay"] = Convert.ToDateTime(row["Ngay"]).ToString("yyyy-MM-dd");  // Định dạng ngày theo kiểu yyyy-MM-dd
                    }

                    // Cập nhật biểu đồ để hiển thị dữ liệu
                    chartBDT.DataBind();
                    
                }
                else
                {
                    // Nếu không có dữ liệu, thêm một điểm mặc định vào biểu đồ
                    chartBDT.Series["ChartBDT"].Points.Clear();  // Xóa điểm cũ
                    chartBDT.Series["ChartBDT"].Points.AddXY(0, "Không có dữ liệu");  // Thêm điểm mặc định, trục X là 0, Y là "Không có dữ liệu"
                }



                query = @" DECLARE @NgayBatDau DATE = '" + dtpBatDau.Value.ToString("yyyy-MM-dd") + @"';
                                DECLARE @NgayKetThuc DATE = '" + dtpKetThuc.Value.ToString("yyyy-MM-dd") + @"';
                                SELECT MONTH(HD.NGAYLAP) AS THANG, 
                                SUM(CTHD.SOLUONG * CTHD.GIA) AS DOANHTHU
                                FROM HOADON HD
                                INNER JOIN CHITIETHOADON CTHD ON HD.MAHD = CTHD.MAHD
                                INNER JOIN DICHVU DV ON CTHD.MADICHVU = DV.MADICHVU
                                WHERE DV.MADICHVU = '"+cbxThongke.Text+"'"+
                                @"AND HD.NGAYLAP BETWEEN @NgayBatDau AND @NgayKetThuc
                                GROUP BY MONTH(HD.NGAYLAP)
                                ORDER BY MONTH(HD.NGAYLAP);";


                    DataSet df = fn.GetData(query);
                    if (df != null && df.Tables.Count > 0 && df.Tables[0].Rows.Count > 0)
                    {
                        ChartBDC.Series["ChartBDC"].Points.Clear();

                        foreach (DataRow row in df.Tables[0].Rows)
                        {
                            int thang = Convert.ToInt32(row["THANG"]);
                            decimal doanhThu = Convert.ToDecimal(row["DOANHTHU"]);

                            ChartBDC.Series["ChartBDC"].Points.AddXY("" + thang, doanhThu);
                        }

                        ChartBDC.Series["ChartBDC"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column; 
                        ChartBDC.Series["ChartBDC"].IsValueShownAsLabel = true; 
                        ChartBDC.ChartAreas[0].AxisX.Title = "Tháng";
                        ChartBDC.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)"; 
                    }
                    else
                    {
                        // Xử lý khi không có dữ liệu
                        MessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                

            }
            if (checkPhong.Checked && !checkDV.Checked && !checkAll.Checked)
            {
                    query = @"DECLARE @ngay_bat_dau NVARCHAR(250) = '" + dtpBatDau.Value.ToString("yyyy-MM-dd") + "';" +
                       "DECLARE @ngay_ket_thuc NVARCHAR(250) = '" + dtpKetThuc.Value.ToString("yyyy-MM-dd") + "';" +
                       @"SELECT P.MAPHONG,
                        P.SOPHONG, P.LOAIPHONG, P.GIA AS GIA_PHONG,
                        SUM(HD.TONGTIEN) AS DOANHTHU
                        FROM HOADON HD
                        INNER JOIN DATPHONG DP ON HD.MADATPHONG = DP.MADATPHONG
                        INNER JOIN PHONG P ON DP.MAPHONG = P.MAPHONG
                        WHERE P.MAPHONG = '"+cbxThongke.Text+"'"+
                        @"AND HD.NGAYLAP BETWEEN @ngay_bat_dau AND @ngay_ket_thuc
                        GROUP BY P.MAPHONG, P.SOPHONG, P.LOAIPHONG, P.GIA; ";
                    DataSet ds = fn.GetData(query);
                    dataGridView1.DataSource = ds.Tables[0];

                   query = @"DECLARE @NgayBatDau DATE = '" + dtpBatDau.Value.ToString("yyyy-MM-dd") + @"';
                    DECLARE @NgayKetThuc DATE = '" + dtpKetThuc.Value.ToString("yyyy-MM-dd") + @"';
                    SELECT CONVERT(VARCHAR(7), HOADON.NGAYLAP, 120) AS Thang, 
                    SUM(HOADON.TONGTIEN) AS DoanhThu
                    FROM HOADON
                    INNER JOIN DATPHONG ON HOADON.MADATPHONG = DATPHONG.MADATPHONG
                    INNER JOIN PHONG ON DATPHONG.MAPHONG = PHONG.MAPHONG
                    WHERE PHONG.MAPHONG = '" + cbxThongke.Text + @"'  
                    AND HOADON.NGAYLAP BETWEEN @NgayBatDau AND @NgayKetThuc 
                    GROUP BY CONVERT(VARCHAR(7), HOADON.NGAYLAP, 120)  
                    ORDER BY CONVERT(VARCHAR(7), HOADON.NGAYLAP, 120); ";

                DataSet dt = fn.GetData(query);

                if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                {
                    chartBDT.DataSource = dt.Tables[0];

                    chartBDT.Series["ChartBDT"].XValueMember = "Thang";
                    chartBDT.Series["ChartBDT"].YValueMembers = "DoanhThu";

                    chartBDT.Series["ChartBDT"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;  // Ngay là chuỗi
                    chartBDT.Series["ChartBDT"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;  // DoanhThu là số

                    chartBDT.DataBind();
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


                query = @"DECLARE @NgayBatDau DATE = '" + dtpBatDau.Value.ToString("yyyy-MM-dd") + @"';
                        DECLARE @NgayKetThuc DATE = '" + dtpKetThuc.Value.ToString("yyyy-MM-dd") + @"';"+
                        @"SELECT MONTH(HD.NGAYLAP) AS THANG, 
                        SUM(HD.TONGTIEN) AS DOANHTHU
                        FROM HOADON HD
                        INNER JOIN DATPHONG DP ON HD.MADATPHONG = DP.MADATPHONG
                        INNER JOIN PHONG P ON DP.MAPHONG = P.MAPHONG
                        WHERE P.MAPHONG = '"+cbxThongke.Text+"' AND HD.NGAYLAP BETWEEN @NgayBatDau AND @NgayKetThuc "+
                        @"GROUP BY MONTH(HD.NGAYLAP)
                        ORDER BY MONTH(HD.NGAYLAP);";

                    DataSet df = fn.GetData(query);

                    if (df != null && df.Tables.Count > 0 && df.Tables[0].Rows.Count > 0)
                    {
                        ChartBDC.Series["ChartBDC"].Points.Clear();

                        foreach (DataRow row in df.Tables[0].Rows)
                        {
                            int thang = Convert.ToInt32(row["THANG"]);
                            decimal doanhThu = Convert.ToDecimal(row["DOANHTHU"]);

                            ChartBDC.Series["ChartBDC"].Points.AddXY("" + thang, doanhThu);
                        }
                        ChartBDC.Series["ChartBDC"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column; // Dạng cột
                        ChartBDC.Series["ChartBDC"].IsValueShownAsLabel = true; 
                        ChartBDC.ChartAreas[0].AxisX.Title = "Tháng"; 
                        ChartBDC.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)"; 
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                
            }

            if (checkAll.Checked )
            {
                if (cbxThongke.Text == "Tất cả")
                {
                        query = @"DECLARE @ngay_bat_dau DATE = '" + dtpBatDau.Value.ToString("yyyy-MM-dd") + "';" +
                                "DECLARE @ngay_ket_thuc DATE = '" + dtpKetThuc.Value.ToString("yyyy-MM-dd") + "';" +
                                @"SELECT P.MAPHONG, P.SOPHONG, P.LOAIPHONG, P.LOAIGIUONG, P.GIA AS PHONG_GIA, DV.MADICHVU, DV.TENDICHVU, DV.GIA AS DICHVU_GIA, DV.DONVITINH, SUM(CTHD.SOLUONG) AS DICHVU_SOLUONG, SUM(CTHD.GIA * CTHD.SOLUONG) AS DICHVU_TONGTIEN, HD.MAHD, HD.NGAYLAP AS NGAYCHECKOUT
                                FROM PHONG P
                                LEFT JOIN DATPHONG DP ON P.MAPHONG = DP.MAPHONG
                                LEFT JOIN HOADON HD ON DP.MADATPHONG = HD.MADATPHONG
                                LEFT JOIN CHITIETHOADON CTHD ON HD.MAHD = CTHD.MAHD
                                LEFT JOIN DICHVU DV ON CTHD.MADICHVU = DV.MADICHVU
                                WHERE HD.MAHD IS NOT NULL
                                AND CTHD.MADICHVU IS NOT NULL
                                AND HD.NGAYLAP BETWEEN @ngay_bat_dau AND @ngay_ket_thuc
                                GROUP BY P.MAPHONG, P.SOPHONG, P.LOAIPHONG, P.LOAIGIUONG, P.GIA, DV.MADICHVU, DV.TENDICHVU, DV.GIA, DV.DONVITINH, HD.MAHD, HD.NGAYLAP
                                ORDER BY HD.MAHD, HD.NGAYLAP DESC;";

                    DataSet ds = fn.GetData(query);
                    dataGridView1.DataSource = ds.Tables[0];

                    query = @"DECLARE @ngay_bat_dau NVARCHAR(250) = '" + dtpBatDau.Value.ToString("yyyy-MM-dd") + "';" +
                            "DECLARE @ngay_ket_thuc NVARCHAR(250) = '" + dtpKetThuc.Value.ToString("yyyy-MM-dd") + "';" +
                            @"SELECT 
                                PH.MAPHONG, SUM(HD.TONGTIEN) + SUM(CTHD.SOLUONG * CTHD.GIA) AS TONGDOANHTHU
                                FROM HOADON HD
                                JOIN DATPHONG DP ON HD.MADATPHONG = DP.MADATPHONG
                                JOIN PHONG PH ON DP.MAPHONG = PH.MAPHONG
                                LEFT JOIN CHITIETHOADON CTHD ON HD.MAHD = CTHD.MAHD
                                LEFT JOIN DICHVU DV ON CTHD.MADICHVU = DV.MADICHVU
                                WHERE HD.NGAYLAP BETWEEN @ngay_bat_dau AND @ngay_ket_thuc
                                GROUP BY PH.MAPHONG;  ";

                    DataSet dtall = fn.GetData(query);

                    if (dtall != null && dtall.Tables.Count > 0 && dtall.Tables[0].Rows.Count > 0)
                    {
                        chartBDT.DataSource = dtall.Tables[0];

                        chartBDT.Series["ChartBDT"].XValueMember = "MAPHONG"; 
                        chartBDT.Series["ChartBDT"].YValueMembers = "TONGDOANHTHU"; 

                        chartBDT.Series["ChartBDT"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                        chartBDT.Series["ChartBDT"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

                        chartBDT.DataBind();
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    query = @"SELECT MONTH(HD.NGAYLAP) AS THANG,
                            SUM(CTHD.SOLUONG * CTHD.GIA) + SUM(PHONG.GIA) AS TONGDOANHTHU
                            FROM HOADON HD
                            LEFT JOIN CHITIETHOADON CTHD ON HD.MAHD = CTHD.MAHD
                            LEFT JOIN DATPHONG DP ON HD.MADATPHONG = DP.MADATPHONG
                            LEFT JOIN PHONG ON DP.MAPHONG = PHONG.MAPHONG
                            GROUP BY MONTH(HD.NGAYLAP)
                            ORDER BY MONTH(HD.NGAYLAP);";

                    DataSet df = fn.GetData(query);
                    if (df != null && df.Tables.Count > 0 && df.Tables[0].Rows.Count > 0)
                    {
                        ChartBDC.Series["ChartBDC"].Points.Clear();

                        foreach (DataRow row in df.Tables[0].Rows)
                        {
                            int thang = Convert.ToInt32(row["THANG"]);
                            decimal doanhThu = Convert.ToDecimal(row["TONGDOANHTHU"]);

                            ChartBDC.Series["ChartBDC"].Points.AddXY("" + thang, doanhThu);
                        }

                        ChartBDC.Series["ChartBDC"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column; // Dạng cột
                        ChartBDC.Series["ChartBDC"].IsValueShownAsLabel = true; 
                        ChartBDC.ChartAreas[0].AxisX.Title = "Tháng"; 
                        ChartBDC.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)"; 
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                }
                else if (cbxThongke.Text == "Dịch vụ")
                {
                    query = @"DECLARE @ngay_bat_dau NVARCHAR(250) = '" + dtpBatDau.Value.ToString("yyyy-MM-dd") + "';" +
                            "DECLARE @ngay_ket_thuc NVARCHAR(250) = '" + dtpKetThuc.Value.ToString("yyyy-MM-dd") + "';" +
                            @"SELECT HD.MAHD AS MaHoaDon, 
                            KH.TENKHACHHANG AS TenKhachHang, 
                            PH.SOPHONG AS SoPhong, 
                            DV.TENDICHVU AS TenDichVu,
                            CTHD.SOLUONG AS SoLuongDichVu, 
                            CTHD.GIA AS GiaDichVu, 
                            (CTHD.SOLUONG * CTHD.GIA) AS TongTienDichVu, 
                            HD.NGAYLAP AS NgayLapHoaDon 
                            FROM HOADON HD
                            JOIN DATPHONG DP ON HD.MADATPHONG = DP.MADATPHONG
                            JOIN PHONG PH ON DP.MAPHONG = PH.MAPHONG
                            JOIN KHACHHANG KH ON DP.MAKHACHHANG = KH.MAKHACHHANG
                            LEFT JOIN CHITIETHOADON CTHD ON HD.MAHD = CTHD.MAHD
                            LEFT JOIN DICHVU DV ON CTHD.MADICHVU = DV.MADICHVU
                            WHERE HD.NGAYLAP BETWEEN @ngay_bat_dau AND @ngay_ket_thuc
                            AND CTHD.MADICHVU IS NOT NULL 
                            ORDER BY HD.NGAYLAP;";
                    DataSet ds = fn.GetData(query);
                    dataGridView1.DataSource = ds.Tables[0];

                    query = @"DECLARE @ngay_bat_dau NVARCHAR(250) = '" + dtpBatDau.Value.ToString("yyyy-MM-dd") + "'; "+
                            "DECLARE @ngay_ket_thuc NVARCHAR(250) = '" + dtpKetThuc.Value.ToString("yyyy-MM-dd") + "'; "+

                   @" SELECT DV.TENDICHVU AS TenDichVu,SUM(CTHD.SOLUONG * CTHD.GIA) AS TONGDOANHTHU  
                        FROM CHITIETHOADON CTHD
                        LEFT JOIN DICHVU DV ON CTHD.MADICHVU = DV.MADICHVU
                        LEFT JOIN HOADON HD ON CTHD.MAHD = HD.MAHD
                        WHERE HD.NGAYLAP BETWEEN @ngay_bat_dau AND @ngay_ket_thuc
                        GROUP BY DV.TENDICHVU
                        ORDER BY DV.TENDICHVU; ";


                    DataSet dtall = fn.GetData(query);

                    if (dtall != null && dtall.Tables.Count > 0 && dtall.Tables[0].Rows.Count > 0)
                    {
                        chartBDT.DataSource = dtall.Tables[0];

                        chartBDT.Series["ChartBDT"].XValueMember = "TenDichVu"; 
                        chartBDT.Series["ChartBDT"].YValueMembers = "TONGDOANHTHU";

                        chartBDT.Series["ChartBDT"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                        chartBDT.Series["ChartBDT"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

                        chartBDT.DataBind();
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    query = @"SELECT MONTH(HOADON.NGAYLAP) AS THANG, 
                           SUM(CHITIETHOADON.SOLUONG * CHITIETHOADON.GIA) AS DOANHTHU
                            FROM HOADON
                            INNER JOIN CHITIETHOADON ON HOADON.MAHD = CHITIETHOADON.MAHD
                            INNER JOIN DICHVU DV ON CHITIETHOADON.MADICHVU = DV.MADICHVU
                            GROUP BY MONTH(HOADON.NGAYLAP)
                            ORDER BY MONTH(HOADON.NGAYLAP);";

                    DataSet df = fn.GetData(query);
                    if (df != null && df.Tables.Count > 0 && df.Tables[0].Rows.Count > 0)
                    {
                        ChartBDC.Series["ChartBDC"].Points.Clear();

                        foreach (DataRow row in df.Tables[0].Rows)
                        {
                            int thang = Convert.ToInt32(row["THANG"]);
                            decimal doanhThu = Convert.ToDecimal(row["DOANHTHU"]);

                            ChartBDC.Series["ChartBDC"].Points.AddXY("" + thang, doanhThu);
                        }

                        ChartBDC.Series["ChartBDC"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column; // Dạng cột
                        ChartBDC.Series["ChartBDC"].IsValueShownAsLabel = true; // Hiển thị giá trị trên cột
                        ChartBDC.ChartAreas[0].AxisX.Title = "Tháng"; // Tiêu đề trục X
                        ChartBDC.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)"; // Tiêu đề trục Y
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                }
                else if (cbxThongke.Text == "Phòng")
                {
                    string ngayBatDau = dtpBatDau.Value.ToString("yyyy-MM-dd");
                    string ngayKetThuc = dtpKetThuc.Value.ToString("yyyy-MM-dd");
                    query = @" DECLARE @ngay_bat_dau NVARCHAR(250) = '" + ngayBatDau + @"';
                            DECLARE @ngay_ket_thuc NVARCHAR(250) = '" + ngayKetThuc + @"';
                    SELECT HD.MAHD AS MaHoaDon, 
                    KH.TENKHACHHANG AS TenKhachHang, 
                    PH.SOPHONG AS SoPhong, 
                    PH.LOAIPHONG AS LoaiPhong,
                    PH.LOAIGIUONG AS LoaiGiuong,
                    PH.GIA AS GiaPhong,
                    HD.NGAYLAP AS NgayLapHoaDon,
                    DP.NGAYCHECKIN AS NgayCheckIn,
                    DP.NGAYCHECKOUT AS NgayCheckOut
                    FROM HOADON HD
                    JOIN DATPHONG DP ON HD.MADATPHONG = DP.MADATPHONG
                    JOIN PHONG PH ON DP.MAPHONG = PH.MAPHONG
                    JOIN KHACHHANG KH ON DP.MAKHACHHANG = KH.MAKHACHHANG
                    LEFT JOIN CHITIETHOADON CTHD ON HD.MAHD = CTHD.MAHD
                    LEFT JOIN DICHVU DV ON CTHD.MADICHVU = DV.MADICHVU
                    WHERE HD.NGAYLAP BETWEEN @ngay_bat_dau AND @ngay_ket_thuc
                    AND CTHD.MADICHVU IS NOT NULL 
                    GROUP BY HD.MAHD, KH.TENKHACHHANG, PH.SOPHONG, 
                    PH.LOAIPHONG,  PH.LOAIGIUONG, PH.GIA, HD.NGAYLAP, 
                    DP.NGAYCHECKIN, DP.NGAYCHECKOUT ORDER BY  HD.NGAYLAP;";
                    DataSet ds = fn.GetData(query);
                    dataGridView1.DataSource = ds.Tables[0];

                    query = @"DECLARE @ngay_bat_dau DATE = '" + dtpBatDau.Value.ToString("yyyy-MM-dd") + "';" +
                               "DECLARE @ngay_ket_thuc DATE = '" + dtpKetThuc.Value.ToString("yyyy-MM-dd") + "';" +
                               @"SELECT PH.MAPHONG, SUM(HD.TONGTIEN) AS TONGDOANHTHU
                                FROM HOADON HD
                                JOIN DATPHONG DP ON HD.MADATPHONG = DP.MADATPHONG
                                JOIN PHONG PH ON DP.MAPHONG = PH.MAPHONG
                                WHERE HD.NGAYLAP BETWEEN @ngay_bat_dau AND @ngay_ket_thuc
                                GROUP BY PH.MAPHONG;";

                    DataSet dtall = fn.GetData(query);

                    if (dtall != null && dtall.Tables.Count > 0 && dtall.Tables[0].Rows.Count > 0)
                    {
                        chartBDT.DataSource = dtall.Tables[0];

                        chartBDT.Series["ChartBDT"].XValueMember = "MAPHONG"; // Cột dữ liệu tương ứng với trục X (DOANHTHU)
                        chartBDT.Series["ChartBDT"].YValueMembers = "TONGDOANHTHU"; // Cột dữ liệu tương ứng với trục Y (TONGDOANHTHU)

                        chartBDT.Series["ChartBDT"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                        chartBDT.Series["ChartBDT"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

                        chartBDT.DataBind();
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    query = @"DECLARE @ngay_bat_dau NVARCHAR(250) = '" + dtpBatDau.Value.ToString("yyyy-MM-dd") + "';" +
                            "DECLARE @ngay_ket_thuc NVARCHAR(250) = '" + dtpKetThuc.Value.ToString("yyyy-MM-dd") + "';" +
                            "SELECT MONTH(HOADON.NGAYLAP) AS THANG, " +
                            "SUM(HOADON.TONGTIEN) AS DOANHTHU " +
                            "FROM HOADON " +
                            "INNER JOIN DATPHONG ON HOADON.MADATPHONG = DATPHONG.MADATPHONG " +
                            "WHERE HOADON.NGAYLAP BETWEEN @ngay_bat_dau AND @ngay_ket_thuc " +
                            "GROUP BY MONTH(HOADON.NGAYLAP) " +
                            "ORDER BY MONTH(HOADON.NGAYLAP);";


                    DataSet df = fn.GetData(query);

                    if (df != null && df.Tables.Count > 0 && df.Tables[0].Rows.Count > 0)
                    {
                        ChartBDC.Series["ChartBDC"].Points.Clear();

                        foreach (DataRow row in df.Tables[0].Rows)
                        {
                            int thang = Convert.ToInt32(row["THANG"]);
                            decimal doanhThu = Convert.ToDecimal(row["DOANHTHU"]);

                            ChartBDC.Series["ChartBDC"].Points.AddXY("" + thang, doanhThu);
                        }

                        ChartBDC.Series["ChartBDC"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column; // Dạng cột
                        ChartBDC.Series["ChartBDC"].IsValueShownAsLabel = true;
                        ChartBDC.ChartAreas[0].AxisX.Title = "Tháng"; 
                        ChartBDC.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)"; 
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAll.Checked)
            {
                checkAll.Checked = checkDV.Checked = checkPhong.Checked = true;
                LoadcheckedAll();
            }
            else
            {
                checkDV.Checked = checkPhong.Checked = false;
                cbxThongke.Items.Clear();
            }
        }

        private void checkedcon_CheckedChanged(object sender, EventArgs e)
        {
            checkAll.Checked = checkDV.Checked && checkPhong.Checked;

            cbxThongke.Items.Clear();

            if (checkDV.Checked && !checkAll.Checked)
            {
                LoadcheckedDV();
            }
            if (checkPhong.Checked && !checkAll.Checked)
            {
                LoadcheckedPhong();
            }
            if (checkAll.Checked)
            {
                LoadcheckedAll();
            }
        }

        private void LoadcheckedPhong()
        {
            cbxThongke.Items.Clear();
            query = "SELECT MAPHONG FROM PHONG";
            DataSet ds = fn.GetData(query);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    cbxThongke.Items.Add(row["MAPHONG"].ToString());
                }
            }
        }

        private void LoadcheckedDV()
        {
            cbxThongke.Items.Clear();
            query = "SELECT MADICHVU FROM DICHVU";
            DataSet ds = fn.GetData(query);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    cbxThongke.Items.Add(row["MADICHVU"].ToString());
                }
            }
        }

        private void LoadcheckedAll()
        {
            cbxThongke.Items.Clear();
            cbxThongke.Items.Add("Tất cả");
            cbxThongke.Items.Add("Dịch vụ");
            cbxThongke.Items.Add("Phòng");
        }

        private void UC_Statistics_Load(object sender, EventArgs e)
        {
            LoadcheckedAll();
        }
    }
}