using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace AppQuanLyKhachSan.CTQuanLyKhachSan
{
    public partial class UC_Customer : UserControl
    {
        functions fn = new functions();
        String query;

        public UC_Customer()
        {
            InitializeComponent();
        }
        public void setComboBox(string query, ComboBox combo)
        {
            combo.Items.Clear();
            using (SqlDataReader sdr = fn.GetForCombo(query))
            {
                while (sdr.Read())
                {
                    combo.Items.Add(sdr.GetString(0));
                }
            } 
        }


        private void cbxBed_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxRoomType.SelectedIndex = -1;
            cbxNumberR.Items.Clear();
            txtPrice.Clear();
        }

        private void cbxRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxNumberR.Items.Clear();
            query = "SELECT SOPHONG FROM PHONG WHERE LOAIGIUONG = N'" + cbxBed.Text + "' AND LOAIPHONG = N'" + cbxRoomType.Text + "' AND TRANGTHAI NOT IN (N'ĐANG SỬ DỤNG', N'ĐANG BẢO TRÌ')";
            setComboBox(query, cbxNumberR);
        }

        String rid;
        String maCN;
        private void cbxNumberR_SelectedIndexChanged(object sender, EventArgs e)
        {
            query = "select GIA, MAPHONG, MACHINHANH from PHONG where SOPHONG = '" + cbxNumberR.Text + "'";
            DataSet ds = fn.GetData(query);
            txtPrice.Text = ds.Tables[0].Rows[0][0].ToString();
            maCN = ds.Tables[0].Rows[0][2].ToString();
            rid = ds.Tables[0].Rows[0][1].ToString();
        }

        private void btnResignCus_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtContact.Text != "" && txtNationnality.Text != "" && dtPCheckin.Text != "" && dtPDate.Text != "" && cbxGender.Text != "" && txtID.Text != "" && txtAddress.Text != "" && cbxBed.Text != "" && cbxRoomType.Text != "" && cbxNumberR.Text != "" && txtPrice.Text != "")
            {
                String name = txtName.Text; // Tên
                Int64 mobile = Int64.Parse(txtContact.Text); // Số điện thoại
                String national = txtNationnality.Text; // Quốc tịch
                String gender = cbxGender.Text; // Giới tính
                String dtP = dtPDate.Text; // Năm sinh
                String CMND = txtID.Text; // CCCD
                String address = txtAddress.Text; // Địa chỉ
                String checkin = dtPCheckin.Value.ToString("yyyy-MM-dd");
                String checkout = dtPCheckOut.Value.ToString("yyyy-MM-dd");
                String dateResign = dtpDateDatPhong.Value.ToString("yyyy-MM-dd");
                // Kiểm tra xem khách hàng đã tồn tại
                query = "SELECT COUNT(*) FROM KHACHHANG WHERE CCCD = '" + CMND + "'";
                int customerCount = (int)fn.GetScalarValue(query); // Lấy giá trị số lượng khách hàng

                if (customerCount > 0)
                {
                    // Cập nhật thông tin khi khách hàng đã tồn tại
                    query = @"  DECLARE @MAPHONG VARCHAR(100);
                    DECLARE @MADATPHONG VARCHAR(100) = 'DP' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR);

                    UPDATE PHONG
                    SET TRANGTHAI = N'ĐÃ ĐẶT'
                    WHERE SOPHONG = '" + cbxNumberR.Text + @"';

                    SELECT @MAPHONG = MAPHONG 
                    FROM PHONG
                    WHERE SOPHONG = '" + cbxNumberR.Text + @"';

                    INSERT INTO DATPHONG(MADATPHONG, MAKHACHHANG, MAPHONG, NGAYDANGKY,NGAYCHECKIN, NGAYCHECKOUT, TRANGTHAI)
                    VALUES (@MADATPHONG, (SELECT MAKHACHHANG FROM KHACHHANG WHERE CCCD = '" + CMND + "'), @MAPHONG, '"+ dateResign + "','" + checkin + "', '" + checkout + "', N'CHƯA THANH TOÁN');";
                     fn.SetData(query, "Khách hàng đã tồn tại. Thông tin đặt phòng đã được cập nhật.");
                }
                else
                {
                    // Thêm mới khách hàng
                    query = @"
                    DECLARE @NewMAKHACHHANG VARCHAR(100);
                    SET @NewMAKHACHHANG = 'KH' + CAST(ISNULL((SELECT MAX(CAST(SUBSTRING(MAKHACHHANG, 3, LEN(MAKHACHHANG)) AS INT)) FROM KHACHHANG), 0) + 1 AS VARCHAR);
                    DECLARE @MAPHONG VARCHAR(100);
                    DECLARE @MADATPHONG VARCHAR(100) = 'DP' + CAST(ABS(CHECKSUM(NEWID())) AS VARCHAR);

                    INSERT INTO KHACHHANG (MAKHACHHANG, TENKHACHHANG, DIENTHOAI, QUOCTICH, GIOITINH, NGAYSINH, CCCD, DIACHI, MACHINHANH)
                    VALUES (@NewMAKHACHHANG, N'" + name + "', '" + mobile + "', N'" + national + "', N'" + gender + "', '" + dtP + "', '" + CMND + "', N'" + address + "', '" + maCN + @"');

                    UPDATE PHONG
                    SET TRANGTHAI = N'ĐÃ ĐẶT' , KHACHHANGONLINE = 0
                    WHERE SOPHONG = '" + cbxNumberR.Text + @"';

                    SELECT @MAPHONG = MAPHONG
                    FROM PHONG
                    WHERE SOPHONG = '" + cbxNumberR.Text + @"';

                    INSERT INTO DATPHONG(MADATPHONG, MAKHACHHANG, MAPHONG, NGAYDANGKY,NGAYCHECKIN, NGAYCHECKOUT, TRANGTHAI)
                    VALUES (@MADATPHONG, @NewMAKHACHHANG, @MAPHONG, '"+ dateResign +"','" + checkin + "', '" + checkout + "', N'CHƯA THANH TOÁN');";
                    fn.SetData(query, "Đăng ký khách hàng mới thành công với số phòng " + cbxNumberR.Text);
                }

                clearAll();
            }
            else
            {
                MessageBox.Show("Xin vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ReloadData();
        }

        public void clearAll()
        {
            txtName.Clear();
            txtContact.Clear();
            cbxGender.SelectedIndex = -1;
            dtPDate.ResetText();
            txtID.Clear();
            txtAddress.Clear();
            dtPCheckin.ResetText();
            cbxBed.SelectedIndex = -1;
            cbxRoomType.SelectedIndex = -1;
            cbxNumberR.Items.Clear();
            txtPrice.Clear();
        }
        private void UC_CustomerRes_Leave(object sender, EventArgs e)
        {
            
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        //Tab chi tiết khách hàng
        private void cbxFindCustomerDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxFindCustomerDetail.SelectedIndex == 0)
            {
                query = "select KHACHHANG.MAKHACHHANG, KHACHHANG.TENKHACHHANG,KHACHHANG.DIENTHOAI, KHACHHANG.QUOCTICH, KHACHHANG.GIOITINH, KHACHHANG.NGAYSINH, KHACHHANG.CCCD, KHACHHANG.DIACHI, PHONG.SOPHONG,PHONG.MAPHONG, PHONG.LOAIPHONG, PHONG.LOAIGIUONG, PHONG.GIA,DATPHONG.NGAYCHECKIN,DATPHONG.NGAYCHECKOUT FROM KHACHHANG JOIN DATPHONG ON KHACHHANG.MAKHACHHANG = DATPHONG.MAKHACHHANG JOIN PHONG ON DATPHONG.MAPHONG = PHONG.MAPHONG ";
                getRecord(query);
            }
            else if (cbxFindCustomerDetail.SelectedIndex == 1)
            {
                query = "select KHACHHANG.MAKHACHHANG, KHACHHANG.TENKHACHHANG,KHACHHANG.DIENTHOAI, KHACHHANG.QUOCTICH, KHACHHANG.GIOITINH, KHACHHANG.NGAYSINH, KHACHHANG.CCCD, KHACHHANG.DIACHI, PHONG.SOPHONG,PHONG.MAPHONG, PHONG.LOAIPHONG, PHONG.LOAIGIUONG, PHONG.GIA,DATPHONG.NGAYCHECKIN,DATPHONG.NGAYCHECKOUT FROM KHACHHANG JOIN DATPHONG ON KHACHHANG.MAKHACHHANG = DATPHONG.MAKHACHHANG JOIN PHONG ON DATPHONG.MAPHONG = PHONG.MAPHONG WHERE DATPHONG.TRANGTHAI = N'CHƯA THANH TOÁN'";
                getRecord(query);
            }
            else if (cbxFindCustomerDetail.SelectedIndex == 2)
            {
                query = "select KHACHHANG.MAKHACHHANG, KHACHHANG.TENKHACHHANG,KHACHHANG.DIENTHOAI, KHACHHANG.QUOCTICH, KHACHHANG.GIOITINH, KHACHHANG.NGAYSINH, KHACHHANG.CCCD, KHACHHANG.DIACHI, PHONG.SOPHONG,PHONG.MAPHONG, PHONG.LOAIPHONG, PHONG.LOAIGIUONG, PHONG.GIA,DATPHONG.NGAYCHECKIN,DATPHONG.NGAYCHECKOUT FROM KHACHHANG JOIN DATPHONG ON KHACHHANG.MAKHACHHANG = DATPHONG.MAKHACHHANG JOIN PHONG ON DATPHONG.MAPHONG = PHONG.MAPHONG WHERE DATPHONG.TRANGTHAI = N'ĐÃ THANH TOÁN' ";
                getRecord(query);
            }

        }
        private void getRecord(String query)
        {
            DataSet ds = fn.GetData(query);
            dtgViewCustomerDetail.DataSource = ds.Tables[0];
            dtgvCheckIN.DataSource = ds.Tables[0];
        }

        public void ReloadData()
        {
             query = "SELECT KHACHHANG.MAKHACHHANG, KHACHHANG.TENKHACHHANG, KHACHHANG.DIENTHOAI, KHACHHANG.QUOCTICH, KHACHHANG.GIOITINH, " +
                "KHACHHANG.NGAYSINH, KHACHHANG.CCCD, KHACHHANG.DIACHI, PHONG.SOPHONG, PHONG.LOAIPHONG, PHONG.LOAIGIUONG, PHONG.GIA, " +
                "DATPHONG.NGAYCHECKIN, DATPHONG.NGAYCHECKOUT,PHONG.MAPHONG FROM KHACHHANG " +
                "JOIN DATPHONG ON KHACHHANG.MAKHACHHANG = DATPHONG.MAKHACHHANG " +
                "JOIN PHONG ON DATPHONG.MAPHONG = PHONG.MAPHONG";
            getRecord(query);
        }

        //Tab Dịch vụ
        private void LoadPhong()
        {
            string query = "SELECT MAPHONG FROM PHONG";
            DataSet ds = fn.GetData(query);

            cbxSearchMaPhong.Items.Clear();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    cbxSearchMaPhong.Items.Add(row["MAPHONG"].ToString());
                }
            }
        }

        private void LoadService()
        {
            String query = "select TENDICHVU FROM DICHVU";
            DataSet ds = fn.GetData(query);

            cbxServiceNameCus.Items.Clear();

            if(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    cbxServiceNameCus.Items.Add(row["TENDICHVU"].ToString());
                }
            }
        }

        private void UC_Customer_Load(object sender, EventArgs e)
        {
            dtPCheckin.Value = DateTime.Now;
            dtPCheckOut.Value = DateTime.Now;
            LoadPhong();
            LoadService();
            LoadMaNVserviceCus();
        }

        private void cbxSearchMaPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            String maphong = cbxSearchMaPhong.SelectedItem.ToString();
            query = "SELECT KHACHHANG.TENKHACHHANG, KHACHHANG.MAKHACHHANG " +
                    "FROM KHACHHANG " +
                    "JOIN DATPHONG ON KHACHHANG.MAKHACHHANG = DATPHONG.MAKHACHHANG " +
                    "JOIN PHONG ON DATPHONG.MAPHONG = PHONG.MAPHONG  WHERE DATPHONG.TRANGTHAI = 'CHƯA THANH TOÁN' AND DATPHONG.MAPHONG = '" + maphong + "' AND PHONG.TRANGTHAI = N'ĐANG SỬ DỤNG'";
            DataSet ds = fn.GetData(query);
            
            if (ds.Tables[0].Rows.Count == 0) //nếu như table bằng rỗng 
            {
                txtNameCusService.Clear();
                txtServiceMaKH.Clear();
            }
            else
            {
                txtNameCusService.Text = ds.Tables[0].Rows[0][0].ToString();
                txtServiceMaKH.Text = ds.Tables[0].Rows[0][1].ToString();
                
            }
            query = @"DECLARE @NGAYCHECKIN DATE;
          SELECT @NGAYCHECKIN = NGAYCHECKIN 
          FROM DATPHONG 
          WHERE MAKHACHHANG = '" + txtServiceMaKH.Text + @"' AND TRANGTHAI = N'CHƯA THANH TOÁN';
          SELECT DICHVU.TENDICHVU, DICHVU.GIA, CHITIETDICHVU.SOLUONG, CHITIETDICHVU.NGAYSUDUNG 
          FROM DICHVU
          JOIN CHITIETDICHVU ON DICHVU.MADICHVU = CHITIETDICHVU.MADICHVU
          JOIN KHACHHANG ON KHACHHANG.MAKHACHHANG = CHITIETDICHVU.MAKHACHHANG
          JOIN DATPHONG ON KHACHHANG.MAKHACHHANG = DATPHONG.MAKHACHHANG
          WHERE DATPHONG.MAPHONG = '" + maphong + @"' 
          AND DATPHONG.TRANGTHAI = N'CHƯA THANH TOÁN'
          AND DATPHONG.NGAYCHECKOUT >= @NGAYCHECKIN
          AND CHITIETDICHVU.MADATPHONG = DATPHONG.MADATPHONG;";

            DataSet dm = fn.GetData(query);

            if (dm.Tables[0].Rows.Count == 0)
            {
                dtgvCustomeDV.DataSource = null;
            }
            else
            {
                dtgvCustomeDV.DataSource = dm.Tables[0];
            }
        }

        private void cbxServiceNameCus_SelectedIndexChanged(object sender, EventArgs e)
        {
            String tendv = cbxServiceNameCus.SelectedItem.ToString();
            query = "SELECT MADICHVU,GIA FROM DICHVU WHERE TENDICHVU = N'" + tendv + "'" ;
            DataSet ds = fn.GetData(query);
            txtMaDV.Text = ds.Tables[0].Rows[0][0].ToString();
            txtServicePriceCus.Text = ds.Tables[0].Rows[0][1].ToString();

        }
        private void btnServiceCus_Click(object sender, EventArgs e)
        {
            String makho;
            String makh = txtServiceMaKH.Text; // Mã khách hàng
            String DVMA = txtMaDV.Text; // Mã dịch vụ
            String manv = cbxMaNVSvKH.Text; // Mã nhân viên
            String tendv = cbxServiceNameCus.Text;
            Int64 quantity;
            String maPhong = cbxSearchMaPhong.Text;
            String madv = txtMaDV.Text;

            if (txtServiceMaKH.Text != "" && txtMaDV.Text != "" && cbxMaNVSvKH.Text != "" && txtServiceQuantity.Text != "")
            {
                // Lấy MAKHO từ DICHVU
                query = @"SELECT K.MAKHO 
              FROM KHO K
              JOIN DICHVU DV ON K.MAKHO = DV.MAKHO 
              WHERE DV.MADICHVU = '" + madv + "';";
                DataSet ds = fn.GetData(query);

                makho = ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0]["MAKHO"].ToString() : null;

                // Kiểm tra tính hợp lệ của số lượng
                if (!Int64.TryParse(txtServiceQuantity.Text, out quantity) || quantity <= 0)
                {
                    MessageBox.Show("Số lượng không hợp lệ!");
                    return;
                }

                // Kiểm tra mã đặt phòng cho mã phòng cụ thể
                query = @"SELECT DATPHONG.MADATPHONG
              FROM DATPHONG
              WHERE DATPHONG.MAPHONG = '" + maPhong + @"' 
              AND DATPHONG.TRANGTHAI = N'CHƯA THANH TOÁN';";
                DataSet dsDatPhong = fn.GetData(query);

                if (dsDatPhong.Tables[0].Rows.Count > 0)
                {
                    String madatphong = dsDatPhong.Tables[0].Rows[0]["MADATPHONG"].ToString();

                    // Kiểm tra xem dịch vụ đã tồn tại chưa
                    query = @"SELECT COUNT(*) 
                  FROM CHITIETDICHVU 
                  WHERE MAKHACHHANG = '" + makh + "' AND MADICHVU = '" + DVMA + "' AND MADATPHONG = '" + madatphong + "';";
                    DataSet checkExist = fn.GetData(query);
                    Int64 recordCount = Int64.Parse(checkExist.Tables[0].Rows[0][0].ToString());

                    if (recordCount > 0)
                    {
                        // Nếu đã tồn tại, thực hiện UPDATE
                        query = @"UPDATE CHITIETDICHVU 
                      SET SOLUONG = SOLUONG + " + quantity + @", NGAYSUDUNG = GETDATE() 
                      WHERE MAKHACHHANG = '" + makh + "' AND MADICHVU = '" + DVMA + "' AND MADATPHONG = '" + madatphong + "';";
                        fn.SetData(query, "Dịch vụ đã được cập nhật thành công cho phòng " + maPhong);
                    }
                    else
                    {
                        // Nếu chưa có, thêm mới
                        query = @"INSERT INTO CHITIETDICHVU 
                      (MAKHACHHANG, MADICHVU, MANHANVIEN, SOLUONG, NGAYSUDUNG, MADATPHONG) 
                      VALUES ('" + makh + "', '" + DVMA + "', '" + manv + "', " + quantity + ", GETDATE(), '" + madatphong + "');";
                        fn.SetData(query, "Dịch vụ đã được thêm thành công cho phòng " + maPhong);
                    }

                    // Nếu makho không phải null, cập nhật số lượng trong kho
                    if (!string.IsNullOrEmpty(makho))
                    {
                        // Kiểm tra số lượng trong kho
                        query = @"SELECT SOLUONG 
                      FROM KHO 
                      WHERE MAKHO = '" + makho + "';";
                        DataSet checkKho = fn.GetData(query);

                        Int64 mh = checkKho.Tables[0].Rows.Count > 0 ? Int64.Parse(checkKho.Tables[0].Rows[0][0].ToString()) : 0;

                        // Nếu đủ số lượng trong kho, cập nhật
                        if (mh >= quantity)
                        {
                            query = @"UPDATE KHO 
                          SET SOLUONG = SOLUONG - " + quantity + @" 
                          WHERE MAKHO = '" + makho + "';";
                            fn.SetData(query, "Kho đã được cập nhật sau giao dịch!");
                        }
                        else
                        {
                            MessageBox.Show("Số lượng trong kho không đủ, nhưng dịch vụ vẫn được thêm vào danh sách!");
                        }
                    }

                    // **Cập nhật số lượng trong DICHVU**
                    query = @"SELECT SOLUONG 
                  FROM DICHVU 
                  WHERE MADICHVU = '" + DVMA + "';";
                    DataSet checkDichVu = fn.GetData(query);

                    Int64 dvQuantity = checkDichVu.Tables[0].Rows.Count > 0 ? Int64.Parse(checkDichVu.Tables[0].Rows[0][0].ToString()) : 0;

                    if (dvQuantity >= quantity)
                    {
                        query = @"UPDATE DICHVU 
                      SET SOLUONG = SOLUONG - " + quantity + @" 
                      WHERE MADICHVU = '" + DVMA + "';";
                        fn.SetData(query, "Số lượng dịch vụ đã được cập nhật thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Số lượng dịch vụ không đủ, nhưng vẫn được xử lý!");
                    }
                }
                else
                {
                    // Không tìm thấy mã đặt phòng hợp lệ
                    MessageBox.Show("Không tìm thấy mã đặt phòng phù hợp cho phòng này!");
                }
            }else
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin");
            }
          
        }
        private void LoadMaNVserviceCus()
        {
            String query = "select MANHANVIEN FROM NHANVIEN";
            DataSet ds = fn.GetData(query);

            cbxMaNVSvKH.Items.Clear();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    cbxMaNVSvKH.Items.Add(row["MANHANVIEN"].ToString());
                }
            }
        }
        private void cbxMaNVSvKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            String manv = cbxMaNVSvKH.SelectedItem.ToString();
            query = "SELECT TENNHANVIEN FROM NHANVIEN WHERE MANHANVIEN = '" + manv + "'";
            DataSet ds = fn.GetData(query);
            txtNameNVSvCus.Text = ds.Tables[0].Rows[0][0].ToString();
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            // Xóa tất cả các mục hiện tại trong ComboBox
            //cbxServiceNameCus.SelectedIndex = 0;

            //// Gọi lại phương thức để load dữ liệu cho ComboBox
            //LoadMaNVserviceCus();        

            //// Cập nhật các trường dữ liệu khác
            //txtMaDV.Clear();
            //cbxServiceNameCus.SelectedIndex = -1;
            //txtServicePriceCus.Clear();
            //txtServiceQuantity.Clear();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MaKH))
            {
                string maKhachHang = MaKH.Trim();

                // Truy vấn xóa khách hàng và dữ liệu liên quan
                string query = @"
            DELETE FROM THONGKEDOANHTHU 
            WHERE MAHOADON IN (
                SELECT MAHD FROM HOADON WHERE MAKHACHHANG = N'" + maKhachHang + @"'
            );

            DELETE FROM CHITIETHOADON 
            WHERE MAHD IN (
                SELECT MAHD FROM HOADON WHERE MAKHACHHANG = N'" + maKhachHang + @"'
            );

            DELETE FROM HOADON 
            WHERE MAKHACHHANG = N'" + maKhachHang + @"';

            DELETE FROM DATPHONG 
            WHERE MAKHACHHANG = N'" + maKhachHang + @"';

            DELETE FROM CHITIETDICHVU 
            WHERE MAKHACHHANG = N'" + maKhachHang + @"';

            DELETE FROM KHACHHANG 
            WHERE MAKHACHHANG = N'" + maKhachHang + @"'; ";

                try
                {
                    fn.SetData(query, "Khách hàng và dữ liệu liên quan đã được xóa thành công.");
                    clearAll();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập mã khách hàng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MaKH) && !string.IsNullOrEmpty(txtName.Text) &&
                !string.IsNullOrEmpty(txtContact.Text) && !string.IsNullOrEmpty(txtNationnality.Text) &&
                !string.IsNullOrEmpty(cbxGender.Text) && !string.IsNullOrEmpty(txtID.Text) &&
                !string.IsNullOrEmpty(txtAddress.Text))
            {
                // Lấy thông tin từ giao diện
                string maKH = MaKH.Trim(); // Mã khách hàng cần cập nhật
                string name = txtName.Text.Trim(); // Tên khách hàng
                Int64 mobile = Int64.Parse(txtContact.Text.Trim()); // Số điện thoại
                string national = txtNationnality.Text.Trim(); // Quốc tịch
                string gender = cbxGender.Text.Trim(); // Giới tính
                string dob = dtPDate.Text.Trim(); // Ngày sinh
                string cmnd = txtID.Text.Trim(); // CCCD
                string address = txtAddress.Text.Trim(); // Địa chỉ

                // Tạo câu lệnh SQL để cập nhật
                query = @" UPDATE KHACHHANG
                SET TENKHACHHANG = N'" + name + @"',
                DIENTHOAI = '" + mobile + @"',
                QUOCTICH = N'" + national + @"',
                GIOITINH = N'" + gender + @"',
                NGAYSINH = '" + dob + @"',
                CCCD = '" + cmnd + @"',
                DIACHI = N'" + address + @"'
                WHERE MAKHACHHANG = '" + maKH + "';";

                // Thực thi truy vấn
                try
                {
                    fn.SetData(query, "Cập nhật thông tin khách hàng thành công.");
                    clearAll(); // Xóa dữ liệu trên giao diện sau khi thực thi thành công
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        String MaKH;
        private void dtgViewCustomerDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem người dùng có nhấn vào một hàng hợp lệ không
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgViewCustomerDetail.Rows[e.RowIndex];
                MaKH = row.Cells["MAKHACHHANG"].Value.ToString();
                // Gán giá trị từ các ô trong hàng vào các TextBox tương ứng
                txtName.Text = row.Cells["TENKHACHHANG"].Value.ToString(); // Tên khách hàng
                txtContact.Text = row.Cells["DIENTHOAI"].Value.ToString(); // Số điện thoại
                txtNationnality.Text = row.Cells["QUOCTICH"].Value.ToString(); // Quốc tịch
                cbxGender.Text = row.Cells["GIOITINH"].Value.ToString(); // Giới tính
                dtPDate.Text = Convert.ToDateTime(row.Cells["NGAYSINH"].Value).ToString("yyyy-MM-dd"); // Năm sinh
                txtID.Text = row.Cells["CCCD"].Value.ToString(); // CCCD
                txtAddress.Text = row.Cells["DIACHI"].Value.ToString(); // Địa chỉ
                dtPCheckin.Text = Convert.ToDateTime(row.Cells["NGAYCHECKIN"].Value).ToString("yyyy-MM-dd"); // Ngày Check-in
                dtPDate.Text = Convert.ToDateTime(row.Cells["NGAYCHECKOUT"].Value).ToString("yyyy-MM-dd"); // Ngày Check-out
                // Cập nhật các thông tin khác nếu cần thiết
                cbxNumberR.Text = row.Cells["SOPHONG"].Value.ToString(); // Số phòng
                
            }
        }

        private void btnTaiLaiKH_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        private void btnDeleteSev_Click(object sender, EventArgs e)
        {
            String makh = txtServiceMaKH.Text; // Mã khách hàng
            String DVMA = txtMaDV.Text; // Mã dịch vụ
            String maPhong = cbxSearchMaPhong.Text;

            // Kiểm tra xem mã đặt phòng tồn tại
            query = @"SELECT MADATPHONG 
              FROM DATPHONG 
              WHERE MAPHONG = '" + maPhong + @"' 
              AND TRANGTHAI = N'CHƯA THANH TOÁN';";
            DataSet dsDatPhong = fn.GetData(query);

            if (dsDatPhong.Tables[0].Rows.Count > 0)
            {
                String madatphong = dsDatPhong.Tables[0].Rows[0][0].ToString();

                // Kiểm tra xem bản ghi tồn tại trong CHITIETDICHVU
                query = "SELECT SOLUONG FROM CHITIETDICHVU " +
                        "WHERE MAKHACHHANG = '" + makh + "' AND MADICHVU = '" + DVMA + "' AND MADATPHONG = '" + madatphong + "';";
                DataSet dsCTDV = fn.GetData(query);

                if (dsCTDV.Tables[0].Rows.Count > 0)
                {
                    Int64 quantity = Int64.Parse(dsCTDV.Tables[0].Rows[0][0].ToString());

                    // Xóa bản ghi trong CHITIETDICHVU
                    query = "DELETE FROM CHITIETDICHVU WHERE MAKHACHHANG = '" + makh + "' AND MADICHVU = '" + DVMA + "' AND MADATPHONG = '" + madatphong + "';";

                    // Cập nhật lại số lượng trong kho
                    query += "UPDATE KHO SET SOLUONG = SOLUONG + " + quantity +
                             " WHERE MADICHVU = '" + DVMA + "';";

                    fn.SetData(query, "Xóa dịch vụ thành công!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dịch vụ cần xóa!");
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy mã đặt phòng phù hợp!");
            }
        }

        private void btnUpdateSerV_Click(object sender, EventArgs e)
        {
            String makh = txtServiceMaKH.Text; // Mã khách hàng
            String DVMA = txtMaDV.Text; // Mã dịch vụ
            String maPhong = cbxSearchMaPhong.Text;
            Int64 newQuantity;

            // Kiểm tra tính hợp lệ của số lượng mới
            if (!Int64.TryParse(txtServiceQuantity.Text, out newQuantity) || newQuantity <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!");
                return;
            }

            // Kiểm tra mã đặt phòng tồn tại
            query = @"SELECT MADATPHONG 
              FROM DATPHONG 
              WHERE MAPHONG = '" + maPhong + @"' 
              AND TRANGTHAI = N'CHƯA THANH TOÁN';";
            DataSet dsDatPhong = fn.GetData(query);

            if (dsDatPhong.Tables[0].Rows.Count > 0)
            {
                String madatphong = dsDatPhong.Tables[0].Rows[0][0].ToString();

                // Lấy số lượng hiện tại từ CHITIETDICHVU
                query = "SELECT SOLUONG FROM CHITIETDICHVU " +
                        "WHERE MAKHACHHANG = '" + makh + "' AND MADICHVU = '" + DVMA + "' AND MADATPHONG = '" + madatphong + "';";
                DataSet dsCTDV = fn.GetData(query);

                if (dsCTDV.Tables[0].Rows.Count > 0)
                {
                    Int64 currentQuantity = Int64.Parse(dsCTDV.Tables[0].Rows[0][0].ToString());

                    // Kiểm tra số lượng tồn trong kho
                    query = "SELECT SOLUONG FROM KHO WHERE MADICHVU = '" + DVMA + "';";
                    DataSet dsKho = fn.GetData(query);

                    if (dsKho.Tables[0].Rows.Count > 0)
                    {
                        Int64 stockQuantity = Int64.Parse(dsKho.Tables[0].Rows[0][0].ToString());
                        Int64 diff = newQuantity - currentQuantity; // Chênh lệch số lượng

                        if (stockQuantity + currentQuantity >= newQuantity) // Kiểm tra tồn kho đủ
                        {
                            // Cập nhật số lượng mới trong CHITIETDICHVU
                            query = "UPDATE CHITIETDICHVU SET SOLUONG = " + newQuantity +
                                    " WHERE MAKHACHHANG = '" + makh + "' AND MADICHVU = '" + DVMA + "' AND MADATPHONG = '" + madatphong + "';";

                            // Điều chỉnh số lượng tồn trong kho
                            query += "UPDATE KHO SET SOLUONG = SOLUONG - " + diff +
                                     " WHERE MADICHVU = '" + DVMA + "';";

                            fn.SetData(query, "Cập nhật dịch vụ thành công!");
                        }
                        else
                        {
                            MessageBox.Show("Không đủ số lượng trong kho để cập nhật!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy dịch vụ trong kho!");
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dịch vụ cần cập nhật!");
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy mã đặt phòng phù hợp!");
            }
        }

        private void btnHuyDat_Click(object sender, EventArgs e)

        {
            String cccD = txtID.Text;
            
            query = "DECLARE @MaPhong VARCHAR(100);" +
                @"SELECT @MaPhong = DP.MAPHONG FROM KHACHHANG KH
                  INNER JOIN DATPHONG DP ON KH.MAKHACHHANG = DP.MAKHACHHANG
                  WHERE KH.CCCD = '" + cccD + "';" + 
                @"UPDATE PHONG SET TRANGTHAI = N'TRỐNG' WHERE MAPHONG IN (SELECT MAPHONG FROM DATPHONG WHERE MAPHONG = @MaPhong);" +
                "DECLARE @MADATPHONG VARCHAR(100);" +
                "SELECT @MADATPHONG = DATPHONG.MADATPHONG FROM DATPHONG WHERE DATPHONG.MAPHONG = @MaPhong "+
                @"AND DATPHONG.TRANGTHAI = N'CHƯA THANH TOÁN';" +
                "DELETE FROM DATPHONG WHERE MADATPHONG = @MADATPHONG;";
            fn.SetData(query, "Hủy đặt phòng thành công");
        }

        private void TabServiceKhachHang_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            query = @"DECLARE @NGAYCHECKIN DATE;
          SELECT @NGAYCHECKIN = NGAYCHECKIN 
          FROM DATPHONG 
          WHERE MAKHACHHANG = '" + txtServiceMaKH.Text + @"' 
            AND TRANGTHAI = N'CHƯA THANH TOÁN';

          SELECT DICHVU.TENDICHVU, 
                 DICHVU.GIA, 
                 CHITIETDICHVU.SOLUONG, 
                 CHITIETDICHVU.NGAYSUDUNG 
          FROM DICHVU
          JOIN CHITIETDICHVU ON DICHVU.MADICHVU = CHITIETDICHVU.MADICHVU
          JOIN KHACHHANG ON KHACHHANG.MAKHACHHANG = CHITIETDICHVU.MAKHACHHANG
          JOIN DATPHONG ON KHACHHANG.MAKHACHHANG = DATPHONG.MAKHACHHANG
          WHERE DATPHONG.MAPHONG = '" + cbxSearchMaPhong.Text + @"' 
            AND DATPHONG.TRANGTHAI = N'CHƯA THANH TOÁN'
            AND DATPHONG.NGAYCHECKOUT >= @NGAYCHECKIN
            AND CHITIETDICHVU.MADATPHONG = DATPHONG.MADATPHONG;";
            DataSet ds = fn.GetData(query);

            dtgvCustomeDV.DataSource = ds.Tables[0];

        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            String tenKH = txtTenKHCheckIn.Text;
            DateTime ngayCheckIn = dtpNgayCheckIn.Value;
            String maPhong = txtMaPhong.Text;
            String cccd = txtCCCD.Text;
            string ngayCheckInString = ngayCheckIn.ToString("yyyy-MM-dd");
            string trangThai = string.Empty; 

            query = @"SELECT PHONG.TRANGTHAI
        FROM PHONG
        JOIN DATPHONG ON PHONG.MAPHONG = DATPHONG.MAPHONG
        JOIN KHACHHANG ON DATPHONG.MAKHACHHANG = KHACHHANG.MAKHACHHANG
        WHERE KHACHHANG.TENKHACHHANG = N'" + tenKH + "' " +
                        "AND CAST(DATPHONG.NGAYCHECKIN AS DATE) ='" + ngayCheckInString + "'" +
                        "AND KHACHHANG.CCCD = '" + cccd + "'" +
                        "AND PHONG.MAPHONG = '" + maPhong + "';";
            DataSet ds = fn.GetData(query);

            // Kiểm tra dữ liệu trước khi truy cập
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // Lấy giá trị từ hàng đầu tiên, cột "TRANGTHAI"
                trangThai = ds.Tables[0].Rows[0]["TRANGTHAI"].ToString();
            }
            else
            {
                // Xử lý khi không có dữ liệu
                MessageBox.Show("Không tìm thấy dữ liệu!");
                return; // Kết thúc hàm nếu không tìm thấy dữ liệu
            }

                Console.WriteLine(trangThai);
            // Kiểm tra và xử lý trạng thái phòng
            if (!string.IsNullOrEmpty(trangThai) && trangThai.ToUpper() == "ĐÃ ĐẶT")
            {
                query = "UPDATE PHONG SET TRANGTHAI = N'ĐANG SỬ DỤNG' WHERE MAPHONG = '" + maPhong + "'";
                fn.SetData(query, "CheckIN thành công");
            }
            else
            {
                MessageBox.Show("Vui lòng cập nhật trạng thái đã đặt cho khách hàng!!");
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem dòng được nhấp có hợp lệ không
            if (e.RowIndex >= 0)
            {
                // Lấy dòng được chọn
                DataGridViewRow selectedRow = dtgvCheckIN.Rows[e.RowIndex];

                // Gán giá trị từ cột cụ thể vào các biến
                txtTenKHCheckIn.Text = selectedRow.Cells["TENKHACHHANG"].Value?.ToString() ?? string.Empty;
                dtpNgayCheckIn.Text = selectedRow.Cells["NGAYCHECKIN"].Value?.ToString() ?? string.Empty;
                txtMaPhong.Text = selectedRow.Cells["MAPHONG"].Value?.ToString() ?? string.Empty;
                txtCCCD.Text = selectedRow.Cells["CCCD"].Value?.ToString() ?? string.Empty;

            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng hợp lệ!");
            }
        }

    }
}
