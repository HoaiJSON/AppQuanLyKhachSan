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
    public partial class UC_Service : UserControl
    {
        functions fn = new functions();
        String query;
        public UC_Service()
        {
            InitializeComponent();
        }
        private void UC_Service_Load(object sender, EventArgs e)
        {


        }

        
        private void btn_AddService_Click(object sender, EventArgs e)
        {
            if(txtNameService.Text != "" && txtPriceService.Text != "" && txtQuantity.Text != "")
            {
                String madichvu = txtMaDichVu.Text;
                String nameService = txtNameService.Text;
                Int64 priceService = Int64.Parse(txtPriceService.Text);
                Int64 quantity = Int64.Parse(txtQuantity.Text);
                String donvitinh = txtDonViTinh.Text;

                // Chuyển đổi hình ảnh từ PictureBox thành mảng byte
                byte[] image = ImageToByteArray(pictureBox1);

                query = @"INSERT INTO DICHVU (MADICHVU, TENDICHVU, HINHANH, GIA, SOLUONG, DONVITINH,MACHINHANH) " +
                        "VALUES (@Madichvu, @NameService, @Image, @PriceService, @Quantity, @DonViTinh, 'CN001')";

                using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-V78FSG02\\CLIENT2;Initial Catalog=QLKS_CN001;User ID=sa;Password=123456;TrustServerCertificate=True;"))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Madichvu", madichvu);
                        command.Parameters.AddWithValue("@NameService", nameService);
                        command.Parameters.AddWithValue("@Image", image);
                        command.Parameters.AddWithValue("@PriceService", priceService);
                        command.Parameters.AddWithValue("@Quantity", quantity);
                        command.Parameters.AddWithValue("@DonViTinh", donvitinh);
                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            MessageBox.Show("Đã thêm dịch vụ thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            UC_Service_Load(this, null);
                            clearAll();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin dịch vụ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        public void clearAll()
        {
            txtNameService.Clear();
            txtPriceService.Clear();
            txtQuantity.Clear();
        }
        private void btnThemanh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn ảnh";
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp; *.gif)|*.jpg; *.jpeg; *.png; *.bmp; *.gif";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
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

        private void dtGridViewService_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (cbxLuaChon.Text == "Dich vu Khac")
            {
                txtMaDichVu.Text = dtGridViewService.SelectedRows[0].Cells[0].Value.ToString();
                txtNameService.Text = dtGridViewService.SelectedRows[0].Cells[1].Value.ToString();
                if(dtGridViewService.SelectedRows[0].Cells[2].Value.ToString() != "")
                {
                    MemoryStream memoryStream = new MemoryStream((byte[])dtGridViewService.SelectedRows[0].Cells[2].Value);
                    pictureBox1.Image = Image.FromStream(memoryStream);
                }else
                {
                    pictureBox1.Image = null;
                }
                txtPriceService.Text = dtGridViewService.SelectedRows[0].Cells[3].Value.ToString();
                txtQuantity.Text = dtGridViewService.SelectedRows[0].Cells[4].Value.ToString();
                txtDonViTinh.Text = dtGridViewService.SelectedRows[0].Cells[5].Value.ToString();

            }
            else
            {
                MessageBox.Show("Khong thay doi duoc vui long vao kho de chinh sua","Thong bao", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            
        }


        //Cập nhật dịch vụ
        private void btn_ChangeService_Click(object sender, EventArgs e)
        {
            string tendv = txtNameService.Text;
            Int64 gia = Int64.Parse(txtPriceService.Text);
            Int64 soluong = Int64.Parse(txtQuantity.Text);
            string madichvu = dtGridViewService.SelectedRows[0].Cells[0].Value.ToString();
            String DonViTinh = txtDonViTinh.Text;

            // Chuyển đổi hình ảnh từ PictureBox thành mảng byte
            byte[] image = ImageToByteArray(pictureBox1);

            query = "UPDATE DICHVU SET TENDICHVU = @NameService, HINHANH = @Image, GIA = @PriceService, SOLUONG = @Quantity, DONVITINH = N'NGHÌN ĐỒNG' WHERE MADICHVU = @ServiceID";

            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-V78FSG02\\CLIENT2;Initial Catalog=QLKS_CN001;User ID=sa;Password=123456;TrustServerCertificate=True;"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NameService", tendv);
                    command.Parameters.AddWithValue("@Image", image);
                    command.Parameters.AddWithValue("@PriceService", gia);
                    command.Parameters.AddWithValue("@Quantity", soluong);
                    command.Parameters.AddWithValue("@ServiceID", madichvu);
                    command.Parameters.AddWithValue("DonViTinh", DonViTinh);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Đã cập nhật dịch vụ thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        UC_Service_Load(this, null);
                        clearAll();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_DeleteService_Click(object sender, EventArgs e)
        {
            if (dtGridViewService.SelectedRows.Count > 0)
            {
                // Lấy mã dịch vụ từ dòng được chọn trong DataGridView
                string madichvu = dtGridViewService.SelectedRows[0].Cells[0].Value.ToString();

                // Câu lệnh xóa dịch vụ
                string query = "DELETE FROM DICHVU WHERE MADICHVU = @ServiceID";

                using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-V78FSG02\\CLIENT2;Initial Catalog=QLKS_CN001;User ID=sa;Password=123456;TrustServerCertificate=True;"))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số mã dịch vụ
                        command.Parameters.AddWithValue("@ServiceID", madichvu);

                        try
                        {
                            connection.Open();
                            int result = command.ExecuteNonQuery();

                            if (result > 0)
                            {
                                MessageBox.Show("Đã xóa dịch vụ thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Tải lại danh sách dịch vụ và xóa các trường nhập liệu
                                UC_Service_Load(this, null);
                                clearAll();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy dịch vụ để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnKho_Click(object sender, EventArgs e)
        {
            frmKho frmKho = new frmKho();
            frmKho.ShowDialog();
        }


        public void setComboBox(string query, ComboBox combo)
        {
            combo.Items.Clear(); // Xóa các mục hiện tại trong ComboBox
            using (SqlDataReader sdr = fn.GetForCombo(query))
            {
                while (sdr.Read())
                {
                    // Thêm các giá trị vào ComboBox
                    combo.Items.Add(sdr.GetString(0)); // Lấy giá trị từ cột đầu tiên
                }
            } // SqlDataReader sẽ tự động đóng ở đây
        }

        private void cbxLuaChon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLuaChon.Text == "Dich vu Kho")
            {
                query = "select MAKHO AS MASANPHAM, TENHANG, SOLUONG,DONVITINH ,GIADONVI, DANHMUC from KHO";
                DataSet ds = fn.GetData(query);
                dtGridViewService.DataSource = ds.Tables[0];

            } else if(cbxLuaChon.Text =="Dich vu Khac")
            {
                query = "select MADICHVU, TENDICHVU,HINHANH,SOLUONG,DONVITINH,GIA from DICHVU WHERE MAKHO IS NULL";
                DataSet ds = fn.GetData(query);
                dtGridViewService.DataSource= ds.Tables[0];
                DataGridViewImageColumn pic = new DataGridViewImageColumn();
                pic = (DataGridViewImageColumn)dtGridViewService.Columns[2];
                pic.ImageLayout = DataGridViewImageCellLayout.Zoom;
            }
        }
    }
}
