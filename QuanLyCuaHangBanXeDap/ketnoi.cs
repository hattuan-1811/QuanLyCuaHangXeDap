using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangBanXeDap
{
    internal class ketnoi
    {
        private string connectionString = "Data Source=LEVANQUYEN\\SQLEXPRESS;Initial Catalog=quanlyxedap;Integrated Security=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public DataTable ExecuteQuery(string query)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public void ExecuteNonQuery(string query)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
