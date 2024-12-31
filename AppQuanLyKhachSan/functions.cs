using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AppQuanLyKhachSan
{
    class functions
    {
        protected SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=LAPTOP-V78FSG02\\CLIENT2;Initial Catalog=QLKS_CN001;User ID=sa;Password=123456;TrustServerCertificate=True;";
            return con;
        }

        public DataSet GetData(string query) // lấy dữ liệu
        {
            SqlConnection conn = GetConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = query;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public void SetData(string query, string message)
        {
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show(message, "Success", MessageBoxButtons.OK , MessageBoxIcon.Information);
        }

        public SqlDataReader GetForCombo(string query)
        {
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand(query, con);

            con.Open(); // Mở kết nối trước khi thực hiện lệnh
            SqlDataReader sdr = cmd.ExecuteReader();
            return sdr; // Tự động đóng kết nối khi reader bị đóng
        }

        public object GetScalarValue(string query)
        {
            using (SqlConnection conn = GetConnection()) // Sử dụng 'using' để tự động đóng kết nối
            {
                conn.Open(); // Mở kết nối
                SqlCommand cmd = new SqlCommand(query, conn); // Khởi tạo câu lệnh SQL
                return cmd.ExecuteScalar(); // Thực thi và trả về giá trị đầu tiên trong kết quả (ví dụ như COUNT(*))
            }
        }

        // Hàm GetValue trả về giá trị đầu tiên từ câu lệnh SELECT
        public string GetValue(string query)
        {
            object result = GetScalarValue(query); // Gọi phương thức GetScalarValue
            return result != null ? result.ToString() : string.Empty; // Nếu kết quả trả về là null, trả về chuỗi rỗng
        }

    }
}
