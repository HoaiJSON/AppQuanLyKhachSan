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
    public partial class NhaCungCap : Form
    {
        functions fn = new functions();
        String query;

        public NhaCungCap()
        {
            InitializeComponent();
            LoadData(); // Tải dữ liệu khi form được mở
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string maNCC = txtMaNhaCC.Text;
                string tenNCC = txtTenNCC.Text;
                string diaChi = txtDiaChi.Text;
                string email = txtEmail.Text;
                int sdt = int.Parse(txtSoDienThoai.Text);

                query = $"INSERT INTO NHACUNGCAP (MANCC, TENNCC, DIACHI, EMAIL, SDT) VALUES ('{maNCC}', N'{tenNCC}', N'{diaChi}', '{email}', {sdt})";
                fn.SetData(query, "Thêm nhà cung cấp thành công!");
                LoadData(); // Tải lại dữ liệu sau khi thêm
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                string maNCC = txtMaNhaCC.Text;
                string tenNCC = txtTenNCC.Text;
                string diaChi = txtDiaChi.Text;
                string email = txtEmail.Text;
                int sdt = int.Parse(txtSoDienThoai.Text);

                query = $"UPDATE NHACUNGCAP SET TENNCC = N'{tenNCC}', DIACHI = N'{diaChi}', EMAIL = '{email}', SDT = {sdt} WHERE MANCC = '{maNCC}'";
                fn.SetData(query, "Cập nhật nhà cung cấp thành công!");
                LoadData(); // Tải lại dữ liệu sau khi cập nhật
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maNCC = txtMaNhaCC.Text;

                query = $"DELETE FROM NHACUNGCAP WHERE MANCC = '{maNCC}'";
                fn.SetData(query, "Xóa nhà cung cấp thành công!");
                LoadData(); // Tải lại dữ liệu sau khi xóa
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadData(); // Tải lại dữ liệu
        }

        private void LoadData()
        {
            try
            {
                query = "SELECT * FROM NHACUNGCAP"; 
                DataSet ds = fn.GetData(query);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Kiểm tra xem người dùng có nhấp vào một hàng hợp lệ không
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    // Lấy dữ liệu từ hàng được chọn và đưa vào các TextBox
                    txtMaNhaCC.Text = row.Cells["MANCC"].Value.ToString();
                    txtTenNCC.Text = row.Cells["TENNCC"].Value.ToString();
                    txtDiaChi.Text = row.Cells["DIACHI"].Value.ToString();
                    txtEmail.Text = row.Cells["EMAIL"].Value.ToString();
                    txtSoDienThoai.Text = row.Cells["SDT"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

    }
}
