using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Form_QL_ShopQuanAo
{
    public partial class FormUsers : Form
    {
        string connStr = "Server=DESKTOP-DJ1KDIM\\SQLEXPRESS;Database=QLShopQuanAo;User Id=sa;Password=123;";

        public FormUsers()
        {
            InitializeComponent();
        }

        private void FormUsers_Load(object sender, EventArgs e)
        {
            LoadUsersToDataGridView();
            LoadRolesToComboBox();

            // Không cho nhập username, hệ thống tự tạo
            txtUsernam.ReadOnly = true;
            txtUsernam.Text = "Hệ thống tự tạo username";
        }

        // 1. LOAD DANH SÁCH USER LÊN DATAGRIDVIEW
        private void LoadUsersToDataGridView()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetUsersWithRoles", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                    FormatDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách user: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 2. LOAD NHÓM QUYỀN LÊN COMBOBOX
        private void LoadRolesToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Sử dụng Stored Procedure
                    SqlCommand cmd = new SqlCommand("sp_GetAllRoles", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dr = cmd.ExecuteReader();

                    cmbRole.Items.Clear();
                    cmbRole.Items.Add("-- Chọn nhóm quyền --");

                    while (dr.Read())
                    {
                        // Giả sử stored procedure trả về cột "RoleName"
                        string roleName = dr["RoleName"].ToString();
                        cmbRole.Items.Add(roleName);
                    }
                    dr.Close();

                    cmbRole.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách nhóm quyền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 3. NÚT THÊM USER - TẠO TÀI KHOẢN MỚI
        private void btnThem_Click(object sender, EventArgs e)
        {
            string password = txtMatKhau.Text.Trim();
            string role = cmbRole.SelectedIndex > 0 ? cmbRole.Text : "NhanVien";

            // Kiểm tra mật khẩu
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("⚠️ Vui lòng nhập mật khẩu!", "Thông báo");
                txtMatKhau.Focus();
                return;
            }

            if (password.Length < 3)
            {
                MessageBox.Show("⚠️ Mật khẩu phải có ít nhất 3 ký tự!", "Thông báo");
                txtMatKhau.Focus();
                return;
            }

            // Kiểm tra đã chọn nhóm quyền chưa
            if (cmbRole.SelectedIndex == 0)
            {
                MessageBox.Show("⚠️ Vui lòng chọn nhóm quyền!", "Thông báo");
                cmbRole.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // GỌI STORED PROCEDURE TẠO TÀI KHOẢN
                    SqlCommand cmd = new SqlCommand("sp_TaoTaiKhoanTuDong", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MatKhau", password);
                    cmd.Parameters.AddWithValue("@VaiTro", role);
                    cmd.Parameters.AddWithValue("@NguoiTao", "Admin"); // Hoặc lấy từ session

                    SqlParameter outputParam = new SqlParameter("@TenDangNhapOUT", SqlDbType.NVarChar, 50);
                    outputParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParam);

                    cmd.ExecuteNonQuery();

                    string generatedUsername = outputParam.Value?.ToString();

                    if (string.IsNullOrEmpty(generatedUsername))
                    {
                        MessageBox.Show("❌ Không thể tạo tài khoản!", "Lỗi");
                        return;
                    }

                    MessageBox.Show($"✅ Tạo tài khoản thành công!\nTên đăng nhập: {generatedUsername}\nNhóm quyền: {role}",
                                  "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset form
                    txtMatKhau.Text = "";
                    cmbRole.SelectedIndex = 0;
                    txtUsernam.Text = "Hệ thống tự tạo username";

                    // Load lại danh sách user
                    LoadUsersToDataGridView();
                }
            }
            catch (SqlException ex)
            {
                HandleSqlError(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 4. NÚT XÓA USER
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("⚠️ Vui lòng chọn user cần xóa!", "Thông báo");
                return;
            }

            string userName = dataGridView1.SelectedRows[0].Cells["UserName"].Value?.ToString();

            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("❌ Không thể xác định user!", "Lỗi");
                return;
            }

            // Xác nhận xóa
            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa user '{userName}'?",
                                                "Xác nhận xóa",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("sp_XoaUser", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TenDangNhap", userName);
                        cmd.Parameters.AddWithValue("@NguoiThucHien", "Admin"); // Hoặc lấy từ session

                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"✅ Đã xóa user '{userName}' thành công!",
                                      "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Load lại danh sách
                        LoadUsersToDataGridView();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"❌ Lỗi khi xóa user: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 5. NÚT CẬP NHẬT - LOAD LẠI DANH SÁCH
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            LoadUsersToDataGridView();
            MessageBox.Show("✅ Đã cập nhật danh sách user!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 6. NÚT THOÁT
        private void btnExit_Click(object sender, EventArgs e)
        {
            
            this.Close();
            // Form QuanLy sẽ hiện lại nếu đang ẩn
        }

        // 7. ĐỊNH DẠNG DATAGRIDVIEW
        private void FormatDataGridView()
        {
            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns["UserName"].HeaderText = "Tên đăng nhập";
                dataGridView1.Columns["RoleName"].HeaderText = "Nhóm quyền";
                dataGridView1.Columns["CreatedDate"].HeaderText = "Ngày tạo";
                dataGridView1.Columns["UserType"].HeaderText = "Loại user";

                dataGridView1.Columns["CreatedDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                dataGridView1.Columns["UserName"].Width = 120;
                dataGridView1.Columns["RoleName"].Width = 100;
                dataGridView1.Columns["CreatedDate"].Width = 130;
                dataGridView1.Columns["UserType"].Width = 80;

                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.SortMode = DataGridViewColumnSortMode.Automatic;
                }

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        // 8. XỬ LÝ LỖI SQL
        private void HandleSqlError(SqlException ex)
        {
            string errorMessage = ex.Message.ToLower();

            if (ex.Number == 15007 || errorMessage.Contains("tên đăng nhập đã tồn tại"))
            {
                MessageBox.Show("❌ Tên đăng nhập đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (errorMessage.Contains("password") || errorMessage.Contains("mật khẩu"))
            {
                MessageBox.Show("❌ Mật khẩu không đủ mạnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (errorMessage.Contains("permission") || errorMessage.Contains("quyền"))
            {
                MessageBox.Show("❌ Không có quyền thực hiện thao tác này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // CÁC SỰ KIỆN KHÁC
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}