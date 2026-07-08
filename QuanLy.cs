using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;

namespace Form_QL_ShopQuanAo
{
    public partial class QuanLy : Form
    {
        string connString;
        string filePath = "";
        private string currentUserRole = "";
        private string currentUserName = "";

        public QuanLy()
        {
            InitializeComponent();
            connString = DataProvider.GetConnectionString();
            this.Load += QuanLy_Load;
        }
        string connStr = "Server=DESKTOP-DJ1KDIM\\SQLEXPRESS;Database=QLShopQuanAo;User Id=sa;Password=123;";
        // THÊM CONSTRUCTOR NÀY VÀO CLASS QuanLy
        public QuanLy(string connectionString, string username)
        {
            InitializeComponent();

            // Lưu ConnectionString và username nhận được từ form đăng nhập
            this.connString = connectionString;
            this.currentUserName = username;

            // Gọi các hàm khởi tạo
            GetCurrentUserInfo();
            ApplyDynamicPermissions();

            // Cập nhật tiêu đề
            this.Text = $"Quản Lý Shop - User: {currentUserName} - Role: {currentUserRole}";

            MessageBox.Show($"Kết nối thành công! Chào {username}");
        }

        // Hàm lấy thông tin user hiện tại
        private void GetCurrentUserInfo()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // GỌI THỦ TỤC ĐỂ LẤY THÔNG TIN USER VÀ ROLE CAO NHẤT
                    SqlCommand cmd = new SqlCommand("sp_GetCurrentUserInfo", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@UserRole", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    // LẤY KẾT QUẢ TỪ OUTPUT PARAMETERS
                    currentUserName = cmd.Parameters["@UserName"].Value?.ToString() ?? "Unknown";
                    currentUserRole = cmd.Parameters["@UserRole"].Value?.ToString() ?? "KhachHang";

                    // Cập nhật tiêu đề form
                    this.Text = $"Quản Lý Shop - User: {currentUserName} - Role: {currentUserRole}";

                    // Ẩn nút quản lý user VÀ combobox quản lý quyền nếu là nhân viên
                    // 🎯 BÂY GIỜ CHỈ KIỂM TRA THEO ROLE CAO NHẤT
                    if (currentUserRole.ToLower() == "nhanvien")
                    {
                        btnUser.Visible = false;
                        cmbRole.Visible = false;
                    }
                    else
                    {
                        btnUser.Visible = true;
                        cmbRole.Visible = true;
                    }

                    // Debug để kiểm tra
                    Debug.WriteLine($"User: {currentUserName}, Highest Role: {currentUserRole}");
                }
            }
            catch (Exception ex)
            {
                currentUserRole = "KhachHang";
                currentUserName = "Unknown";
                Debug.WriteLine("Lỗi lấy thông tin user: " + ex.Message);
            }
        }

        // Hàm kiểm tra quyền chi tiết
        // Hàm kiểm tra quyền chi tiết - DÙNG HÀM SQL ĐỂ KIỂM TRA QUYỀN THỰC TẾ
        private bool CheckUserPermission(string tableName, string permissionType)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // DÙNG HAS_PERMS_BY_NAME để kiểm tra quyền THỰC TẾ của user
                    string query = @"
                SELECT HAS_PERMS_BY_NAME(@tableName, 'OBJECT', @permissionType)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@tableName", tableName);
                    cmd.Parameters.AddWithValue("@permissionType", permissionType);

                    int result = (int)cmd.ExecuteScalar();
                    bool hasPermission = result == 1;

                    Debug.WriteLine($"Quyền {permissionType} trên {tableName}: {hasPermission}");
                    return hasPermission;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi kiểm tra quyền {permissionType} trên {tableName}: " + ex.Message);
                return false;
            }
        }

        // Hiển thị thông báo không có quyền
        private void ShowPermissionDenied(string operation)
        {
            MessageBox.Show($"Bạn không có quyền {operation}!\n\n" +
                           $"User: {currentUserName}\n" +
                           $"Role: {currentUserRole}",
                           "Không có quyền",
                           MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // Áp dụng phân quyền động
        // Áp dụng phân quyền động - KIỂM TRA QUYỀN THỰC TẾ
        private void ApplyDynamicPermissions()
        {
            try
            {
                // 🔹 KIỂM TRA QUYỀN THỰC TẾ TRÊN TỪNG BẢNG - KHÔNG QUA ROLE
                bool canInsertSanPham = CheckUserPermission("SANPHAM", "INSERT");
                bool canUpdateSanPham = CheckUserPermission("SANPHAM", "UPDATE");
                bool canDeleteSanPham = CheckUserPermission("SANPHAM", "DELETE");

                bool canInsertNhanVien = CheckUserPermission("NHANVIEN", "INSERT");
                bool canUpdateNhanVien = CheckUserPermission("NHANVIEN", "UPDATE");
                bool canDeleteNhanVien = CheckUserPermission("NHANVIEN", "DELETE");

                bool canInsertHoaDon = CheckUserPermission("HOADON", "INSERT");
                bool canInsertKhachHang = CheckUserPermission("KHACHHANG", "INSERT");

                // Quyền import/export - KIỂM TRA THỰC TẾ
                bool canImportAll = canInsertSanPham && canInsertNhanVien && canInsertKhachHang && canInsertHoaDon;
                bool canExport = CheckUserPermission("SANPHAM", "SELECT") &&
                                CheckUserPermission("KHACHHANG", "SELECT") &&
                                CheckUserPermission("NHANVIEN", "SELECT");

                // 🔹 ÁP DỤNG QUYỀN THEO KẾT QUẢ KIỂM TRA THỰC TẾ

                // Import/Export
                Import.Enabled = canInsertSanPham; // Import sheet
                ImportAll.Enabled = canImportAll; // Import tất cả
                button2.Enabled = canExport; // Export toàn bộ

                // Quản lý nhân viên/sản phẩm
                Sua.Enabled = canUpdateSanPham || canUpdateNhanVien;
                Xoa.Enabled = canDeleteSanPham || canDeleteNhanVien;

                // Thêm mới - QUAN TRỌNG: DÙ LÀ KHACHHANG NHƯNG CÓ QUYỀN INSERT THÌ VẪN ĐƯỢC
                //radioButton3.Enabled = canInsertSanPham; // Thêm sản phẩm
                // ThemNhanVien.Enabled = canInsertNhanVien; // Thêm nhân viên
                //ThemNhanVien.Enabled = false;

                // Debug info - xem quyền thực tế
                Debug.WriteLine($"=== PHÂN QUYỀN THỰC TẾ ===");
                Debug.WriteLine($"User: {currentUserName}, Role: {currentUserRole}");
                Debug.WriteLine($"INSERT SANPHAM: {canInsertSanPham}");
                Debug.WriteLine($"INSERT NHANVIEN: {canInsertNhanVien}");
                Debug.WriteLine($"Thêm NV enabled: {ThemNhanVien.Enabled}");
                Debug.WriteLine($"========================");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi áp dụng phân quyền: " + ex.Message, "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void QuanLy_Load(object sender, EventArgs e)
        {
            lblTopN.Visible = false;
            txtTopN.Visible = false;

            // Lấy thông tin user và áp dụng phân quyền
            GetCurrentUserInfo();
            ApplyDynamicPermissions();

            //Timer timer = new Timer();
            //timer.Interval = 3000;
            //timer.Tick += Timer_Tick;
            //timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                string query = "EXEC sp_KiemTraSession";
                DataTable dt = DataProvider.LoadCSDL(query);

                if (dt.Rows.Count > 0)
                {
                    int isActive = Convert.ToInt32(dt.Rows[0][0]);

                    if (isActive == 0)
                    {
                        MessageBox.Show("Phiên làm việc đã kết thúc. Hệ thống sẽ tự động đăng xuất!");
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Lỗi kiểm tra session: " + ex.Message);
            }
        }

        // ====== CHỌN FILE EXCEL ======
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckUserPermission("SANPHAM", "INSERT"))
                {
                    ShowPermissionDenied("import dữ liệu");
                    return;
                }

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Excel Files|*.xlsx;*.xls";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    filePath = ofd.FileName;

                    try
                    {
                        FileInfo fileInfo = new FileInfo(filePath);
                        using (var package = new ExcelPackage(fileInfo))
                        {
                            if (package.Workbook.Worksheets.Count == 0)
                            {
                                MessageBox.Show("File Excel không có sheet nào.");
                                return;
                            }

                            comboBox1.Items.Clear();
                            foreach (var ws in package.Workbook.Worksheets)
                            {
                                comboBox1.Items.Add(ws.Name);
                            }

                            MessageBox.Show("Đã tải danh sách sheet từ Excel. Hãy chọn sheet để import!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi đọc file Excel: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====== ĐỌC EXCEL THÀNH DATATABLE ======
        private DataTable ReadExcelToDataTable(string filePath, string sheetName)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            DataTable dt = new DataTable();

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null)
                {
                    MessageBox.Show("Không tìm thấy sheet " + sheetName);
                    return null;
                }

                bool hasHeader = true;

                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    string colName = hasHeader ? worksheet.Cells[1, col].Text : $"Column {col}";
                    dt.Columns.Add(colName);
                }

                int startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                {
                    DataRow row = dt.NewRow();
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        row[col - 1] = worksheet.Cells[rowNum, col].Text;
                    }
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }

        // ====== HÀM GỌI BCP ======
        private void ImportWithBCP(DataTable dt, string tableName)
        {
            // B1: Xuất DataTable ra file CSV 
            string tempFile = Path.GetTempFileName();
            using (StreamWriter sw = new StreamWriter(tempFile, false, new UTF8Encoding(true)))
            {
                foreach (DataRow row in dt.Rows)
                {
                    string line = string.Join(",", row.ItemArray.Select(f => f.ToString().Trim()));
                    sw.WriteLine(line);
                }
            }

            // B2: import file vào bảng tạm
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connString);
            string tmpTable = tableName + "_TMP";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand dropCmd = new SqlCommand($"IF OBJECT_ID('{tmpTable}', 'U') IS NOT NULL DROP TABLE {tmpTable}", conn);
                dropCmd.ExecuteNonQuery();

                SqlCommand createCmd = new SqlCommand($"SELECT TOP 0 * INTO {tmpTable} FROM {tableName}", conn);
                createCmd.ExecuteNonQuery();
            }

            string bcpCommand = $@"bcp {builder.InitialCatalog}.dbo.{tmpTable} in ""{tempFile}"" -c -C 65001 -t, -S {builder.DataSource} -U {builder.UserID} -P {builder.Password}";

            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + bcpCommand);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new Exception("BCP import lỗi: " + error);
            }

            // B3: MERGE từ bảng tạm vào bảng chính
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string mergeSql = $@"
                MERGE {tableName} AS target
                USING {tmpTable} AS source
                ON target.[{dt.Columns[0].ColumnName}] = source.[{dt.Columns[0].ColumnName}]
                WHEN MATCHED THEN UPDATE SET
                    {string.Join(",", dt.Columns.Cast<DataColumn>().Skip(1).Select(c => $"target.[{c.ColumnName}] = source.[{c.ColumnName}]"))}
                WHEN NOT MATCHED THEN
                    INSERT ({string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => $"[{c.ColumnName}]"))})
                    VALUES ({string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => $"source.[{c.ColumnName}]"))});";

                SqlCommand mergeCmd = new SqlCommand(mergeSql, conn);
                mergeCmd.ExecuteNonQuery();

                SqlCommand dropTmp = new SqlCommand($"DROP TABLE {tmpTable}", conn);
                dropTmp.ExecuteNonQuery();
            }
        }

        // ====== IMPORT CHUNG ======
        private void ImportSheet(string sheetName)
        {
            DataTable dt = ReadExcelToDataTable(filePath, sheetName);
            if (dt == null || dt.Rows.Count == 0) return;

            ImportWithBCP(dt, sheetName);
        }

        // ====== IMPORT BUTTON ======
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckUserPermission("SANPHAM", "INSERT"))
                {
                    ShowPermissionDenied("import dữ liệu");
                    return;
                }

                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("Vui lòng chọn file Excel trước!");
                    return;
                }
                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn sheet cần import!");
                    return;
                }

                string sheetName = comboBox1.SelectedItem.ToString();
                ImportSheet(sheetName);

                MessageBox.Show("Import dữ liệu từ sheet " + sheetName + " thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi import: " + ex.Message);
            }
        }

        // ====== IMPORT TẤT CẢ ======
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckUserPermission("SANPHAM", "INSERT") ||
                    !CheckUserPermission("KHACHHANG", "INSERT") ||
                    !CheckUserPermission("NHANVIEN", "INSERT") ||
                    !CheckUserPermission("HOADON", "INSERT"))
                {
                    ShowPermissionDenied("import tất cả dữ liệu");
                    return;
                }

                // Import theo thứ tự phụ thuộc
                ImportSheet("KHACHHANG");
                ImportSheet("NHANVIEN");
                ImportSheet("THENHANVIEN");
                ImportSheet("SANPHAM");
                ImportSheet("HOADON");
                ImportSheet("CHITIETHOADON");

                MessageBox.Show("Import tất cả sheet thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi import tất cả: " + ex.Message);
            }
        }

        // ====== EXPORT TOÀN BỘ ======
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckUserPermission("SANPHAM", "SELECT") ||
                    !CheckUserPermission("KHACHHANG", "SELECT") ||
                    !CheckUserPermission("NHANVIEN", "SELECT"))
                {
                    ShowPermissionDenied("export dữ liệu");
                    return;
                }

                string[] bangCanXuat = GetTableNames();

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Files|*.xlsx";
                sfd.FileName = "TatCaBang.xlsx";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                using (var package = new OfficeOpenXml.ExcelPackage())
                {
                    foreach (var table in bangCanXuat)
                    {
                        // B1: export bằng bcp ra file csv 
                        string tmpFile = Path.GetTempFileName();
                        ExportWithBCP(table, tmpFile);

                        // B2: đọc csv vào datatable
                        DataTable dt = CsvToDataTable(tmpFile, table);

                        // B3: ghi vào Excel
                        if (dt.Rows.Count > 0)
                        {
                            var ws = package.Workbook.Worksheets.Add(table);
                            ws.Cells["A1"].LoadFromDataTable(dt, true);
                            ws.Cells[ws.Dimension.Address].AutoFitColumns();
                        }
                    }
                    package.SaveAs(new FileInfo(sfd.FileName));
                }

                MessageBox.Show("Xuất tất cả bảng thành công vào file Excel!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message);
            }
        }

        private void ExportWithBCP(string tableName, string outputFile)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connString);

            string bcpCommand = $@"bcp ""SELECT * FROM {builder.InitialCatalog}.dbo.{tableName}"" queryout ""{outputFile}"" -c -C 65001 -t, -S {builder.DataSource} -U {builder.UserID} -P {builder.Password}";

            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + bcpCommand);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new Exception("BCP export lỗi: " + error);
            }
        }

        private DataTable CsvToDataTable(string filePath, string tableName)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table ORDER BY ORDINAL_POSITION",
                    conn
                );
                cmd.Parameters.AddWithValue("@table", tableName);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dt.Columns.Add(reader.GetString(0));
                    }
                }
            }

            // Đọc file CSV 
            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] values = line.Split(',');
                    dt.Rows.Add(values);
                }
            }

            return dt;
        }

        private string[] GetTableNames()
        {
            var tableNames = new List<string>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                DataTable dt = conn.GetSchema("Tables");
                foreach (DataRow row in dt.Rows)
                {
                    string tableName = row["TABLE_NAME"].ToString();

                    if (!tableName.EndsWith("_TMP", StringComparison.OrdinalIgnoreCase))
                    {
                        tableNames.Add(tableName);
                    }
                }
                conn.Close();
            }
            return tableNames.ToArray();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Cập nhật trạng thái session trong DB
                string query = "EXEC sp_TatSession";
                DataProvider.LoadCSDL(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật session: " + ex.Message);
            }

            // 🎯 THÊM CODE NÀY: QUAY LẠI FORM ĐĂNG NHẬP
            this.Hide(); // Ẩn form hiện tại

            // Mở form đăng nhập
            dangnhap loginForm = new dangnhap();
            loginForm.Show();

            // KHÔNG dùng Application.Exit() nữa
        }

        private void ThongKe_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM V_TonKho";
                DataTable dt = DataProvider.LoadCSDL(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu trong view V_TonKho!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thống kê: " + ex.Message);
            }
        }

        private void QuanLy_Load_1(object sender, EventArgs e)
        {
            lblTopN.Visible = false;
            txtTopN.Visible = false;
            cmbRole.Items.Add("Quyền đơn lẻ");
            cmbRole.Items.Add("Nhóm quyền");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string maKH = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(maKH))
            {
                MessageBox.Show("Vui lòng nhập mã khách hàng!", "Thông báo");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DataProvider.GetConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_TimThongTinKhachHang", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MaKH", maKH);
                    cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@GioiTinh", SqlDbType.NVarChar, 10).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@DienThoai", SqlDbType.NVarChar, 15).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    // Tạo DataTable để hiển thị
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Họ tên");
                    dt.Columns.Add("Giới tính");
                    dt.Columns.Add("Điện thoại");
                    dt.Columns.Add("Email");
                    dt.Columns.Add("Địa chỉ");

                    dt.Rows.Add(
                        cmd.Parameters["@HoTen"].Value?.ToString(),
                        cmd.Parameters["@GioiTinh"].Value?.ToString(),
                        cmd.Parameters["@DienThoai"].Value?.ToString(),
                        cmd.Parameters["@Email"].Value?.ToString(),
                        cmd.Parameters["@DiaChi"].Value?.ToString()
                    );

                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
            }
            textBox1.Clear();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(DataProvider.GetConnectionString()))
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand("sp_HoaDonMoiNhat", conn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@MaHD", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@MaKH", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@MaNV", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@NgayLap", SqlDbType.DateTime).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@TongTien", SqlDbType.Decimal).Direction = ParameterDirection.Output;

                            cmd.ExecuteNonQuery();

                            // Tạo DataTable để hiển thị
                            DataTable dt = new DataTable();
                            dt.Columns.Add("Mã HĐ");
                            dt.Columns.Add("Mã KH");
                            dt.Columns.Add("Mã NV");
                            dt.Columns.Add("Ngày lập");
                            dt.Columns.Add("Tổng tiền");

                            dt.Rows.Add(
                                cmd.Parameters["@MaHD"].Value?.ToString(),
                                cmd.Parameters["@MaKH"].Value?.ToString(),
                                cmd.Parameters["@MaNV"].Value?.ToString(),
                                Convert.ToDateTime(cmd.Parameters["@NgayLap"].Value).ToString("dd/MM/yyyy"),
                                cmd.Parameters["@TongTien"].Value?.ToString()
                            );

                            dataGridView1.DataSource = dt;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lấy hóa đơn mới nhất: " + ex.Message);
                    }
                }
                else if (radioButton2.Checked)
                {
                    try
                    {
                        string query = "EXEC SP_LietKeNhanVien";
                        DataTable dt = DataProvider.LoadCSDL(query);
                        dataGridView1.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi liệt kê nhân viên: " + ex.Message);
                    }
                }
                else if (radioButton3.Checked)
                {
                    if (!CheckUserPermission("SANPHAM", "INSERT"))
                    {
                        ShowPermissionDenied("thêm sản phẩm");
                        return;
                    }

                    using (ThemSanPham f = new ThemSanPham())
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                using (SqlConnection conn = new SqlConnection(DataProvider.GetConnectionString()))
                                {
                                    conn.Open();
                                    SqlCommand cmd = new SqlCommand("SP_ThemSanPham", conn);
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.AddWithValue("@MASP", f.MaSP);
                                    cmd.Parameters.AddWithValue("@TENSP", f.TenSP);
                                    cmd.Parameters.AddWithValue("@LOAI", f.Loai);
                                    cmd.Parameters.AddWithValue("@SIZE", f.Size);
                                    cmd.Parameters.AddWithValue("@GIA", f.Gia);
                                    cmd.Parameters.AddWithValue("@SOLUONG", f.SoLuong);

                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Đã thêm sản phẩm thành công!");

                                    // Load lại danh sách
                                    string query = "EXEC sp_XemSanPham";
                                    DataTable dt = DataProvider.LoadCSDL(query);
                                    dataGridView1.DataSource = dt;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message);
                            }
                        }
                    }
                }
                else if (ThemNhanVien.Checked)
                {
                    if (!CheckUserPermission("NHANVIEN", "INSERT"))
                    {
                        ShowPermissionDenied("thêm nhân viên");
                        return;
                    }

                    using (ThemNhanVien f = new ThemNhanVien())
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                using (SqlConnection conn = new SqlConnection(DataProvider.GetConnectionString()))
                                {
                                    conn.Open();
                                    SqlCommand cmd = new SqlCommand("sp_ThemNhanVien", conn);
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.AddWithValue("@MaNV", f.MaNV);
                                    cmd.Parameters.AddWithValue("@HoTen", f.HoTen);
                                    cmd.Parameters.AddWithValue("@SDT", f.SDT);
                                    cmd.Parameters.AddWithValue("@GioiTinh", f.GioiTinh);
                                    cmd.Parameters.AddWithValue("@ChucVu", f.ChucVu);
                                    cmd.Parameters.AddWithValue("@NgayVaoLam", f.NgayVaoLam);
                                    cmd.Parameters.AddWithValue("@Luong", f.Luong);

                                    cmd.ExecuteNonQuery();

                                    MessageBox.Show("Đã thêm nhân viên thành công!");
                                }

                                // Sau khi thêm, có thể load lại danh sách nhân viên:
                                string query = "EXEC SP_LietKeNhanVien";
                                DataTable dt = DataProvider.LoadCSDL(query);
                                dataGridView1.DataSource = dt;
                            }
                            catch (SqlException ex)
                            {
                                // Bắt lỗi từ thủ tục hoặc trigger (RAISERROR)
                                MessageBox.Show("Lỗi SQL: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else if (LuongThucLanh.Checked)
                {
                    try
                    {
                        string query = "EXEC sp_HienThiLuongThucNhan";
                        DataTable dt = DataProvider.LoadCSDL(query);
                        dataGridView1.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi hiển thị lương thực nhận: " + ex.Message);
                    }
                }
                else if (TongLuongThucLanh.Checked)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(DataProvider.GetConnectionString()))
                        {
                            conn.Open();

                            // Gọi hàm SQL
                            string query = "SELECT dbo.fn_TongLuongThucLanh() AS [Tổng lương thực lãnh]";
                            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Hiển thị ra DataGridView
                            dataGridView1.DataSource = dt;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lấy tổng lương thực lãnh: " + ex.Message);
                    }
                }
                else if (LietKeSanPham.Checked)
                {
                    try
                    {
                        string query = "EXEC sp_LietKeSanPham";
                        DataTable dt = DataProvider.LoadCSDL(query);
                        dataGridView1.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi liệt kê sản phẩm: " + ex.Message);
                    }
                }
                else if (ChiTietHoaDon.Checked)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(DataProvider.GetConnectionString()))
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand("sp_ChiTietHoaDon", conn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            dataGridView1.DataSource = dt;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xem chi tiết hóa đơn: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thực hiện thao tác: " + ex.Message);
            }
            finally
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                ThemNhanVien.Checked = false;
                LuongThucLanh.Checked = false;
                TongLuongThucLanh.Checked = false;
                LietKeSanPham.Checked = false;
                ChiTietHoaDon.Checked = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (SoLuongSPConLai.Checked)
                {
                    string maSP = txtTopN.Text.Trim();
                    if (string.IsNullOrEmpty(maSP))
                    {
                        MessageBox.Show("Vui lòng nhập mã sản phẩm!");
                        return;
                    }

                    try
                    {
                        string query = $"SELECT '{maSP}' AS MaSanPham, dbo.fn_SoLuong('{maSP}') AS SoLuong";
                        DataTable dt = DataProvider.LoadCSDL(query);
                        dataGridView1.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xem số lượng sản phẩm: " + ex.Message);
                    }

                    SoLuongSPConLai.Checked = false;
                    lblTopN.Visible = false;
                    txtTopN.Visible = false;
                    txtTopN.Clear();
                }
                else if (TongSoNhanVien.Checked)
                {
                    try
                    {
                        string query = "SELECT dbo.fn_TongSoNhanVien() AS TongSoNhanVien";
                        DataTable dt = DataProvider.LoadCSDL(query);
                        dataGridView1.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xem tổng số nhân viên: " + ex.Message);
                    }

                    TongSoNhanVien.Checked = false;
                    lblTopN.Visible = false;
                    txtTopN.Visible = false;
                    txtTopN.Clear();
                }
                else if (SanPhamBanChay.Checked)
                {
                    if (string.IsNullOrWhiteSpace(txtTopN.Text))
                    {
                        MessageBox.Show("Vui lòng nhập số lượng TopN!");
                        return;
                    }

                    if (!int.TryParse(txtTopN.Text.Trim(), out int topN) || topN <= 0)
                    {
                        MessageBox.Show("TopN phải là số nguyên dương!");
                        return;
                    }

                    try
                    {
                        string query = $"SELECT * FROM dbo.f_TopSanPhamBanChay2({topN})";
                        DataTable dt = DataProvider.LoadCSDL(query);
                        dataGridView1.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lấy sản phẩm bán chạy: " + ex.Message);
                    }

                    SanPhamBanChay.Checked = false;
                    lblTopN.Visible = false;
                    txtTopN.Visible = false;
                    txtTopN.Clear();
                }
                else if (KhachHangGanDay.Checked)
                {
                    try
                    {
                        string query = "SELECT * FROM dbo.fn_KhachHangMoiNhat()";
                        DataTable dt = DataProvider.LoadCSDL(query);
                        dataGridView1.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lấy khách hàng gần đây: " + ex.Message);
                    }

                    KhachHangGanDay.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thực hiện thống kê: " + ex.Message);
            }
            finally
            {
                SanPhamBanChay.Checked = false;
                KhachHangGanDay.Checked = false;
                SoLuongSPConLai.Checked = false;
                TongSoNhanVien.Checked = false;
            }
        }

        private void SanPhamBanChay_CheckedChanged(object sender, EventArgs e)
        {
            if (SanPhamBanChay.Checked)
            {
                lblTopN.Text = "Nhập số lượng xem:";
                lblTopN.ForeColor = Color.Blue;
                lblTopN.Visible = true;
                txtTopN.Visible = true;
            }
            else
            {
                lblTopN.Visible = false;
                txtTopN.Visible = false;
            }
        }

        private void KhachHangGanDay_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SoLuongSPConLai_CheckedChanged(object sender, EventArgs e)
        {
            if (SoLuongSPConLai.Checked)
            {
                lblTopN.Text = "Nhập mã sản phẩm:";
                lblTopN.ForeColor = Color.Blue;
                lblTopN.Visible = true;
                txtTopN.Visible = true;
            }
            else
            {
                lblTopN.Visible = false;
                txtTopN.Visible = false;
            }
        }

        private void txtTopN_TextChanged(object sender, EventArgs e)
        {

        }

        private void Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckUserPermission("NHANVIEN", "DELETE") && !CheckUserPermission("SANPHAM", "DELETE"))
                {
                    ShowPermissionDenied("xóa dữ liệu");
                    return;
                }

                // 1️⃣ Kiểm tra có chọn dòng chưa
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn một dòng để xóa!");
                    return;
                }

                // 2️⃣ Xác định loại dữ liệu đang xem
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                bool isNhanVien = dataGridView1.Columns.Contains("MANV");
                bool isSanPham = dataGridView1.Columns.Contains("MASP");

                if (isNhanVien)
                {
                    if (!CheckUserPermission("NHANVIEN", "DELETE"))
                    {
                        ShowPermissionDenied("xóa nhân viên");
                        return;
                    }

                    string maNV = row.Cells["MANV"].Value.ToString();

                    // 3️⃣ Xác nhận trước khi xóa
                    if (MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên {maNV} không?",
                                        "Xác nhận xóa",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question) == DialogResult.No)
                        return;

                    // 4️⃣ Gọi thủ tục xóa
                    using (SqlConnection conn = new SqlConnection(DataProvider.GetConnectionString()))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("sp_XoaNhanVien", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaNV", maNV);

                        cmd.ExecuteNonQuery();
                    }

                    // 5 Load lại danh sách nhân viên
                    string query = "EXEC SP_LietKeNhanVien";
                    DataTable dt = DataProvider.LoadCSDL(query);
                    dataGridView1.DataSource = dt;

                    MessageBox.Show("Đã xóa nhân viên thành công!");
                }
                else if (isSanPham)
                {
                    if (!CheckUserPermission("SANPHAM", "DELETE"))
                    {
                        ShowPermissionDenied("xóa sản phẩm");
                        return;
                    }

                    // Xử lý xóa sản phẩm ở đây nếu cần
                    MessageBox.Show("Chức năng xóa sản phẩm chưa được triển khai!");
                }
            }
            catch (SqlException)
            {
                // Nhận thông báo từ RAISERROR trong SQL
                MessageBox.Show("Không được phép xóa quản lý ! ", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message);
            }
        }

        private void Sua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn một dòng để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow row = dataGridView1.SelectedRows[0];

                // 🔍 Kiểm tra xem DataGridView đang hiển thị bảng nào
                bool isNhanVien = dataGridView1.Columns.Contains("MANV");
                bool isSanPham = dataGridView1.Columns.Contains("MASP");

                if (isNhanVien)
                {
                    if (!CheckUserPermission("NHANVIEN", "UPDATE"))
                    {
                        ShowPermissionDenied("sửa thông tin nhân viên");
                        return;
                    }

                    // ==============================
                    // 🔹 SỬA NHÂN VIÊN
                    // ==============================
                    string maNV = row.Cells["MANV"].Value.ToString();
                    string hoTen = row.Cells["HOTEN"].Value.ToString();
                    string sdt = row.Cells["SDT"].Value.ToString();
                    string gioiTinh = row.Cells["GIOITINH"].Value.ToString();
                    string chucVu = row.Cells["CHUCVU"].Value.ToString();
                    DateTime ngayVaoLam = Convert.ToDateTime(row.Cells["NGAYVAOLAM"].Value);
                    decimal luong = Convert.ToDecimal(row.Cells["LUONG"].Value);

                    using (SqlConnection conn = new SqlConnection(DataProvider.GetConnectionString()))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("sp_SuaNhanVien", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@HoTen", hoTen);
                        cmd.Parameters.AddWithValue("@SDT", sdt);
                        cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                        cmd.Parameters.AddWithValue("@ChucVu", chucVu);
                        cmd.Parameters.AddWithValue("@NgayVaoLam", ngayVaoLam);
                        cmd.Parameters.AddWithValue("@Luong", luong);

                        try
                        {
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Cập nhật thông tin nhân viên thành công!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (SqlException)
                        {
                            MessageBox.Show("Lương nhân viên không được tăng quá 10%!", "Thông báo");
                            return;
                        }
                    }

                    // Sau khi sửa nhân viên → load lại danh sách nhân viên
                    string query = "EXEC SP_LietKeNhanVien";
                    DataTable dt = DataProvider.LoadCSDL(query);
                    dataGridView1.DataSource = dt;
                }
                else if (isSanPham)
                {
                    if (!CheckUserPermission("SANPHAM", "UPDATE"))
                    {
                        ShowPermissionDenied("sửa thông tin sản phẩm");
                        return;
                    }

                    // ==============================
                    // 🔹 SỬA SẢN PHẨM
                    // ==============================
                    string maSP = row.Cells["MASP"].Value.ToString();
                    string tenSP = row.Cells["TENSP"].Value.ToString();
                    string loai = row.Cells["LOAI"].Value.ToString();
                    string size = row.Cells["SIZE"].Value.ToString();
                    decimal gia = Convert.ToDecimal(row.Cells["GIA"].Value);
                    int soLuong = Convert.ToInt32(row.Cells["SOLUONG"].Value);

                    using (SqlConnection conn = new SqlConnection(DataProvider.GetConnectionString()))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("sp_SuaSanPham", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@MASP", maSP);
                        cmd.Parameters.AddWithValue("@TENSP", tenSP);
                        cmd.Parameters.AddWithValue("@LOAI", loai);
                        cmd.Parameters.AddWithValue("@SIZE", size);
                        cmd.Parameters.AddWithValue("@GIA", gia);
                        cmd.Parameters.AddWithValue("@SOLUONG", soLuong);

                        try
                        {
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Cập nhật thông tin sản phẩm thành công!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Không xác định được loại dữ liệu cần sửa!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ThemNhanVien_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRole.SelectedItem == null) return;

            string selectedItem = cmbRole.SelectedItem.ToString();

            if (selectedItem == "Quyền đơn lẻ")
            {
                // Mở form cho item 1
                FormRole formrole = new FormRole();
                formrole.Show();

            }
            else if (selectedItem == "Nhóm quyền")
            {
                // Mở form cho item 2
                FormRoleGroup formrolegroup = new FormRoleGroup();
                formrolegroup.Show();

            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            FormUsers formuser = new FormUsers();
            formuser.Show();

        }
    }
}
