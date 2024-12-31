using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppQuanLyKhachSan.CTQuanLyKhachSan
{
    public partial class UC_ThemPhong : UserControl
    {
        functions fn = new functions();
        String query;
        public UC_ThemPhong()
        {
            InitializeComponent();
        }

        private void UC_ThemPhong_Load(object sender, EventArgs e)
        {
            query = "SELECT MAPHONG ,SOPHONG,LOAIPHONG,LOAIGIUONG,HINHANH, GIA,MACHINHANH,TRANGTHAI FROM PHONG; ";
            DataSet ds = fn.GetData(query);
            dtGridViewThemPhong.DataSource = ds.Tables[0];

            DataGridViewImageColumn pic = new DataGridViewImageColumn();
            pic = (DataGridViewImageColumn)dtGridViewThemPhong.Columns[4];
            pic.ImageLayout = DataGridViewImageCellLayout.Zoom;

            LoadChiNhanh();
        }

        private void btn_AddRoom_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoomNumber.Text) ||
                string.IsNullOrWhiteSpace(CbxRoomType.Text) ||
                string.IsNullOrWhiteSpace(CbxBed.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin phòng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maPhong = txtMaPhong.Text;
            string roomNumber = txtRoomNumber.Text;
            string type = CbxRoomType.Text;
            string bed = CbxBed.Text;

            // Kiểm tra và chuyển đổi giá
            if (!Int64.TryParse(txtPrice.Text, out Int64 price))
            {
                MessageBox.Show("Giá phòng không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maChiNhanh = txtChiNhanh.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(maChiNhanh))
            {
                MessageBox.Show("Vui lòng chọn chi nhánh!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chuyển đổi hình ảnh
            byte[] anh = ImageToByteArray(pictureBox1);

            // Tạo câu lệnh SQL với tham số
            query = "INSERT INTO PHONG (MAPHONG, SOPHONG, LOAIPHONG, LOAIGIUONG, HINHANH, GIA, MACHINHANH) VALUES (@MaPhong, @SoPhong, @LoaiPhong, @LoaiGiuong, @HinhAnh, @Gia, @MaChiNhanh)";

            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-V78FSG02\\CLIENT2;Initial Catalog=QLKS_CN001;User ID=sa;Password=123456;TrustServerCertificate=True;"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaPhong", maPhong);
                    command.Parameters.AddWithValue("@SoPhong", roomNumber);
                    command.Parameters.AddWithValue("@LoaiPhong", type);
                    command.Parameters.AddWithValue("@LoaiGiuong", bed);
                    command.Parameters.AddWithValue("@HinhAnh", anh);
                    command.Parameters.AddWithValue("@Gia", price);
                    command.Parameters.AddWithValue("@MaChiNhanh", maChiNhanh);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Đã thêm phòng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            UC_ThemPhong_Load(this, null);
            clearAll();
        }


        public void clearAll()
        {
            txtMaPhong.Clear();
            txtRoomNumber.Clear();
            CbxRoomType.SelectedIndex = -1;
            CbxBed.SelectedIndex = -1;
            txtPrice.Clear();

        }

        private void UC_ThemPhong_Leave(object sender, EventArgs e)
        {
            clearAll();
        }

        public void ReloadData()
        {
            query = "SELECT MAPHONG,SOPHONG,LOAIPHONG,LOAIGIUONG,HINHANH,GIA,MACHINHANH,TRANGTHAI FROM PHONG;";
            DataSet ds = fn.GetData(query);
            dtGridViewThemPhong.DataSource = ds.Tables[0];
        }


        private void LoadChiNhanh()
        {
            string query = "SELECT MACHINHANH FROM CHINHANH";
            DataSet ds = fn.GetData(query);

            txtChiNhanh.Items.Clear();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    txtChiNhanh.Items.Add(row["MACHINHANH"].ToString());
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhong.Text))
            {
                MessageBox.Show("Vui lòng chọn phòng cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maPhong = txtMaPhong.Text;

            // Câu lệnh SQL để xóa phòng
            string query = "DELETE FROM PHONG WHERE MAPHONG = @MaPhong";

            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-V78FSG02\\CLIENT2;Initial Catalog=QLKS_CN001;User ID=sa;Password=123456;TrustServerCertificate=True;"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaPhong", maPhong);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đã xóa phòng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Tải lại danh sách phòng
                            UC_ThemPhong_Load(this, null);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy phòng để xóa", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void btnThemanh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn ảnh";
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp; *.gif)|*.jpg; *.jpeg; *.png; *.bmp; *.gif";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog.FileName;
            }
        }

        private byte[] ImageToByteArray(PictureBox pictureBox)
        {
            MemoryStream memoryStream = new MemoryStream();
            pictureBox.Image.Save(memoryStream, pictureBox.Image.RawFormat);
            return memoryStream.ToArray();
        }

        private void dtGridViewThemPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dtGridViewThemPhong.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dtGridViewThemPhong.SelectedRows[0];

                // Gán giá trị từ DataGridView vào các trường
                txtMaPhong.Text = row.Cells[0].Value.ToString(); // Mã phòng
                txtRoomNumber.Text = row.Cells[1].Value.ToString(); // Số phòng

                // Đặt giá trị cho ComboBox loại phòng và giường
                CbxRoomType.Text = row.Cells[2].Value.ToString(); // Loại phòng
                CbxBed.Text = row.Cells[3].Value.ToString(); // Loại giường

                // Kiểm tra xem có hình ảnh trong cột ảnh hay không
                if (dtGridViewThemPhong.SelectedRows[0].Cells[4].Value.ToString() != "")
                {
                    MemoryStream memoryStream = new MemoryStream((byte[])dtGridViewThemPhong.SelectedRows[0].Cells[4].Value);
                    pictureBox1.Image = Image.FromStream(memoryStream);
                }
                else
                {
                    pictureBox1.Image = null;
                }

                // Gán giá trị cho giá
                txtPrice.Text = row.Cells[5].Value.ToString();

                txtChiNhanh.Text = row.Cells[6].Value.ToString();
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            string maPhong = txtMaPhong.Text;
            string roomNumber = txtRoomNumber.Text;
            string type = CbxRoomType.Text;
            string bed = CbxBed.Text;

            // Kiểm tra và chuyển đổi giá
            if (!Int64.TryParse(txtPrice.Text, out Int64 price))
            {
                MessageBox.Show("Giá phòng không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maChiNhanh = txtChiNhanh.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(maChiNhanh))
            {
                MessageBox.Show("Vui lòng chọn chi nhánh!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chuyển đổi hình ảnh
            byte[] anh = ImageToByteArray(pictureBox1);

            // Câu lệnh SQL UPDATE với điều kiện WHERE
            query = "UPDATE PHONG SET SOPHONG = @SoPhong, LOAIPHONG = @LoaiPhong, LOAIGIUONG = @LoaiGiuong, HINHANH = @HinhAnh, GIA = @Gia, MACHINHANH = @MaChiNhanh WHERE MAPHONG = @MaPhong";

            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-V78FSG02\\CLIENT2;Initial Catalog=QLKS_CN001;User ID=sa;Password=123456;TrustServerCertificate=True;"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaPhong", maPhong);
                    command.Parameters.AddWithValue("@SoPhong", roomNumber);
                    command.Parameters.AddWithValue("@LoaiPhong", type);
                    command.Parameters.AddWithValue("@LoaiGiuong", bed);
                    command.Parameters.AddWithValue("@HinhAnh", anh);
                    command.Parameters.AddWithValue("@Gia", price);
                    command.Parameters.AddWithValue("@MaChiNhanh", maChiNhanh);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đã cập nhật phòng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy phòng để cập nhật", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            UC_ThemPhong_Load(this, null);
        }

        private void btnXemPhong_Click(object sender, EventArgs e)
        {
            frmXemPhong xp = new frmXemPhong();
            xp.ShowDialog();
        }

        private void btnBaoTri_Click(object sender, EventArgs e)
        {
            query = "UPDATE PHONG SET TRANGTHAI = N'ĐANG BẢO TRÌ' WHERE MAPHONG = '"+txtMaPhong.Text+"'";
            fn.SetData(query, "Bảo trì thành công");
        }

        private void dtGridViewThemPhong_Click(object sender, EventArgs e)
        {
            query = "SELECT MAPHONG ,SOPHONG,LOAIPHONG,LOAIGIUONG,HINHANH, GIA,MACHINHANH,TRANGTHAI FROM PHONG WHERE SOPHONG LIKE '"+txtSearchMaPhong.Text+"%' ";
            DataSet ds = fn.GetData(query);
            dtGridViewThemPhong.DataSource = ds.Tables[0];
        }
    }
}
