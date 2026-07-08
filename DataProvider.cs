using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace Form_QL_ShopQuanAo
{
    public static class DataProvider
    {
        private static string connString;

        public static void SetConnectionString(string server, string database, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(database))
                throw new ArgumentException("Server và Database không được rỗng!");

            connString = $"Data Source={server};Initial Catalog={database};User ID={username};Password={password};TrustServerCertificate=True;";
        }

        public static string GetConnectionString()
        {
            if (string.IsNullOrEmpty(connString))
                throw new InvalidOperationException("⚠️ ConnectionString chưa được khởi tạo. Gọi SetConnectionString trước khi sử dụng.");
            return connString;
        }

        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Không thể kết nối tới SQL Server: " + ex.Message);
                return false;
            }
        }

        public static DataTable LoadCSDL(string query)
        {
            DataTable dt = new DataTable();

            if (string.IsNullOrWhiteSpace(query))
            {
                Debug.WriteLine("Câu lệnh truy vấn rỗng!");
                return dt;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Lỗi SQL khi thực thi query: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi không xác định khi tải dữ liệu: {ex.Message}");
            }

            return dt;
        }

        public static bool ExecuteNonQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                Debug.WriteLine("Câu lệnh không hợp lệ!");
                return false;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Lỗi SQL khi thực thi non-query: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi không xác định khi thực thi câu lệnh: {ex.Message}");
                return false;
            }
        }

        public static List<string> GetAllDatabases(string server, string username, string password)
        {
            List<string> databases = new List<string>();
            string tempConnStr = $"Data Source={server};Initial Catalog=master;User ID={username};Password={password};TrustServerCertificate=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(tempConnStr))
                {
                    conn.Open();
                    string sql = "SELECT name FROM sys.databases WHERE database_id > 4";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            databases.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Không thể lấy danh sách database từ server: " + ex.Message);
            }

            return databases;
        }

        public static DataTable LoadCSDL_Silent(string query)
        {
            DataTable dt = new DataTable();

            if (string.IsNullOrWhiteSpace(query))
                return dt;

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            catch
            {
                // Bỏ qua lỗi
            }

            return dt;
        }

        public static bool TestConnectionSilent()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    conn.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}