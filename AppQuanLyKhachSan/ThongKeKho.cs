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
    public partial class btnThongKeKho : Form
    {
        functions fn = new functions();
        String query;
        public btnThongKeKho()
        {
            InitializeComponent();
        }

        private void ThongKeKho_Load(object sender, EventArgs e)
        {

        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            if (rbtnAll.Checked)
            {
                query = @"SELECT MAKHO AS MaKho,TENHANG AS TenHang, DANHMUC AS DanhMuc,DONVITINH,
                         SOLUONG AS SoLuongTon,GIADONVI AS GiaDonVi FROM KHO
                         ORDER BY DANHMUC, TENHANG;";
                DataSet ds = fn.GetData(query);
                dataGridView1.DataSource = ds.Tables[0];

            }
            if (rbtnDaHet.Checked) 
            {
                query = @"SELECT MAKHO AS MaKho,
                        TENHANG AS TenHang,
                        DANHMUC AS DanhMuc,
                        SOLUONG AS SoLuongTon,
                        GIADONVI AS GiaDonVi,
                        DONVITINH
                        FROM KHO WHERE SOLUONG = 0 ORDER BY DANHMUC, TENHANG;";
                DataSet ds = fn.GetData(query);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                }
                else
                {
                    MessageBox.Show("không có sản phẩm nào đã hết");
                }

            }
            if (rbtnGanHet.Checked)
            {
                query = @"SELECT MAKHO AS MaKho,
                        TENHANG AS TenHang,
                        DANHMUC AS DanhMuc,
                        SOLUONG AS SoLuongTon,
                        GIADONVI AS GiaDonVi,
                        DONVITINH
                        FROM KHO WHERE SOLUONG < 10 ORDER BY DANHMUC, TENHANG;";
                DataSet ds = fn.GetData(query);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                }
                else
                {
                    MessageBox.Show("không có sản phẩm nào đã hết");
                }
            }
        }
    }
}
