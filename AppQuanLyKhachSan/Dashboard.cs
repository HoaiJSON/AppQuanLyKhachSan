using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace AppQuanLyKhachSan
{
    public partial class Dashboard : Form
    {
        functions fn = new functions();
        String query;
        public Dashboard()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Dashboard_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;

            // Sử dụng Timer để tăng dần độ mờ của form
            Timer fadeTimer = new Timer();
            fadeTimer.Interval = 10;  // Tốc độ thay đổi opacity (mỗi 10ms)
            fadeTimer.Tick += (s, args) =>
            {
                if (this.Opacity < 1)
                {
                    this.Opacity += 0.05;  // Tăng độ mờ lên 0.05 mỗi lần
                }
                else
                {
                    fadeTimer.Stop(); // Dừng Timer khi đạt opacity = 1 (hiển thị đầy đủ)
                }
            };

            fadeTimer.Start();

            uC_ThemPhong1.Visible = false;
            uC_Customer1.Visible = false;
            uC_Service1.Visible = false;
            uC_CheckOut1.Visible = false;
            uC_Employee1.Visible = false;
            uC_Statistics1.Visible = false;
            uC_HoaDon1.Visible = false;
            btnThemPhong.PerformClick();

        }

        private void btnThemPhong_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnThemPhong.Left + 70;
            uC_ThemPhong1.Visible = true;
            uC_ThemPhong1.BringToFront();
            uC_ThemPhong1.ReloadData();
        }

        private void btnDangKyKhachHang_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnKhachHang.Left + 80;
            uC_Customer1.Visible = true;
            uC_Customer1.BringToFront();
            uC_Customer1.ReloadData();
        }

        private void btnDichVu_Click(object sender, EventArgs e)
        {
            query = "SELECT CV.TENCHUCVU FROM NHANVIEN NV JOIN CHUCVU CV ON NV.MACHUCVU = CV.MACHUCVU WHERE NV.MANHANVIEN = '" + manv + "'";
            DataSet ds = fn.GetData(query);
            String chucvu = ds.Tables[0].Rows[0]["TENCHUCVU"].ToString();

            if (chucvu == "Quản lý")
            {
                PanelMoving.Left = btnDichVu.Left + 70;
            uC_Service1.Visible = true;
            uC_Service1.BringToFront();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vui lòng đăng nhập với quyền quản trị!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnHoadon_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnThanhToan.Left + 75;
            uC_CheckOut1.Visible = true;
            uC_CheckOut1.BringToFront();
            uC_CheckOut1.ReloadData();
        }

        public String manv;
        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            query = "SELECT CV.TENCHUCVU FROM NHANVIEN NV JOIN CHUCVU CV ON NV.MACHUCVU = CV.MACHUCVU WHERE NV.MANHANVIEN = '"+ manv + "'";
            DataSet ds = fn.GetData(query);
            String chucvu = ds.Tables[0].Rows[0]["TENCHUCVU"].ToString();

            if (chucvu == "Quản lý")
            {
                PanelMoving.Left = btnNhanVien.Left + 80;
                uC_Employee1.Visible = true;
                uC_Employee1.BringToFront();

            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vui lòng đăng nhập với quyền quản trị!","Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnThongKe.Left + 80;
            uC_Statistics1.Visible = true;
            uC_Statistics1.BringToFront();
        }

        private void btnLoadForm_Click(object sender, EventArgs e)
        {
            uC_ThemPhong1.ReloadData();
            uC_Customer1.ReloadData();
            uC_CheckOut1.ReloadData();
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            query = "SELECT CV.TENCHUCVU FROM NHANVIEN NV JOIN CHUCVU CV ON NV.MACHUCVU = CV.MACHUCVU WHERE NV.MANHANVIEN = '" + manv + "'";
            DataSet ds = fn.GetData(query);
            String chucvu = ds.Tables[0].Rows[0]["TENCHUCVU"].ToString();

            if (chucvu == "Quản lý")
            {
                PanelMoving.Left = btnHoaDon.Left + 70;
            uC_HoaDon1.Visible = true;
            uC_HoaDon1.BringToFront();

            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vui lòng đăng nhập với quyền quản trị!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
            frmDangNhap fn = new frmDangNhap();
            fn.ShowDialog();
        }

        private void uC_HoaDon1_Load(object sender, EventArgs e)
        {

        }
    }
}
