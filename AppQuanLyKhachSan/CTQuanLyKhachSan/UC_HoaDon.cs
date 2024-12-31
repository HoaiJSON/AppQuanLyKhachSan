using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppQuanLyKhachSan.CTQuanLyKhachSan
{
    public partial class UC_HoaDon : UserControl
    {
        functions fn = new functions();
        String query;
        public UC_HoaDon()
        {
            InitializeComponent();
        }

        private void UC_HoaDon_Load(object sender, EventArgs e)
        {
            query = @"	SELECT 
                HOADON.MAHD,
                KHACHHANG.TENKHACHHANG,
                KHACHHANG.DIENTHOAI AS 'SDT_KHACHHANG',
                NHANVIEN.TENNHANVIEN,
                DATPHONG.MAPHONG,
                PHONG.SOPHONG,
                PHONG.LOAIPHONG,
	            NGAYCHECKIN,
	            NGAYCHECKOUT,
                HOADON.NGAYLAP,
                STRING_AGG(DICHVU.TENDICHVU, ', ') AS 'DICHVU', 
                SUM(CHITIETHOADON.SOLUONG) AS 'SL_DICHVU',
                SUM(CHITIETHOADON.GIA) AS 'GIA_DICHVU',
                HOADON.TONGTIEN
                FROM HOADON
                INNER JOIN KHACHHANG ON HOADON.MAKHACHHANG = KHACHHANG.MAKHACHHANG
                INNER JOIN NHANVIEN ON HOADON.MANHANVIEN = NHANVIEN.MANHANVIEN
                INNER JOIN DATPHONG ON HOADON.MADATPHONG = DATPHONG.MADATPHONG
                INNER JOIN PHONG ON DATPHONG.MAPHONG = PHONG.MAPHONG
                LEFT JOIN CHITIETHOADON ON HOADON.MAHD = CHITIETHOADON.MAHD
                LEFT JOIN DICHVU ON CHITIETHOADON.MADICHVU = DICHVU.MADICHVU
                GROUP BY HOADON.MAHD, KHACHHANG.TENKHACHHANG, KHACHHANG.DIENTHOAI, NHANVIEN.TENNHANVIEN, DATPHONG.MAPHONG, PHONG.SOPHONG, PHONG.LOAIPHONG, NGAYCHECKIN, NGAYCHECKOUT, HOADON.NGAYLAP, HOADON.TONGTIEN;";
            DataSet ds = fn.GetData(query);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Chọn cả hàng
            dataGridView1.MultiSelect = false; // Chỉ chọn được một hàng tại một thời điểm

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                DataGridViewRow firstRow = dataGridView1.Rows[0];
                String mahd = firstRow.Cells["MAHD"].Value?.ToString(); // Lấy mã hóa đơn (MAHD)

                query = "DELETE FROM DATPHONG " +
                        "WHERE MADATPHONG = (SELECT MADATPHONG FROM HOADON WHERE MAHD = '" + mahd + "');" +
                        "DELETE FROM THONGKEDOANHTHU " +
                        "WHERE MAHOADON = '" + mahd + "';" +
                        "DELETE FROM CHITIETHOADON " +
                        "WHERE MAHD = '" + mahd + "';" +
                        "DELETE FROM HOADON " +
                        "WHERE MAHD = '" + mahd + "';";

                fn.SetData(query, "Xóa thành công!!");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            query = @"SELECT HOADON.MAHD, KHACHHANG.TENKHACHHANG, KHACHHANG.DIENTHOAI AS 'SDT_KHACHHANG',
                NHANVIEN.TENNHANVIEN, DATPHONG.MAPHONG,
                PHONG.SOPHONG,
                PHONG.LOAIPHONG,
                NGAYCHECKIN,
                NGAYCHECKOUT,
                HOADON.NGAYLAP,
                STRING_AGG(DICHVU.TENDICHVU, ', ') AS 'DICHVU', 
                SUM(CHITIETHOADON.SOLUONG) AS 'SL_DICHVU',
                SUM(CHITIETHOADON.GIA) AS 'GIA_DICHVU',
                HOADON.TONGTIEN
                FROM HOADON
                INNER JOIN KHACHHANG ON HOADON.MAKHACHHANG = KHACHHANG.MAKHACHHANG
                INNER JOIN NHANVIEN ON HOADON.MANHANVIEN = NHANVIEN.MANHANVIEN
                INNER JOIN DATPHONG ON HOADON.MADATPHONG = DATPHONG.MADATPHONG
                INNER JOIN PHONG ON DATPHONG.MAPHONG = PHONG.MAPHONG
                LEFT JOIN CHITIETHOADON ON HOADON.MAHD = CHITIETHOADON.MAHD
                LEFT JOIN DICHVU ON CHITIETHOADON.MADICHVU = DICHVU.MADICHVU " +
                @"WHERE KHACHHANG.TENKHACHHANG LIKE N'" + txtName.Text + "%' " +
                @" AND DATPHONG.TRANGTHAI = N'ĐÃ THANH TOÁN'
                GROUP BY
                HOADON.MAHD, 
                KHACHHANG.TENKHACHHANG, 
                KHACHHANG.DIENTHOAI, 
                NHANVIEN.TENNHANVIEN, 
                DATPHONG.MAPHONG, 
                PHONG.SOPHONG, 
                PHONG.LOAIPHONG, 
                NGAYCHECKIN, 
                NGAYCHECKOUT, 
                HOADON.NGAYLAP, 
                HOADON.TONGTIEN;";
            DataSet ds = fn.GetData(query);
            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
