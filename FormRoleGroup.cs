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

namespace Form_QL_ShopQuanAo
{
    public partial class FormRoleGroup : Form
    {
        public FormRoleGroup()
        {
            InitializeComponent();
        }
        string connStr = "Server=DESKTOP-DJ1KDIM\\SQLEXPRESS;Database=QLShopQuanAo;User Id=sa;Password=123;";

        private void FormRoleGroup_Load(object sender, EventArgs e)
        {
            LoadRolesToComboBox();
            InitializePermissionsGridView();
            cmbRole.SelectedIndexChanged += cmbRole_SelectedIndexChanged;
        }

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

        private void InitializePermissionsGridView()
        {
            dataGridView1.Columns.Clear();

            // Tắt chế độ tự động thêm dòng mới
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

            // Load danh sách bảng
            LoadTablePermissions();
        }

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

        private void LoadTablePermissions()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // SỬ DỤNG STORED PROCEDURE
                    SqlCommand cmd = new SqlCommand("sp_GetTablesForPermissions", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Thêm dữ liệu vào DataGridView
                    foreach (DataRow row in dt.Rows)
                    {
                        string tableName = row["TableName"].ToString();
                        int rowIndex = dataGridView1.Rows.Add();
                        dataGridView1.Rows[rowIndex].Cells["TableName"].Value = tableName;
                        dataGridView1.Rows[rowIndex].Cells["HasSelect"].Value = false;
                        dataGridView1.Rows[rowIndex].Cells["HasInsert"].Value = false;
                        dataGridView1.Rows[rowIndex].Cells["HasUpdate"].Value = false;
                        dataGridView1.Rows[rowIndex].Cells["HasDelete"].Value = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách bảng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadUsersInRole(string roleName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("sp_GetUsersInRole", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoleName", roleName);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    listBox1.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        listBox1.Items.Add(row["UserName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải user trong nhóm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUsersNotInRole(string roleName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("sp_GetUsersNotInRole", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoleName", roleName);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    listBox3.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        listBox3.Items.Add(row["UserName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải user ngoài nhóm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRole.SelectedIndex > 0)
            {
                string selectedRole = cmbRole.Text;
                LoadUsersInRole(selectedRole);
                LoadUsersNotInRole(selectedRole);
            }
            else
            {
                listBox1.Items.Clear();
                listBox3.Items.Clear();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cmbRole.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn nhóm quyền trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listBox3.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn user từ danh sách ngoài nhóm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string userName = listBox3.SelectedItem.ToString();
            string roleName = cmbRole.Text;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("sp_AddUserToRole", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@RoleName", roleName);
                    cmd.Parameters.Add("@ResultMessage", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    string resultMessage = cmd.Parameters["@ResultMessage"].Value.ToString();
                    MessageBox.Show(resultMessage, "Thông báo", MessageBoxButtons.OK,
                        resultMessage.Contains("Lỗi") ? MessageBoxIcon.Error : MessageBoxIcon.Information);

                    if (!resultMessage.Contains("Lỗi"))
                    {
                        // Load lại danh sách user
                        LoadUsersInRole(roleName);
                        LoadUsersNotInRole(roleName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm user vào nhóm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (cmbRole.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn nhóm quyền trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn user từ danh sách trong nhóm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string userName = listBox1.SelectedItem.ToString();
            string roleName = cmbRole.Text;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("sp_RemoveUserFromRole", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@RoleName", roleName);
                    cmd.Parameters.Add("@ResultMessage", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    string resultMessage = cmd.Parameters["@ResultMessage"].Value.ToString();
                    MessageBox.Show(resultMessage, "Thông báo", MessageBoxButtons.OK,
                        resultMessage.Contains("Lỗi") ? MessageBoxIcon.Error : MessageBoxIcon.Information);

                    if (!resultMessage.Contains("Lỗi"))
                    {
                        // Load lại danh sách user
                        LoadUsersInRole(roleName);
                        LoadUsersNotInRole(roleName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa user khỏi nhóm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string roleName = txtRoleName.Text.Trim();

            if (string.IsNullOrEmpty(roleName))
            {
                MessageBox.Show("Vui lòng nhập tên nhóm quyền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra xem có tích ít nhất 1 quyền không
            bool hasPermission = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["HasSelect"].Value != null && Convert.ToBoolean(row.Cells["HasSelect"].Value) ||
                    row.Cells["HasInsert"].Value != null && Convert.ToBoolean(row.Cells["HasInsert"].Value) ||
                    row.Cells["HasUpdate"].Value != null && Convert.ToBoolean(row.Cells["HasUpdate"].Value) ||
                    row.Cells["HasDelete"].Value != null && Convert.ToBoolean(row.Cells["HasDelete"].Value))
                {
                    hasPermission = true;
                    break;
                }
            }

            if (!hasPermission)
            {
                MessageBox.Show("Bạn chưa chọn quyền cho nhóm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Tạo nhóm quyền
                    SqlCommand cmd = new SqlCommand("sp_CreateRoleGroup", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoleName", roleName);
                    cmd.Parameters.Add("@ResultMessage", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    string resultMessage = cmd.Parameters["@ResultMessage"].Value.ToString();

                    if (resultMessage.Contains("thành công"))
                    {
                        // Cấp quyền cho nhóm
                        GrantPermissionsToRole(roleName);

                        MessageBox.Show("Tạo nhóm quyền thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Load lại combobox và reset form
                        LoadRolesToComboBox();
                        txtRoleName.Clear();
                        ResetPermissions();
                    }
                    else
                    {
                        MessageBox.Show(resultMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo nhóm quyền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GrantPermissionsToRole(string roleName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string tableName = row.Cells["TableName"].Value.ToString();

                        if (row.Cells["HasSelect"].Value != null && Convert.ToBoolean(row.Cells["HasSelect"].Value))
                        {
                            string sql = $"GRANT SELECT ON {tableName} TO [{roleName}]";
                            new SqlCommand(sql, conn).ExecuteNonQuery();
                        }

                        if (row.Cells["HasInsert"].Value != null && Convert.ToBoolean(row.Cells["HasInsert"].Value))
                        {
                            string sql = $"GRANT INSERT ON {tableName} TO [{roleName}]";
                            new SqlCommand(sql, conn).ExecuteNonQuery();
                        }

                        if (row.Cells["HasUpdate"].Value != null && Convert.ToBoolean(row.Cells["HasUpdate"].Value))
                        {
                            string sql = $"GRANT UPDATE ON {tableName} TO [{roleName}]";
                            new SqlCommand(sql, conn).ExecuteNonQuery();
                        }

                        if (row.Cells["HasDelete"].Value != null && Convert.ToBoolean(row.Cells["HasDelete"].Value))
                        {
                            string sql = $"GRANT DELETE ON {tableName} TO [{roleName}]";
                            new SqlCommand(sql, conn).ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cấp quyền cho nhóm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbRole.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn nhóm quyền cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string roleName = cmbRole.Text;

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa nhóm quyền '{roleName}' không?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("sp_DeleteRoleGroup", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoleName", roleName);
                        cmd.Parameters.Add("@ResultMessage", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        string resultMessage = cmd.Parameters["@ResultMessage"].Value.ToString();
                        MessageBox.Show(resultMessage, "Thông báo", MessageBoxButtons.OK,
                            resultMessage.Contains("Lỗi") ? MessageBoxIcon.Error : MessageBoxIcon.Information);

                        if (!resultMessage.Contains("Lỗi"))
                        {
                            // Load lại combobox
                            LoadRolesToComboBox();
                            listBox1.Items.Clear();
                            listBox3.Items.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa nhóm quyền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ResetPermissions()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["HasSelect"].Value = false;
                row.Cells["HasInsert"].Value = false;
                row.Cells["HasUpdate"].Value = false;
                row.Cells["HasDelete"].Value = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Nút khác nếu cần
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRole.SelectedIndex > 0)
            {
                string selectedRole = cmbRole.Text;

                // 🎯 GỌI HÀM LOAD USER - THÊM 2 DÒNG NÀY
                LoadUsersInRole(selectedRole);
                LoadUsersNotInRole(selectedRole);
            }
            else
            {
                listBox1.Items.Clear();
                listBox3.Items.Clear();

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}