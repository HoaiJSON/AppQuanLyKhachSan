using System;
using System.Collections;
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
    public partial class UC_CheckOut : UserControl
    {
        functions fn = new functions();
        String query;
        public UC_CheckOut()
        {
            InitializeComponent();
        }

        private void UC_CheckOut_Load(object sender, EventArgs e)
        {
            dtPCheckOut.Value = DateTime.Now;

            query = "select KHACHHANG.MAKHACHHANG, KHACHHANG.TENKHACHHANG," +
                    "KHACHHANG.DIENTHOAI, KHACHHANG.QUOCTICH, KHACHHANG.GIOITINH, " +
                    "KHACHHANG.NGAYSINH, KHACHHANG.CCCD, KHACHHANG.DIACHI, " +
                    "PHONG.MAPHONG,PHONG.MAPHONG, PHONG.LOAIPHONG, PHONG.LOAIGIUONG, PHONG.GIA," +
                    "DATPHONG.NGAYCHECKIN,DATPHONG.NGAYCHECKOUT,DATPHONG.MADATPHONG FROM KHACHHANG " +
                    "JOIN DATPHONG ON KHACHHANG.MAKHACHHANG = DATPHONG.MAKHACHHANG " +
                    "JOIN PHONG ON DATPHONG.MAPHONG = PHONG.MAPHONG WHERE PHONG.TRANGTHAI = N'ĐANG SỬ DỤNG'" +
                    "AND DATPHONG.TRANGTHAI != N'ĐÃ THANH TOÁN';";

            DataSet ds = fn.GetData(query);
            guna2DataGridView1.DataSource = ds.Tables[0];
        }


        String makh;
        DateTime ngaycheckin;
        Int64 gia;
        Int64 sDT;
        String madatphong;
        String maphong;
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.Rows[e.RowIndex].Cells[e.RowIndex].Value != null) 
            {
                dtPCheckOut.Text = guna2DataGridView1.Rows[e.RowIndex].Cells[14].Value.ToString();
                makh = guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtcname.Text = guna2DataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtNumberR.Text = guna2DataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
                sDT = Int64.Parse(guna2DataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                gia = Int64.Parse(guna2DataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString());
                ngaycheckin = Convert.ToDateTime(guna2DataGridView1.Rows[e.RowIndex].Cells[13].Value);
                madatphong = guna2DataGridView1.Rows[e.RowIndex].Cells[15].Value.ToString();
                maphong = guna2DataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            

            Payment frmPayment = new Payment();
            frmPayment.tenkhachhang = txtcname.Text;
            frmPayment.makhachhang = makh;
            frmPayment.ngaycheckin = ngaycheckin;
            frmPayment.ngaycheckout = dtPCheckOut.Value;
            frmPayment.mahoadon = GenerateRandomInvoiceCode(5);
            frmPayment.giaphong = gia;
            frmPayment.sodienthoai = sDT;
            frmPayment.maDatPhong = madatphong;
            frmPayment.maPhong = maphong;
            frmPayment.ShowDialog();
        }

        public static string GenerateRandomInvoiceCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";  // Bao gồm chữ cái và số
            Random random = new Random();
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }

        private void ClearAll()
        {
            txtcname.Clear();
            txtName.Clear();
            txtNumberR.Clear();
            dtPCheckOut.ResetText();
        }

        private void UC_CheckOut_Leave(object sender, EventArgs e)
        {
            ClearAll();
        }

        public void ReloadData()
        {
            UC_CheckOut_Load(this, null); // Gọi lại phương thức Load để tải lại dữ liệu
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            query = "select KHACHHANG.MAKHACHHANG, KHACHHANG.TENKHACHHANG,KHACHHANG.DIENTHOAI, KHACHHANG.QUOCTICH, KHACHHANG.GIOITINH, KHACHHANG.NGAYSINH, KHACHHANG.CCCD, KHACHHANG.DIACHI, PHONG.SOPHONG, PHONG.LOAIPHONG, PHONG.LOAIGIUONG, PHONG.GIA,DATPHONG.NGAYCHECKIN,DATPHONG.NGAYCHECKOUT FROM KHACHHANG JOIN DATPHONG ON KHACHHANG.MAKHACHHANG = DATPHONG.MAKHACHHANG JOIN PHONG ON DATPHONG.MAPHONG = PHONG.MAPHONG WHERE KHACHHANG.TENKHACHHANG LIKE N'" + txtName.Text + "%' AND DATPHONG.TRANGTHAI = N'CHƯA THANH TOÁN'";
            DataSet ds = fn.GetData(query);
            guna2DataGridView1.DataSource = ds.Tables[0];
        }
    }
}
