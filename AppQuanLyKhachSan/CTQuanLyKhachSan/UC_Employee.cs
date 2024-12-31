using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppQuanLyKhachSan.CTQuanLyKhachSan
{
    public partial class UC_Employee : UserControl
    {

        functions fn = new functions();
        String query;
        public UC_Employee()
        {
            InitializeComponent();
        }

        private void loadCN()
        {
            query = "Select MACHINHANH FROM CHINHANH;" +
                    "SELECT * FROM CHUCVU;" +
                    "SELECT NV.MANHANVIEN, NV.TENNHANVIEN, NV.SODIENTHOAI, NV.DIACHI, NV.GIOITINH, NV.CCCD, NV.EMAIL, CV.TENCHUCVU, NV.MACHINHANH, TK.TENDANGNHAP,TK.MATKHAU FROM NHANVIEN NV JOIN TAIKHOAN TK ON NV.MANHANVIEN = TK.MANHANVIEN JOIN CHUCVU CV ON CV.MACHUCVU = NV.MACHUCVU;";
            DataSet ds = fn.GetData(query);
            txtChinhanh.Text = ds.Tables[0].Rows[0][0].ToString();

            txtChucVu.Items.Clear();

            if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    txtChucVu.Items.Add(row["TENCHUCVU"].ToString());
                }
            }
            dataGridView1.DataSource = ds.Tables[2];
        }
        
        private void btnResignEmployee_Click(object sender, EventArgs e)
        {
            String maNV = txtMaNV.Text;
            String tenNV = txtNameEmployee.Text;
            String sDt = txtPhoneEmployee.Text;
            String DiaChi = txtAddressEmploy.Text;
            String cccd = txtCCCD.Text;
            String eMail = txtEmailEployee.Text;
            String gioiTinh = cbxGenderEmployee.Text;
            String ChucVu = txtChucVu.Text;

            query = @"DECLARE @MACHUCVU VARCHAR(100);" +
                    "SELECT @MACHUCVU = MACHUCVU FROM CHUCVU WHERE TENCHUCVU = N'" + ChucVu + "';" +
                    "INSERT INTO NHANVIEN (MANHANVIEN, TENNHANVIEN, SODIENTHOAI, DIACHI, GIOITINH, CCCD, EMAIL, MACHUCVU, MACHINHANH) VALUES ('" + maNV + "', N'" + tenNV + "', '" + sDt + "', N'" + DiaChi + "',N'" + gioiTinh + "','" + cccd + "','" + eMail + "',@MACHUCVU,'" + txtChinhanh.Text + "');";

            fn.SetData(query,"Thành Công");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Lấy giá trị từ các trường nhập liệu
            String maNV = txtMaNV.Text;
            String tenNV = txtNameEmployee.Text;
            String sDt = txtPhoneEmployee.Text;
            String DiaChi = txtAddressEmploy.Text;
            String eMail = txtEmailEployee.Text;
            String gioiTinh = cbxGenderEmployee.Text;
            String ChucVu = txtChucVu.Text;
            String chiNhanh = txtChinhanh.Text;  // Lấy giá trị chi nhánh
            String cccd = txtCCCD.Text;  // Lấy CCCD từ TextBox

            // Kiểm tra và đảm bảo chi nhánh không rỗng
            if (string.IsNullOrEmpty(chiNhanh))
            {
                MessageBox.Show("Chi nhánh không được để trống.");
                return;
            }

            // Truy vấn cập nhật thông tin nhân viên
            query = @"DECLARE @MACHUCVU VARCHAR(100);" +
                    "SELECT @MACHUCVU = MACHUCVU FROM CHUCVU WHERE TENCHUCVU = N'" + ChucVu + "'; " +
                    "UPDATE NHANVIEN SET " +
                    "TENNHANVIEN = N'" + tenNV + "', " +  // Đảm bảo thêm 'N' trước chuỗi để hỗ trợ Unicode
                    "SODIENTHOAI = '" + sDt + "', " +
                    "DIACHI = N'" + DiaChi + "', " +  // Đảm bảo Unicode
                    "EMAIL = '" + eMail + "', " +
                    "GIOITINH = N'" + gioiTinh + "', " +  // Đảm bảo Unicode
                    "MACHUCVU = @MACHUCVU, " +
                    "MACHINHANH = N'" + chiNhanh + "', " +  // Đảm bảo giá trị chi nhánh là hợp lệ
                    "CCCD = '" + cccd + "' " +  // Cập nhật CCCD
                    "WHERE MANHANVIEN = N'" + maNV + "';";  // Đảm bảo Unicode cho mã nhân viên

            // Gọi hàm thực hiện truy vấn
            fn.SetData(query, "Cập nhật thành công");
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Retrieve the employee ID from the input field
            String maNV = txtMaNV.Text;

            // Delete query
            query = "DELETE FROM TAIKHOAN WHERE MANHANVIEN = '"+ maNV + "';" +
                    "DELETE FROM NHANVIEN WHERE MANHANVIEN = '"+ maNV + "';";

            fn.SetData(query, "Xóa thành công");
        }


        private void btnLoad_Click(object sender, EventArgs e)
        {
            // Truy vấn để lấy dữ liệu nhân viên
            query = "SELECT NV.MANHANVIEN, NV.TENNHANVIEN, NV.SODIENTHOAI, NV.DIACHI, NV.GIOITINH, NV.CCCD, NV.EMAIL, CV.TENCHUCVU, NV.MACHINHANH, TK.TENDANGNHAP, TK.MATKHAU " +
                    "FROM NHANVIEN NV " +
                    "JOIN TAIKHOAN TK ON NV.MANHANVIEN = TK.MANHANVIEN " +
                    "JOIN CHUCVU CV ON CV.MACHUCVU = NV.MACHUCVU;";

            DataSet ds = fn.GetData(query);

            // Kiểm tra nếu DataSet có ít nhất một bảng
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dataGridView1.DataSource = dt;

                // Nếu mã nhân viên được nhập, tìm và điền thông tin vào các trường nhập liệu
                String maNV = txtMaNV.Text;
                if (!string.IsNullOrEmpty(maNV))
                {
                    DataRow[] selectedRows = dt.Select("MANHANVIEN = '" + maNV + "'");

                    if (selectedRows.Length > 0)
                    {
                        // Điền các thông tin vào TextBox từ dòng dữ liệu
                        txtNameEmployee.Text = selectedRows[0]["TENNHANVIEN"].ToString();
                        txtPhoneEmployee.Text = selectedRows[0]["SODIENTHOAI"].ToString();
                        txtAddressEmploy.Text = selectedRows[0]["DIACHI"].ToString();
                        txtEmailEployee.Text = selectedRows[0]["EMAIL"].ToString();
                        txtCCCD.Text = selectedRows[0]["CCCD"].ToString(); 
                        string gender = selectedRows[0]["GIOITINH"].ToString().Trim();
                        int indexGender = cbxGenderEmployee.Items.IndexOf(gender); 
                        if (indexGender >= 0)
                        {
                            cbxGenderEmployee.SelectedIndex = indexGender; 
                        }
                        else
                        {
                            cbxGenderEmployee.SelectedIndex = -1; 
                        }

                        // Chức vụ
                        txtChucVu.Text = selectedRows[0]["TENCHUCVU"].ToString();

                        // Chi nhánh
                        txtChinhanh.Text = selectedRows[0]["MACHINHANH"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhân viên với mã " + maNV);
                    }
                }
            }
            else
            {
                MessageBox.Show("Không thể tải dữ liệu nhân viên.");
            }
        }





        private void UC_Employee_Load(object sender, EventArgs e)
        {
            loadCN();
            cbxGenderEmployee.Items.Clear(); // Xóa các mục cũ nếu có
            cbxGenderEmployee.Items.Add("Nam");
            cbxGenderEmployee.Items.Add("Nữ");
            cbxGenderEmployee.Items.Add("Khác");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure that the clicked cell is not the header (Header row check)
            if (e.RowIndex >= 0)
            {
                // Get the values from the clicked row
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Fill the form fields with the data from the selected row
                txtMaNV.Text = row.Cells["MANHANVIEN"].Value.ToString();
                txtNameEmployee.Text = row.Cells["TENNHANVIEN"].Value.ToString();
                txtPhoneEmployee.Text = row.Cells["SODIENTHOAI"].Value.ToString();
                txtAddressEmploy.Text = row.Cells["DIACHI"].Value.ToString();
                txtCCCD.Text = row.Cells["CCCD"].Value.ToString();
                txtEmailEployee.Text = row.Cells["EMAIL"].Value.ToString();
                // Setting ComboBox values
                // Giả sử ComboBox đã được điền với các giá trị như "Nam", "Nữ", "Khác"
                string gender = row.Cells["GIOITINH"].Value.ToString().Trim(); // Lấy giá trị giới tính từ DataGridView và loại bỏ khoảng trắng thừa

                // Tìm chỉ mục của giá trị giới tính trong ComboBox
                int index = cbxGenderEmployee.Items.IndexOf(gender);

                if (index >= 0)
                {
                    cbxGenderEmployee.SelectedIndex = index; // Thiết lập ComboBox với chỉ mục được chọn
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy giá trị giới tính trong ComboBox
                    MessageBox.Show("Giới tính không có trong danh sách ComboBox.");
                }

                txtChucVu.SelectedItem = row.Cells["TENCHUCVU"].Value.ToString();

                // You can also use SelectedIndex or SelectedValue depending on how your ComboBox is set up
            }
        }

        private void btnDangkyTK_Click(object sender, EventArgs e)
        {
            DangkyTaiKhoan dk = new DangkyTaiKhoan();
            dk.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            query = "SELECT NV.MANHANVIEN, NV.TENNHANVIEN, NV.SODIENTHOAI, NV.DIACHI, NV.GIOITINH, NV.CCCD, NV.EMAIL, CV.TENCHUCVU, NV.MACHINHANH, TK.TENDANGNHAP, TK.MATKHAU " +
                    "FROM NHANVIEN NV " +
                    "JOIN TAIKHOAN TK ON NV.MANHANVIEN = TK.MANHANVIEN " +
                    "JOIN CHUCVU CV ON CV.MACHUCVU = NV.MACHUCVU " +
                    "WHERE NV.TENNHANVIEN LIKE N'"+txtName.Text+"%'";

            DataSet ds = fn.GetData(query);
            dataGridView1.DataSource = ds.Tables[0]; 

        }
    }
}
