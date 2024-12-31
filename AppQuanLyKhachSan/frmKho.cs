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
    public partial class frmKho : Form
    {
        functions fn = new functions();
        String query;
        public frmKho()
        {
            InitializeComponent();
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            String Mancc = txtMaNhaCC.Text;
            String TenNCC = cbxTenNCC.Text;
            String maKho = txtMaKho.Text;
            String TenHang = txtTenSP.Text;
            Int64 soLuong = Int64.Parse(txtSoLuong.Text);
            Int64 giaDonVi = Int64.Parse(txtGiaDonVi.Text);
            Int64 giatriTong = Int64.Parse(txtGiaTriTong.Text);
            String ngayNhapHang = txtNgayNhapHang.Text;
            String danhmuc = txtLoaiSP.Text;
            String ngayCapNhat = txtNgayCapNhat.Text;
            String donViTinh = txtDonViTinh.Text;

            query = " insert into KHO (MAKHO,TENHANG,SOLUONG,GIADONVI,GIATRITONG,DONVITINH,NGAYNHAPHANG,DANHMUC,NGAYCAPNHAT,MANCC) values ('"+ maKho + "',N'"+TenHang+"',"+soLuong+","+giaDonVi+","+giatriTong+",'"+ donViTinh +"','"+ngayNhapHang+"',N'"+danhmuc+"','"+ngayCapNhat+"','"+Mancc+"')";
            fn.SetData(query,"Thêm thành công");
            frmKho_Load(this,null);
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy dữ liệu từ các TextBox và ComboBox
                String maKho = txtMaKho.Text;
                String TenHang = txtTenSP.Text;
                Int64 soLuong = Int64.Parse(txtSoLuong.Text);
                Int64 giaDonVi = Int64.Parse(txtGiaDonVi.Text);
                Int64 giatriTong = Int64.Parse(txtGiaTriTong.Text);
                String ngayNhapHang = txtNgayNhapHang.Text;
                String danhmuc = txtLoaiSP.Text;
                String ngayCapNhat = txtNgayCapNhat.Text;
                String Mancc = txtMaNhaCC.Text;
                String donViTinh = txtDonViTinh.Text;

                // Truy vấn cập nhật
                query = $"UPDATE KHO SET TENHANG = N'{TenHang}', SOLUONG = {soLuong}, GIADONVI = {giaDonVi}, GIATRITONG = {giatriTong}, DONVITINH = {donViTinh} , NGAYNHAPHANG = '{ngayNhapHang}', DANHMUC = N'{danhmuc}', NGAYCAPNHAT = '{ngayCapNhat}', MANCC = '{Mancc}' WHERE MAKHO = '{maKho}'";

                // Thực hiện cập nhật
                fn.SetData(query, "Cập nhật thành công");

                // Tải lại dữ liệu trong GridView
                frmKho_Load(this, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy mã kho từ TextBox
                String maKho = txtMaKho.Text;

                if (string.IsNullOrEmpty(maKho))
                {
                    MessageBox.Show("Vui lòng chọn kho cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xác nhận xóa
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa kho này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Truy vấn xóa
                    query = $"DELETE FROM KHO WHERE MAKHO = '{maKho}'";

                    // Thực hiện xóa
                    fn.SetData(query, "Xóa thành công");

                    // Tải lại dữ liệu trong GridView
                    frmKho_Load(this, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void frmKho_Load(object sender, EventArgs e)
        {
            query = "select MAKHO, TENHANG,SOLUONG,GIADONVI, GIATRITONG,DONVITINH,NGAYNHAPHANG,NGAYCAPNHAT,DANHMUC, KHO.MANCC,TENNCC,DIACHI,EMAIL,SDT from KHO JOIN NHACUNGCAP ON KHO.MANCC = NHACUNGCAP.MANCC";
            DataSet ds = fn.GetData(query);
            dtGVKho.DataSource = ds.Tables[0];
            LoadTenNCC();

            // Tự động lấy mã NCC cho nhà cung cấp đầu tiên nếu có dữ liệu
            if (cbxTenNCC.Items.Count > 0)
            {
                cbxTenNCC.SelectedIndex = 0;  // Chọn nhà cung cấp đầu tiên
                LoadMaNCC();  // Lấy mã NCC cho nhà cung cấp đó
            }
        }

        private void dtGVKho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu dòng được chọn không phải là dòng tiêu đề và không phải dòng trống
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtGVKho.Rows[e.RowIndex];

                // Lấy dữ liệu từ các cột trong hàng được chọn
                txtMaKho.Text = row.Cells["MAKHO"].Value?.ToString();
                txtTenSP.Text = row.Cells["TENHANG"].Value?.ToString();
                txtSoLuong.Text = row.Cells["SOLUONG"].Value?.ToString();
                txtGiaDonVi.Text = row.Cells["GIADONVI"].Value?.ToString();
                txtGiaTriTong.Text = row.Cells["GIATRITONG"].Value?.ToString();
                txtNgayNhapHang.Text = row.Cells["NGAYNHAPHANG"].Value?.ToString();
                txtNgayCapNhat.Text = row.Cells["NGAYCAPNHAT"].Value?.ToString();
                txtLoaiSP.Text = row.Cells["DANHMUC"].Value?.ToString();
                txtMaNhaCC.Text = row.Cells["MANCC"].Value?.ToString();
                cbxTenNCC.Text = row.Cells["TENNCC"].Value?.ToString();
                txtDonViTinh.Text = row.Cells["DONVITINH"].Value?.ToString();
            }
        }

        private void LoadTenNCC()
        {
            try
            {
                // Câu truy vấn lấy danh sách tên nhà cung cấp
                query = "SELECT TENNCC FROM NHACUNGCAP";

                // Lấy dữ liệu từ database
                DataSet ds = fn.GetData(query);

                // Xóa các mục đã có trong ComboBox (nếu có)
                cbxTenNCC.Items.Clear();

                // Duyệt qua từng dòng dữ liệu và thêm vào ComboBox
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    cbxTenNCC.Items.Add(row["TENNCC"].ToString());
                }

                // Đặt mục mặc định (nếu cần)
                if (cbxTenNCC.Items.Count > 0)
                {
                    cbxTenNCC.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách nhà cung cấp: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbxTenNCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxTenNCC.SelectedItem != null)  // Kiểm tra nếu có lựa chọn
            {
                LoadMaNCC();  // Gọi phương thức để lấy mã NCC
            }
        }
        private void LoadMaNCC()
        {
            try
            {
                string tenNCC = cbxTenNCC.SelectedItem.ToString(); // Lấy tên nhà cung cấp từ ComboBox

                // Truy vấn lấy MANCC dựa trên TENNCC
                query = $"SELECT MANCC FROM NHACUNGCAP WHERE TENNCC = N'{tenNCC}'";

                // Lấy dữ liệu từ cơ sở dữ liệu
                DataSet ds = fn.GetData(query);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    // Lấy MANCC từ kết quả
                    string maNCC = ds.Tables[0].Rows[0]["MANCC"].ToString();

                    // Cập nhật vào TextBox
                    txtMaNhaCC.Text = maNCC;
                }
                else
                {
                    // Nếu không tìm thấy, hiển thị thông báo lỗi
                    MessageBox.Show("Không tìm thấy mã nhà cung cấp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy mã nhà cung cấp: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNcc(object sender, EventArgs e)
        {
            NhaCungCap ncc = new NhaCungCap();
            ncc.ShowDialog();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
           btnThongKeKho thongke = new btnThongKeKho();
            thongke.ShowDialog();
        }
    }
}
