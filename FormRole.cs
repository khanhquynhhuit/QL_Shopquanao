using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Form_QL_ShopQuanAo
{
    public partial class FormRole : Form
    {
        public FormRole()
        {
            InitializeComponent();
        }

        // THỬ ĐỔI THÀNH KẾT NỐI VỚI USER/PASSWORD
        string connStr = "Server=DESKTOP-DJ1KDIM\\SQLEXPRESS;Database=QLShopQuanAo;User Id=sa;Password=123;";
        private DataTable currentPermissions;

        private void FormRole_Load(object sender, EventArgs e)
        {
            // 🎯 QUAN TRỌNG: Khởi tạo DataGridView trước khi load dữ liệu
            InitializePermissionsGridView();
            LoadUserList();
            LoadTableList();

            // DEBUG: KIỂM TRA COMBOBOX
            dataGridView1.CellBeginEdit += dataGridView1_CellBeginEdit;
        }
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Chỉ xử lý khi click vào các cột checkbox và không phải header
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

                // Chỉ xử lý các cột permission (checkbox)
                if (columnName == "HasSelect" || columnName == "HasInsert" ||
                    columnName == "HasUpdate" || columnName == "HasDelete")
                {
                    // Lấy giá trị hiện tại của ô
                    var currentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    bool currentValue = currentCell.Value != null && Convert.ToBoolean(currentCell.Value);

                    // Nếu ô đang trống (false) thì hủy edit
                    if (!currentValue)
                    {
                        e.Cancel = true; // Quan trọng: hủy sự kiện edit
                        MessageBox.Show("Chỉ có thể bỏ tích quyền đã được cấp!", "Thông báo",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        // 🎯 PHƯƠNG THỨC MỚI: KHỞI TẠO DATAGRIDVIEW VỚI CÁC CỘT CHECKBOX
        private void InitializePermissionsGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AllowUserToAddRows = false;

            // Thêm cột TableName
            DataGridViewTextBoxColumn colTableName = new DataGridViewTextBoxColumn();
            colTableName.Name = "TableName";
            colTableName.HeaderText = "Tên bảng";
            colTableName.Width = 150;
            dataGridView1.Columns.Add(colTableName);

            // Thêm các cột CheckBox cho permissions
            AddCheckBoxColumn("HasSelect", "SELECT");
            AddCheckBoxColumn("HasInsert", "INSERT");
            AddCheckBoxColumn("HasUpdate", "UPDATE");
            AddCheckBoxColumn("HasDelete", "DELETE");

            // Căn giữa header
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (column.Name != "TableName")
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        // 🎯 PHƯƠNG THỨC MỚI: THÊM CỘT CHECKBOX
        private void AddCheckBoxColumn(string name, string headerText)
        {
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = name;
            checkBoxColumn.HeaderText = headerText;
            checkBoxColumn.Width = 70;
            checkBoxColumn.TrueValue = 1;
            checkBoxColumn.FalseValue = 0;
            checkBoxColumn.IndeterminateValue = 0;

            dataGridView1.Columns.Add(checkBoxColumn);
        }

        // 1. LOAD DANH SÁCH USER LÊN COMBOBOX
        private void LoadUserList()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("sp_GetUsersForComboBox", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dr = cmd.ExecuteReader();

                    cboUser.Items.Clear();
                    cboUser.Items.Add("-- Chọn user --");

                    int userCount = 0;
                    while (dr.Read())
                    {
                        string userName = dr["UserName"].ToString();
                        cboUser.Items.Add(userName);
                        userCount++;
                    }
                    dr.Close();

                    
                    cboUser.SelectedIndex = 0;
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Lỗi SQL: {sqlEx.Message}\nNumber: {sqlEx.Number}", "Lỗi SQL");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tổng quát: {ex.Message}", "Lỗi");
            }
        }
       
        
        /// </summary>

        // 2. LOAD DANH SÁCH TABLE LÊN COMBOBOX
        private void LoadTableList()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetTablesForComboBox", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dr = cmd.ExecuteReader();

                    cboTable.Items.Clear();
                    cboTable.Items.Add("-- Chọn bảng --");

                    while (dr.Read())
                    {
                        cboTable.Items.Add(dr["TableName"].ToString());
                    }
                    dr.Close();
                    cboTable.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải bảng: " + ex.Message, "Lỗi");
            }
        }

        // 3. SỰ KIỆN CLICK COMBOBOX - MỞ DROPDOWN
        private void cboUser_Click(object sender, EventArgs e)
        {
            if (cboUser.DroppedDown == false)
            {
                cboUser.DroppedDown = true;
            }
        }

        private void cboTable_Click(object sender, EventArgs e)
        {
            if (cboTable.DroppedDown == false)
            {
                cboTable.DroppedDown = true;
            }
        }

        // 4. SỰ KIỆN ENTER - MỞ DROPDOWN KHI FOCUS
        private void cboUser_Enter(object sender, EventArgs e)
        {
            cboUser.DroppedDown = true;
        }

        private void cboTable_Enter(object sender, EventArgs e)
        {
            cboTable.DroppedDown = true;
        }

        // 5. KHI CHỌN USER - LOAD QUYỀN LÊN DATAGRIDVIEW
        private void cboUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboUser.SelectedIndex > 0)
            {
                string selectedUser = cboUser.Text;
                LoadUserPermissions(selectedUser);
            }
        }

        // 6. LOAD QUYỀN USER LÊN DATAGRIDVIEW - PHIÊN BẢN ĐÃ SỬA
        // 6. LOAD QUYỀN USER LÊN DATAGRIDVIEW - PHIÊN BẢN ĐÃ SỬA
        private void LoadUserPermissions(string userName)
        {
            try
            {
               

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    

                    SqlCommand cmd = new SqlCommand("sp_GetUserPermissions", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", userName);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                   

                    currentPermissions = dt;

                    // 🎯 QUAN TRỌNG: Xóa dữ liệu cũ và thêm dữ liệu mới thủ công
                    dataGridView1.Rows.Clear();

                    foreach (DataRow row in dt.Rows)
                    {
                        // 🚫 LỌC BỎ BẢNG SESSION_GLOBAL
                        string tableName = row["TableName"].ToString();
                        if (tableName == "SESSION_GLOBAL")
                            continue;

                        int rowIndex = dataGridView1.Rows.Add();
                        dataGridView1.Rows[rowIndex].Cells["TableName"].Value = row["TableName"];
                        dataGridView1.Rows[rowIndex].Cells["HasSelect"].Value = Convert.ToBoolean(row["HasSelect"]);
                        dataGridView1.Rows[rowIndex].Cells["HasInsert"].Value = Convert.ToBoolean(row["HasInsert"]);
                        dataGridView1.Rows[rowIndex].Cells["HasUpdate"].Value = Convert.ToBoolean(row["HasUpdate"]);
                        dataGridView1.Rows[rowIndex].Cells["HasDelete"].Value = Convert.ToBoolean(row["HasDelete"]);
                    }

                    // 🎯 TẮT CHẾ ĐỘ TỰ ĐỘNG THÊM DÒNG MỚI
                    dataGridView1.AllowUserToAddRows = false;

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi khi tải quyền: {ex.Message}", "Lỗi");
            }
        }

        // 7. ĐỊNH DẠNG DATAGRIDVIEW QUYỀN - PHIÊN BẢN ĐÃ SỬA (đơn giản hóa)
        private void FormatPermissionsGridView()
        {
            // Đã được xử lý trong InitializePermissionsGridView()
        }

        // 8. NÚT PHÂN QUYỀN ĐƠN LẺ - CHỈ CẤP QUYỀN ĐÃ CHỌN
        private void btnPhanQuyenDonLe_Click(object sender, EventArgs e)
        {
            if (cboUser.SelectedIndex == 0 || cboTable.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn user và bảng!", "Thông báo");
                return;
            }

            string userName = cboUser.Text;
            string tableName = cboTable.Text;
            string permissionType = GetSelectedPermission();

            if (string.IsNullOrEmpty(permissionType))
            {
                MessageBox.Show("Vui lòng chọn quyền!", "Thông báo");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Gọi stored procedure phân quyền
                    SqlCommand cmd = new SqlCommand("sp_GrantPermission", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@TableName", tableName);
                    cmd.Parameters.AddWithValue("@PermissionType", permissionType);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"✅ Đã cấp quyền {permissionType} trên bảng {tableName} cho user {userName}", "Thành công");

                    // Load lại datagridview để cập nhật quyền mới
                    LoadUserPermissions(userName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi phân quyền: " + ex.Message, "Lỗi");
            }
        }

        // 8.1 NÚT CẬP NHẬT - LOAD LẠI QUYỀN LÊN DATAGRIDVIEW
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (cboUser.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn user trước!", "Thông báo");
                return;
            }

            string userName = cboUser.Text;

            try
            {
                // Load lại quyền từ database lên datagridview
                LoadUserPermissions(userName);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi cập nhật: " + ex.Message, "Lỗi");
            }
        }

        // 9. NÚT THU HỒI QUYỀN ĐÃ CHỌN - PHIÊN BẢN ĐÃ SỬA
        private void btnThuHoiQuyen_Click(object sender, EventArgs e)
        {
            if (cboUser.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn user!", "Thông báo");
                return;
            }

            string userName = cboUser.Text;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string tableName = row.Cells["TableName"].Value.ToString();

                        // 🎯 SỬA: Kiểm tra giá trị checkbox hiện tại
                        bool currentSelect = row.Cells["HasSelect"].Value != null ? Convert.ToBoolean(row.Cells["HasSelect"].Value) : false;
                        bool currentInsert = row.Cells["HasInsert"].Value != null ? Convert.ToBoolean(row.Cells["HasInsert"].Value) : false;
                        bool currentUpdate = row.Cells["HasUpdate"].Value != null ? Convert.ToBoolean(row.Cells["HasUpdate"].Value) : false;
                        bool currentDelete = row.Cells["HasDelete"].Value != null ? Convert.ToBoolean(row.Cells["HasDelete"].Value) : false;

                        // Thu hồi quyền nếu checkbox không được chọn
                        if (!currentSelect) RevokePermission(conn, userName, tableName, "SELECT");
                        if (!currentInsert) RevokePermission(conn, userName, tableName, "INSERT");
                        if (!currentUpdate) RevokePermission(conn, userName, tableName, "UPDATE");
                        if (!currentDelete) RevokePermission(conn, userName, tableName, "DELETE");
                    }

                    MessageBox.Show("Đã thu hồi các quyền đã chọn!", "Thành công");
                    LoadUserPermissions(userName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thu hồi quyền: " + ex.Message, "Lỗi");
            }
        }

        // 10. HÀM THU HỒI QUYỀN
        private void RevokePermission(SqlConnection conn, string userName, string tableName, string permissionType)
        {
            try
            {
                string sql = $"REVOKE {permissionType} ON {tableName} FROM [{userName}]";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // Bỏ qua lỗi nếu quyền không tồn tại
            }
        }

        // 11. LẤY QUYỀN ĐƯỢC CHỌN TỪ CHECKBOX
        private string GetSelectedPermission()
        {
            if (chkSelect.Checked) return "SELECT";
            if (chkInsert.Checked) return "INSERT";
            if (chkUpdate.Checked) return "UPDATE";
            if (chkDelete.Checked) return "DELETE";
            return "";
        }

        // 12. XỬ LÝ CHECKBOX CHỈ CHỌN 1 QUYỀN VÀ HIỆN THÔNG BÁO
        private void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            HandleSingleCheckbox(sender);
        }

        private void chkInsert_CheckedChanged(object sender, EventArgs e)
        {
            HandleSingleCheckbox(sender);
        }

        private void chkUpdate_CheckedChanged(object sender, EventArgs e)
        {
            HandleSingleCheckbox(sender);
        }

        private void chkDelete_CheckedChanged(object sender, EventArgs e)
        {
            HandleSingleCheckbox(sender);
        }

        private void HandleSingleCheckbox(object sender)
        {
            CheckBox currentCheckbox = (CheckBox)sender;
            CheckBox[] checkboxes = { chkSelect, chkInsert, chkUpdate, chkDelete };

            // Nếu checkbox hiện tại được chọn
            if (currentCheckbox.Checked)
            {
                // Kiểm tra xem có checkbox nào khác đang được chọn không
                foreach (CheckBox chk in checkboxes)
                {
                    if (chk != currentCheckbox && chk.Checked)
                    {
                        // Nếu có checkbox khác đang chọn, hiển thị thông báo và bỏ chọn cái hiện tại
                        MessageBox.Show("Chỉ được chọn 1 quyền duy nhất!", "Thông báo",
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        currentCheckbox.Checked = false;
                        return; // Thoát ngay sau khi hiển thị thông báo
                    }
                }

                // Nếu không có checkbox nào khác chọn, đảm bảo chỉ có 1 checkbox được chọn
                foreach (CheckBox chk in checkboxes)
                {
                    if (chk != currentCheckbox)
                    {
                        chk.Checked = false;
                    }
                }
            }
        }
        // CÁC SỰ KIỆN KHÁC
        // THÊM SỰ KIỆN NÀY VÀO FORM
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Chỉ xử lý khi click vào các cột checkbox và không phải header
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

                // Chỉ xử lý các cột permission (checkbox)
                if (columnName == "HasSelect" || columnName == "HasInsert" ||
                    columnName == "HasUpdate" || columnName == "HasDelete")
                {
                    // Lấy giá trị hiện tại của ô
                    var currentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    bool currentValue = currentCell.Value != null && Convert.ToBoolean(currentCell.Value);

                    // Nếu ô đang trống (false) thì không cho click
                    if (!currentValue)
                    {
                        // Hủy sự kiện click
                        dataGridView1.CancelEdit();

                        // Hiển thị thông báo
                        MessageBox.Show("Chỉ có thể bỏ tích quyền đã được cấp!", "Thông báo",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        private void button3_Click(object sender, EventArgs e) { }
        private void cboUser_SelectedIndexChanged_1(object sender, EventArgs e) { }

        private void cboTable_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
            // Form QuanLy sẽ hiện lại nếu đang ẩn
        }
    }
}