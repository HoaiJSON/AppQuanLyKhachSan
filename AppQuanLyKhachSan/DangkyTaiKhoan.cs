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
    public partial class DangkyTaiKhoan : Form
    {
        functions fn = new functions();
        String query;
        public DangkyTaiKhoan()
        {
            InitializeComponent();
        }
        private void DangkyTaiKhoan_Load(object sender, EventArgs e)
        {
            loadMaNV();
            LoadTaiKhoan();
        }

        private void loadMaNV()
        {
            query = "SELECT MANHANVIEN, TENNHANVIEN FROM NHANVIEN";

            // Lấy dữ liệu từ cơ sở dữ liệu
            DataSet ds = fn.GetData(query);

            if (ds != null && ds.Tables.Count > 0)
            {
                // Gán dữ liệu vào ComboBox
                cbxMaNV.DataSource = ds.Tables[0];
                cbxMaNV.DisplayMember = "MANHANVIEN"; // Hiển thị mã nhân viên
                cbxMaNV.ValueMember = "MANHANVIEN";   // Lấy giá trị mã nhân viên

                // Gán sự kiện SelectedIndexChanged để lấy tên nhân viên
                cbxMaNV.SelectedIndexChanged += (s, e) =>
                {
                    if (cbxMaNV.SelectedValue != null)
                    {
                        // Lấy mã nhân viên đang chọn
                        string maNhanVien = cbxMaNV.SelectedValue.ToString();

                        // Tìm tên nhân viên tương ứng
                        DataRow[] rows = ds.Tables[0].Select($"MANHANVIEN = '{maNhanVien}'");
                        if (rows.Length > 0)
                        {
                            string tenNhanVien = rows[0]["TENNHANVIEN"].ToString();
                            txtTenNV.Text = tenNhanVien;
                        }
                    }
                };
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtUserEmployee.Text.Trim();
            string matKhau = txtPasswordEmployee.Text.Trim();
            String maNV = cbxMaNV.Text;
            String tenNV = txtTenNV.Text;

            // Kiểm tra nếu các trường dữ liệu không trống
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin tài khoản!");
                return;
            }

            query = "INSERT INTO TAIKHOAN(TENDANGNHAP,MATKHAU,MANHANVIEN) VALUES " +
                    "('"+tenDangNhap+"','"+matKhau+"','"+maNV+"')";

            fn.SetData(query, "Tài khoản đã đượcc thêm thành công");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            query = "DELETE FROM TAIKHOAN WHERE MANHANVIEN = '"+cbxMaNV+"'";
            fn.SetData(query, "Xóa thành công");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            query = "UPDATE TAIKHOAN SET MATKHAU ='"+txtPasswordEmployee.Text+"',TENDANGNHAP = '"+txtUserEmployee.Text+"' WHERE MANHANVIEN = 'NV001'";
            fn.SetData(query, "Cập nhật thành công");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            txtPasswordEmployee.Clear();
            txtUserEmployee.Clear();
        }
        
        private void LoadTaiKhoan()
        {
            query = "SELECT * FROM TAIKHOAN";
           DataSet ds = fn.GetData(query);
            dataGridView1.DataSource = ds.Tables[0];

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the click is valid (not on headers or outside the grid)
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Access the clicked row
                DataGridViewRow clickedRow = dataGridView1.Rows[e.RowIndex];

                // Retrieve values from the row cells
                string tenDangNhap = clickedRow.Cells["TENDANGNHAP"].Value?.ToString();
                string matKhau = clickedRow.Cells["MATKHAU"].Value?.ToString();
                string maNhanVien = clickedRow.Cells["MANHANVIEN"].Value?.ToString();

                // Display data in input controls (assuming you have these text boxes)
                txtUserEmployee.Text = tenDangNhap;
                txtPasswordEmployee.Text = matKhau;
                cbxMaNV.Text = maNhanVien;

            }
        }

    }
}
