using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace AppQuanLyKhachSan
{
    public partial class frmXemPhong : Form
    {
        functions fn = new functions();
        String query;
        private Timer updateTimer;
        private DateTime lastCheckedTime = DateTime.Now; 

        private string connectionString = "Data Source=LAPTOP-V78FSG02\\CLIENT2;Initial Catalog=QLKS_CN001;User ID=sa;Password=123456;TrustServerCertificate=True;";
        public frmXemPhong()
        {
            InitializeComponent();
        }

        private void frmXemPhong_Load(object sender, EventArgs e)
        {

            updateTimer = new Timer();
            updateTimer.Interval = 60000; // Kiểm tra mỗi phút
            updateTimer.Tick += (s, ev) => CheckForUpdates();
            updateTimer.Start();
            rbtnTrucTiep.Visible = false;
            rbtnOnline.Visible = false;

            pictureBoxNotification.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxNotification.Visible = true;

            CheckForUpdates();
            lastCheckedTime = DateTime.Now;
        }

        private void cbxSearchPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxSearchPhong.SelectedIndex == 0) // Phòng trống
            {
                LoadRoomData("N'TRỐNG'");
            }
            else if (cbxSearchPhong.SelectedIndex == 1) // Đang bảo trì
            {
                LoadRoomData("N'ĐANG BẢO TRÌ'");
            }
            else if (cbxSearchPhong.SelectedIndex == 2) // Đã đặt
            {
                LoadRoomData("N'ĐÃ ĐẶT'");
                rbtnTrucTiep.Visible = true;
                rbtnOnline.Visible = true;
            }
            else if (cbxSearchPhong.SelectedIndex == 3) // Đang sử dụng
            {
                LoadRoomData("N'ĐANG SỬ DỤNG'");
            }
            else
            {
                dtgvXemPhong.DataSource = null;
                rbtnTrucTiep.Visible = false;
                rbtnOnline.Visible = false;
            }
        }

        private void rbtn_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxSearchPhong.SelectedIndex == 2) // Chỉ xử lý nếu chọn "ĐÃ ĐẶT"
            {
                string condition = rbtnTrucTiep.Checked ? "KHACHHANGONLINE = 0" : rbtnOnline.Checked ? "KHACHHANGONLINE = 1" : string.Empty;
                if (!string.IsNullOrEmpty(condition))
                {
                    query = $"SELECT MAPHONG, SOPHONG, LOAIPHONG, LOAIGIUONG, GIA, TRANGTHAI, GHICHU FROM PHONG WHERE {condition}";
                    LoadDataGridView(query);
                }
            }
        }

        private void LoadRoomData(string status)
        {
            query = $"SELECT MAPHONG, SOPHONG, LOAIPHONG, LOAIGIUONG, GIA, TRANGTHAI FROM PHONG WHERE TRANGTHAI = {status}";
            LoadDataGridView(query);
        }

        private void LoadDataGridView(string query)
        {
            DataSet ds = fn.GetData(query);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtgvXemPhong.DataSource = null;
                return;
            }
            dtgvXemPhong.DataSource = ds.Tables[0];
        }

        private void CheckForUpdates()
        {
            string query = "SELECT COUNT(*) FROM PHONG WHERE KHACHHANGONLINE = 1 AND TRANGTHAI = N'ĐÃ ĐẶT' AND THOIGIANUPDATE >= @LastCheckedTime";

            try
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("Chuỗi kết nối không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LastCheckedTime", lastCheckedTime.ToString("yyyy-MM-dd HH:mm:ss"));

                        int updateCount = (int)command.ExecuteScalar();

                        ToolTip toolTip = new ToolTip();
                        if (updateCount > 0)
                        {
                            pictureBoxNotification.Image = Image.FromFile(@"D:\QuanLyKhachSan\AppQuanLyKhachSan\Image\icon\notification.png");
                            toolTip.SetToolTip(pictureBoxNotification, $"Có {updateCount} thông báo mới!");
                        }
                        else
                        {
                            pictureBoxNotification.Image = Image.FromFile(@"D:\QuanLyKhachSan\AppQuanLyKhachSan\Image\icon\bell.png");
                            toolTip.SetToolTip(pictureBoxNotification, "Không có thông báo mới!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void pictureBoxNotification_Click(object sender, EventArgs e)
        {
            string query = "SELECT SOPHONG, LOAIPHONG, LOAIGIUONG, THOIGIANUPDATE, GHICHU " +
                           "FROM PHONG " +
                           "WHERE KHACHHANGONLINE = 1 AND TRANGTHAI = N'ĐÃ ĐẶT' AND THOIGIANUPDATE >= @LastCheckedTime";

            try
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("Chuỗi kết nối không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số cho thời gian kiểm tra
                        command.Parameters.AddWithValue("@LastCheckedTime", lastCheckedTime.ToString("yyyy-MM-dd HH:mm:ss"));

                        SqlDataReader reader = command.ExecuteReader();

                        // In số lượng bản ghi trong reader
                        Console.WriteLine("Has rows: " + reader.HasRows);
                        if (reader.HasRows)
                        {
                            string notifications = "Danh sách thông báo mới:\n\n";
                            while (reader.Read())
                            {
                                string sophong = reader["SOPHONG"].ToString();
                                string loaiphong = reader["LOAIPHONG"].ToString();
                                string loaigiuong = reader["LOAIGIUONG"].ToString();
                                string thoigianupdate = reader["THOIGIANUPDATE"].ToString();
                                string ghichu = reader["GHICHU"].ToString();

                                notifications += $"Phòng: {sophong}\n" +
                                                 $"Loại phòng: {loaiphong}\n" +
                                                 $"Loại giường: {loaigiuong}\n" +
                                                 $"Thời gian đặt: {thoigianupdate}\n" +
                                                 $"Ghi chú: {ghichu}\n\n";
                            }

                            MessageBox.Show(notifications, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            lastCheckedTime = DateTime.Now; 
                            Console.WriteLine("Updated lastCheckedTime: " + lastCheckedTime); 
                        }
                        else
                        {
                            MessageBox.Show("Không có thông báo mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);  // In lỗi ra console
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (updateTimer != null)
            {
                updateTimer.Stop();
                updateTimer.Dispose();
            }
            base.OnFormClosing(e);
        }
    }
}
