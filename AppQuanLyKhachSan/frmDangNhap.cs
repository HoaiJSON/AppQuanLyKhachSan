using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppQuanLyKhachSan
{
    public partial class frmDangNhap : Form
    {
        functions fn = new functions();
        String query;
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();

            if(txtUserName.Text != "" && txtPassword.Text != "")
            {
                query = "select TENDANGNHAP, MATKHAU,MANHANVIEN from TAIKHOAN where TENDANGNHAP = '" + txtUserName.Text + "' AND MATKHAU = '" + txtPassword.Text + "'";

                DataSet ds = fn.GetData(query);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    db.manv = ds.Tables[0].Rows[0]["MANHANVIEN"].ToString();
                }
                if (ds.Tables[0].Rows.Count != 0)
                {
                    labelError.Visible = false;
                    this.Hide();
                    db.Show();
                    txtUserName.Clear();
                }
                else
                {
                    labelError.Visible = true;
                    txtPassword.Clear();
                }

            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!!", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // Hiển thị mật khẩu
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                // Ẩn mật khẩu
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
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
            // Mặc định ẩn mật khẩu khi form load
            txtPassword.UseSystemPasswordChar = true;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            txtPassword.Clear();
            txtUserName.Clear();
        }
    }
}
