using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing; // Thư viện in ấn
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AppQuanLyKhachSan
{
    public partial class Payment : Form
    {
        functions fn = new functions();
        String query;
        string printContent = "";
        public Payment()
        {
            InitializeComponent();
        }

        public String mahoadon;
        public String tenkhachhang;
        public String makhachhang;
        public DateTime ngaycheckin;
        public DateTime ngaycheckout;
        public Int64 giaphong;
        public Int64 sodienthoai;
        public String maDatPhong;
        public String maPhong;
        
        private void Payment_Load(object sender, EventArgs e)
        {
            txtMaHD.Text = mahoadon;
            txtPaymentTenKH.Text = tenkhachhang;
            txtPaymentCheckout.Text = ngaycheckout.ToString("yyyy-MM-dd");
            txtPaymentCheckin.Text = ngaycheckin.ToString("yyyy-MM-dd");
            txtPaymentPhone.Text = sodienthoai.ToString();
            txtPaymentPrice.Text = giaphong.ToString();

            query = @"DECLARE @NGAYCHECKIN DATE;
          SELECT @NGAYCHECKIN = NGAYCHECKIN
          FROM DATPHONG
          WHERE MAKHACHHANG = '" + makhachhang + @"'
            AND TRANGTHAI = N'CHƯA THANH TOÁN';
            SELECT DICHVU.TENDICHVU, 
                 DICHVU.GIA, 
                 CHITIETDICHVU.SOLUONG, 
                 CHITIETDICHVU.NGAYSUDUNG
          FROM DICHVU
          JOIN CHITIETDICHVU ON DICHVU.MADICHVU = CHITIETDICHVU.MADICHVU
          JOIN KHACHHANG ON KHACHHANG.MAKHACHHANG = CHITIETDICHVU.MAKHACHHANG
          JOIN DATPHONG ON KHACHHANG.MAKHACHHANG = DATPHONG.MAKHACHHANG
          WHERE DATPHONG.MAPHONG = '" + maPhong + @"'
            AND DATPHONG.TRANGTHAI = N'CHƯA THANH TOÁN'
            AND DATPHONG.NGAYCHECKOUT >= @NGAYCHECKIN
            AND CHITIETDICHVU.MADATPHONG = DATPHONG.MADATPHONG; ";


            DataSet ds = fn.GetData(query);
            dtgvServiceHD.DataSource = ds.Tables[0];

            try
            {
                // Lấy ngày checkin và checkout
                DateTime ngayCheckin = DateTime.Parse(txtPaymentCheckin.Text);
                DateTime ngayCheckout = DateTime.Parse(txtPaymentCheckout.Text);

                // Kiểm tra nếu ngày checkout sớm hơn ngày checkin
                if (ngayCheckout < ngayCheckin)
                {
                    MessageBox.Show("Ngày checkout không thể sớm hơn ngày checkin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Chuyển đổi giá phòng
                Int64 Giaphong = Convert.ToInt64(giaphong.ToString());

                // Tính số ngày ở
                TimeSpan duration = ngayCheckout - ngayCheckin;
                Int64 soNgayO = duration.Days;

                // Nếu ngày ở nhỏ hơn 1, mặc định là 1 ngày (hoặc xử lý logic khác)
                if (soNgayO == 0)
                {
                    soNgayO = 1;
                }

                // Tính tổng tiền phòng
                Int64 tongTienPhong = soNgayO * Giaphong;

                // Tính tổng tiền dịch vụ
                Int64 tongTienDichVu = 0;
                foreach (DataGridViewRow row in dtgvServiceHD.Rows)
                {
                    if (!row.IsNewRow) // Bỏ qua dòng trống cuối cùng của DataGridView
                    {
                        int soLuong = Convert.ToInt32(row.Cells["quantity"].Value);
                        int giaDichVu = Convert.ToInt32(row.Cells["Price"].Value);
                        tongTienDichVu += soLuong * giaDichVu;
                    }
                }

                // Tính tổng tiền (tiền phòng + tiền dịch vụ)
                Int64 tongTien = tongTienPhong + tongTienDichVu;

                txtTongTien.Text = tongTien.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi trong quá trình tính toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoadMaNVserviceCus();

            // Khởi tạo PrintDocument và PrintPreviewDialog
            printDocument1 = new PrintDocument();
            printPreviewDialog1 = new PrintPreviewDialog();

            // Đăng ký sự kiện PrintPage cho PrintDocument
            printDocument1.PrintPage += new PrintPageEventHandler(this.printDocument1_PrintPage);

        }


        private void LoadMaNVserviceCus()
        {
            String query = "select MANHANVIEN FROM NHANVIEN";
            DataSet ds = fn.GetData(query);

            txtPaymentMaNV.Items.Clear();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    txtPaymentMaNV.Items.Add(row["MANHANVIEN"].ToString());
                }
            }
        }



        private void txtPaymentMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            String tennv = txtPaymentMaNV.SelectedItem.ToString();
            query = "SELECT TENNHANVIEN FROM NHANVIEN WHERE MANHANVIEN = '" + tennv + "'";
            DataSet ds = fn.GetData(query);

            txtPaymentNameNV.Text = ds.Tables[0].Rows[0][0].ToString();

        }

        Int64 slDV;
        Int64 GiaDV;
        private void dtgvServiceHD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgvServiceHD.Rows[e.RowIndex].Cells[e.RowIndex].Value != null)
            {
                slDV = Int64.Parse(dtgvServiceHD.Rows[e.RowIndex].Cells[1].Value.ToString());
                GiaDV = Int64.Parse(dtgvServiceHD.Rows[e.RowIndex].Cells[2].Value.ToString());

            }
        }


        private void btnSaveHD_Click(object sender, EventArgs e)
        {
            String maHD = txtMaHD.Text;
            String maNV = txtPaymentMaNV.Text;
            String maKH = makhachhang;
            Int64 Tong = Int64.Parse(txtTongTien.Text);
            String madP = maDatPhong;

            // Chèn thông tin hóa đơn
            query = "INSERT INTO HOADON (MAHD, MANHANVIEN, MADATPHONG, MAKHACHHANG, TONGTIEN, NGAYLAP) VALUES" +
                    "('" + maHD + "', '" + maNV + "', '" + madP + "', '" + maKH + "', '" + Tong + "', GETDATE());";
            fn.SetData(query, "Thành công");

            // Duyệt qua từng dịch vụ trong DataGridView để thêm chi tiết dịch vụ
            foreach (DataGridViewRow row in dtgvServiceHD.Rows)
            {
                if (!row.IsNewRow) // Bỏ qua dòng trống cuối cùng của DataGridView
                {
                    // Kiểm tra xem có thông tin dịch vụ không
                    if (row.Cells["Namepayment"].Value != null && !string.IsNullOrEmpty(row.Cells["Namepayment"].Value.ToString()))
                    {
                        string tenDichVu = row.Cells["Namepayment"].Value.ToString();
                        int soLuong = Convert.ToInt32(row.Cells["quantity"].Value);
                        int gia = Convert.ToInt32(row.Cells["Price"].Value);

                        // Lấy mã dịch vụ từ tên dịch vụ
                        query = @"DECLARE @MADICHVU VARCHAR(100) "+
                                "SELECT @MADICHVU = MADICHVU FROM DICHVU WHERE TENDICHVU = N'" + tenDichVu + "';"+
                                "SELECT COUNT(1) FROM CHITIETHOADON WHERE MAHD = '" + maHD + "' AND MADICHVU = @MADICHVU;";
                        if (fn.GetValue(query) == "0")  // Nếu không tồn tại cặp (MAHD, MADICHVU)
                        {
                            query = @"DECLARE @MADICHVU VARCHAR(100) " +
                                    @"SELECT @MADICHVU = MADICHVU FROM DICHVU WHERE TENDICHVU = N'" + tenDichVu + "';" +
                                    "INSERT INTO CHITIETHOADON (MAHD, MADICHVU, SOLUONG, GIA) " +
                                    "VALUES ('" + maHD + "', @MADICHVU, '" + soLuong + "', '" + gia + "'); ";

                            fn.SetData(query, "Thành công");
                        }
                        else
                        {
                            // Nếu dịch vụ đã có trong hóa đơn, có thể cập nhật số lượng hoặc báo lỗi
                            MessageBox.Show("Dịch vụ này đã có trong hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }

            query = "UPDATE PHONG SET TRANGTHAI = N'TRỐNG', KHACHHANGONLINE = NULL  WHERE MAPHONG IN (SELECT MAPHONG FROM DATPHONG WHERE MAPHONG = '" + maPhong + "');" +
                    @"UPDATE DATPHONG SET TRANGTHAI = N'ĐÃ THANH TOÁN' WHERE MADATPHONG = '" + maDatPhong + "';";
            fn.SetData(query, "Thanh toán thành công");

            this.Close();
        }

        private void btnPrintHD_Click(object sender, EventArgs e)
        {
            if (dtgvServiceHD.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dtgvServiceHD.SelectedRows[0];

                // Lấy dữ liệu từ các TextBox
                string maHD = txtMaHD.Text;
                string tenKH = txtPaymentTenKH.Text;
                string sdtKH = txtPaymentPhone.Text;
                string soPhong = txtPaymentCheckin.Text;
                string loaiPhong = txtPaymentCheckout.Text;
                string ngayCheckin = txtPaymentCheckin.Text;
                string ngayCheckout = txtPaymentCheckout.Text;
                string tongTien = txtTongTien.Text;

                // Gán sự kiện PrintPage để vẽ hóa đơn
                printDocument1.PrintPage += (s, ev) =>
                {
                    Graphics g = ev.Graphics;
                    Font titleFont = new Font("Arial", 24, FontStyle.Bold); // Font tiêu đề
                    Font contentFont = new Font("Arial", 16); // Font nội dung
                    Brush brush = Brushes.Black;

                    // Vẽ phần nền cho logo
                    g.FillRectangle(Brushes.LightGray, new Rectangle(50, 20, 750, 150)); // Tô màu nền cho phần logo

                    // Vẽ logo
                    Image logo = Image.FromFile("D:\\QuanLyKhachSan\\AppQuanLyKhachSan\\Image\\logoks.png");
                    g.DrawImage(logo, new Rectangle(50, 60, 150, 50));

                    // Vẽ thông tin tiêu đề
                    g.DrawString("KHÁCH SẠN", titleFont, brush, new PointF(200, 60));
                    g.DrawString("DNC", titleFont, brush, new PointF(200, 100));
                    g.DrawString("DNC ĐẦU TƯ VÀ PHÁT TRIỂN", contentFont, brush, new PointF(450, 60));
                    g.DrawString("+84 912 345 678", contentFont, brush, new PointF(450, 90));
                    g.DrawString("Quận Ninh Kiều, TP.Cần Thơ", contentFont, brush, new PointF(450, 120));

                    // Vẽ tiêu đề hóa đơn
                    g.DrawString("Phiếu Thu", titleFont, Brushes.Red, new PointF(360, 180));
                    g.DrawString($"Mã hóa đơn: #{maHD}", contentFont, brush, new PointF(50, 220));
                    g.DrawString($"Ngày: {DateTime.Now:dd/MM/yyyy}", contentFont, brush, new PointF(550, 220));
                    g.DrawString($"Tên khách hàng: {tenKH}", contentFont, brush, new PointF(50, 250));


                    // Lằn kẻ ngang dưới tiêu đề hóa đơn
                    g.DrawLine(Pens.Black, new Point(50, 290), new Point(750, 290)); // Đường kẻ ngang

                    // Vẽ bảng dịch vụ
                    g.DrawString("Mục", contentFont, brush, new PointF(50, 300));
                    g.DrawString("Số lượng/Số giờ", contentFont, brush, new PointF(200, 300));
                    g.DrawString("Đơn giá", contentFont, brush, new PointF(450, 300));
                    g.DrawString("Thành tiền", contentFont, brush, new PointF(630, 300));

                    g.DrawLine(Pens.Black, new Point(50, 330), new Point(750, 330)); // Đường kẻ ngang

                    // Khởi tạo biến để xác định vị trí y cho từng dòng dịch vụ
                    int yPosition = 340; // Vị trí ban đầu cho dòng dịch vụ đầu tiên

                    // Duyệt qua tất cả các dịch vụ trong DataGridView
                    foreach (DataGridViewRow row in dtgvServiceHD.Rows)
                    {
                        if (!row.IsNewRow) // Bỏ qua dòng trống cuối cùng của DataGridView
                        {
                            // Lấy thông tin dịch vụ từ các ô trong dòng
                            string tenDV = row.Cells["Namepayment"].Value.ToString();
                            string soLuongDV = row.Cells["quantity"].Value.ToString();
                            string giaDV = row.Cells["Price"].Value.ToString();
                            string thanhTienDV = (Convert.ToInt32(soLuongDV) * Convert.ToInt32(giaDV)).ToString();

                            // Vẽ thông tin dịch vụ
                            g.DrawString(tenDV, contentFont, brush, new PointF(50, yPosition));
                            g.DrawString(soLuongDV, contentFont, brush, new PointF(200, yPosition));
                            g.DrawString(giaDV, contentFont, brush, new PointF(450, yPosition));
                            g.DrawString(thanhTienDV, contentFont, brush, new PointF(630, yPosition));

                            // Cập nhật vị trí y cho dòng tiếp theo
                            yPosition += 30; // Mỗi dịch vụ cách nhau 30px
                        }
                    }

                    // Lằn kẻ ngang dưới danh sách dịch vụ
                    g.DrawLine(Pens.Black, new Point(50, yPosition), new Point(750, yPosition));

                    // Vẽ tổng tiền
                    g.DrawString($"Tổng cộng: {tongTien}", titleFont, Brushes.Red, new PointF(420, yPosition + 30));
                };

                // Hiển thị Print Preview trước khi in
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Debug nội dung in
            Console.WriteLine("Đang in nội dung: " + printContent);

            if (!string.IsNullOrEmpty(printContent))
            {
                Font printFont = new Font("Arial", 10);
                e.Graphics.DrawString(printContent, printFont, Brushes.Black, new PointF(50, 50));
            }
            else
            {
                e.Graphics.DrawString("Không có nội dung để in.", new Font("Arial", 12), Brushes.Black, new PointF(50, 50));
            }
        }
    }

}
