namespace Form_QL_ShopQuanAo
{
    partial class QuanLy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NhapLieu = new System.Windows.Forms.Button();
            this.XuatDuLieu = new System.Windows.Forms.Button();
            this.ThongKe = new System.Windows.Forms.Button();
            this.DangXuat = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Import = new System.Windows.Forms.Button();
            this.ImportAll = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ChiTietHoaDon = new System.Windows.Forms.RadioButton();
            this.LietKeSanPham = new System.Windows.Forms.RadioButton();
            this.TongLuongThucLanh = new System.Windows.Forms.RadioButton();
            this.LuongThucLanh = new System.Windows.Forms.RadioButton();
            this.ThemNhanVien = new System.Windows.Forms.RadioButton();
            this.button2 = new System.Windows.Forms.Button();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtTopN = new System.Windows.Forms.TextBox();
            this.lblTopN = new System.Windows.Forms.Label();
            this.KhachHangGanDay = new System.Windows.Forms.RadioButton();
            this.button3 = new System.Windows.Forms.Button();
            this.SoLuongSPConLai = new System.Windows.Forms.RadioButton();
            this.TongSoNhanVien = new System.Windows.Forms.RadioButton();
            this.SanPhamBanChay = new System.Windows.Forms.RadioButton();
            this.Xoa = new System.Windows.Forms.Button();
            this.Sua = new System.Windows.Forms.Button();
            this.btnUser = new System.Windows.Forms.Button();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // NhapLieu
            // 
            this.NhapLieu.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NhapLieu.Location = new System.Drawing.Point(29, 568);
            this.NhapLieu.Name = "NhapLieu";
            this.NhapLieu.Size = new System.Drawing.Size(121, 31);
            this.NhapLieu.TabIndex = 0;
            this.NhapLieu.Text = "Nhập liệu";
            this.NhapLieu.UseVisualStyleBackColor = true;
            this.NhapLieu.Click += new System.EventHandler(this.button1_Click);
            // 
            // XuatDuLieu
            // 
            this.XuatDuLieu.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XuatDuLieu.Location = new System.Drawing.Point(873, 567);
            this.XuatDuLieu.Name = "XuatDuLieu";
            this.XuatDuLieu.Size = new System.Drawing.Size(121, 34);
            this.XuatDuLieu.TabIndex = 1;
            this.XuatDuLieu.Text = "Xuất dữ liệu";
            this.XuatDuLieu.UseVisualStyleBackColor = true;
            this.XuatDuLieu.Click += new System.EventHandler(this.button2_Click);
            // 
            // ThongKe
            // 
            this.ThongKe.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThongKe.Location = new System.Drawing.Point(737, 567);
            this.ThongKe.Name = "ThongKe";
            this.ThongKe.Size = new System.Drawing.Size(121, 34);
            this.ThongKe.TabIndex = 2;
            this.ThongKe.Text = "Thống kê";
            this.ThongKe.UseVisualStyleBackColor = true;
            this.ThongKe.Click += new System.EventHandler(this.ThongKe_Click);
            // 
            // DangXuat
            // 
            this.DangXuat.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DangXuat.ForeColor = System.Drawing.Color.Red;
            this.DangXuat.Location = new System.Drawing.Point(1179, 601);
            this.DangXuat.Name = "DangXuat";
            this.DangXuat.Size = new System.Drawing.Size(104, 33);
            this.DangXuat.TabIndex = 3;
            this.DangXuat.Text = "Đăng xuất";
            this.DangXuat.UseVisualStyleBackColor = true;
            this.DangXuat.Click += new System.EventHandler(this.button4_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(41, 319);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(939, 240);
            this.dataGridView1.TabIndex = 4;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(156, 567);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(170, 24);
            this.comboBox1.TabIndex = 5;
            // 
            // Import
            // 
            this.Import.Location = new System.Drawing.Point(156, 601);
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(64, 23);
            this.Import.TabIndex = 6;
            this.Import.Text = "Import";
            this.Import.UseVisualStyleBackColor = true;
            this.Import.Click += new System.EventHandler(this.button5_Click);
            // 
            // ImportAll
            // 
            this.ImportAll.Location = new System.Drawing.Point(246, 601);
            this.ImportAll.Name = "ImportAll";
            this.ImportAll.Size = new System.Drawing.Size(80, 23);
            this.ImportAll.TabIndex = 7;
            this.ImportAll.Text = "Import all";
            this.ImportAll.UseVisualStyleBackColor = true;
            this.ImportAll.Click += new System.EventHandler(this.button6_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(22, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(276, 42);
            this.label1.TabIndex = 8;
            this.label1.Text = "Tìm khách hàng";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(268, 63);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(112, 22);
            this.textBox1.TabIndex = 9;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Nhập mã khách hàng :";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(386, 63);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Tìm ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.groupBox1.Controls.Add(this.ChiTietHoaDon);
            this.groupBox1.Controls.Add(this.LietKeSanPham);
            this.groupBox1.Controls.Add(this.TongLuongThucLanh);
            this.groupBox1.Controls.Add(this.LuongThucLanh);
            this.groupBox1.Controls.Add(this.ThemNhanVien);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox1.Location = new System.Drawing.Point(1000, 273);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(283, 318);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hàm thủ tục";
            // 
            // ChiTietHoaDon
            // 
            this.ChiTietHoaDon.AutoSize = true;
            this.ChiTietHoaDon.Location = new System.Drawing.Point(6, 217);
            this.ChiTietHoaDon.Name = "ChiTietHoaDon";
            this.ChiTietHoaDon.Size = new System.Drawing.Size(184, 22);
            this.ChiTietHoaDon.TabIndex = 18;
            this.ChiTietHoaDon.TabStop = true;
            this.ChiTietHoaDon.Text = "Xem chi tiết hóa đơn";
            this.ChiTietHoaDon.UseVisualStyleBackColor = true;
            // 
            // LietKeSanPham
            // 
            this.LietKeSanPham.AutoSize = true;
            this.LietKeSanPham.Location = new System.Drawing.Point(6, 189);
            this.LietKeSanPham.Name = "LietKeSanPham";
            this.LietKeSanPham.Size = new System.Drawing.Size(157, 22);
            this.LietKeSanPham.TabIndex = 17;
            this.LietKeSanPham.TabStop = true;
            this.LietKeSanPham.Text = "Liệt kê sản phẩm";
            this.LietKeSanPham.UseVisualStyleBackColor = true;
            // 
            // TongLuongThucLanh
            // 
            this.TongLuongThucLanh.AutoSize = true;
            this.TongLuongThucLanh.Location = new System.Drawing.Point(6, 105);
            this.TongLuongThucLanh.Name = "TongLuongThucLanh";
            this.TongLuongThucLanh.Size = new System.Drawing.Size(186, 22);
            this.TongLuongThucLanh.TabIndex = 16;
            this.TongLuongThucLanh.TabStop = true;
            this.TongLuongThucLanh.Text = "Tổng lương thực lãnh";
            this.TongLuongThucLanh.UseVisualStyleBackColor = true;
            // 
            // LuongThucLanh
            // 
            this.LuongThucLanh.AutoSize = true;
            this.LuongThucLanh.Location = new System.Drawing.Point(6, 77);
            this.LuongThucLanh.Name = "LuongThucLanh";
            this.LuongThucLanh.Size = new System.Drawing.Size(182, 22);
            this.LuongThucLanh.TabIndex = 15;
            this.LuongThucLanh.TabStop = true;
            this.LuongThucLanh.Text = "Xem lương thực lãnh";
            this.LuongThucLanh.UseVisualStyleBackColor = true;
            // 
            // ThemNhanVien
            // 
            this.ThemNhanVien.AutoSize = true;
            this.ThemNhanVien.Location = new System.Drawing.Point(6, 49);
            this.ThemNhanVien.Name = "ThemNhanVien";
            this.ThemNhanVien.Size = new System.Drawing.Size(147, 22);
            this.ThemNhanVien.TabIndex = 14;
            this.ThemNhanVien.TabStop = true;
            this.ThemNhanVien.Text = "Thêm nhân viên";
            this.ThemNhanVien.UseVisualStyleBackColor = true;
            this.ThemNhanVien.CheckedChanged += new System.EventHandler(this.ThemNhanVien_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(164, 269);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 34);
            this.button2.TabIndex = 13;
            this.button2.Text = "Thực thi";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(6, 133);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(149, 22);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Thêm sản phẩm";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.SanPhamBanChay_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 23);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(155, 22);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Liệt kê nhân viên";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 161);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(162, 22);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Xem hóa đơn mới";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.LightSkyBlue;
            this.groupBox2.Controls.Add(this.txtTopN);
            this.groupBox2.Controls.Add(this.lblTopN);
            this.groupBox2.Controls.Add(this.KhachHangGanDay);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.SoLuongSPConLai);
            this.groupBox2.Controls.Add(this.TongSoNhanVien);
            this.groupBox2.Controls.Add(this.SanPhamBanChay);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox2.Location = new System.Drawing.Point(1000, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(283, 239);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Hàm Function";
            // 
            // txtTopN
            // 
            this.txtTopN.Location = new System.Drawing.Point(197, 153);
            this.txtTopN.Name = "txtTopN";
            this.txtTopN.Size = new System.Drawing.Size(70, 24);
            this.txtTopN.TabIndex = 16;
            this.txtTopN.TextChanged += new System.EventHandler(this.txtTopN_TextChanged);
            // 
            // lblTopN
            // 
            this.lblTopN.AutoSize = true;
            this.lblTopN.ForeColor = System.Drawing.Color.Red;
            this.lblTopN.Location = new System.Drawing.Point(3, 149);
            this.lblTopN.Name = "lblTopN";
            this.lblTopN.Size = new System.Drawing.Size(0, 18);
            this.lblTopN.TabIndex = 15;
            // 
            // KhachHangGanDay
            // 
            this.KhachHangGanDay.AutoSize = true;
            this.KhachHangGanDay.Location = new System.Drawing.Point(6, 115);
            this.KhachHangGanDay.Name = "KhachHangGanDay";
            this.KhachHangGanDay.Size = new System.Drawing.Size(180, 22);
            this.KhachHangGanDay.TabIndex = 14;
            this.KhachHangGanDay.TabStop = true;
            this.KhachHangGanDay.Text = "Khách hàng gần đây";
            this.KhachHangGanDay.UseVisualStyleBackColor = true;
            this.KhachHangGanDay.CheckedChanged += new System.EventHandler(this.KhachHangGanDay_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(164, 194);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(113, 34);
            this.button3.TabIndex = 13;
            this.button3.Text = "Thực thi";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // SoLuongSPConLai
            // 
            this.SoLuongSPConLai.AutoSize = true;
            this.SoLuongSPConLai.Location = new System.Drawing.Point(6, 31);
            this.SoLuongSPConLai.Name = "SoLuongSPConLai";
            this.SoLuongSPConLai.Size = new System.Drawing.Size(179, 22);
            this.SoLuongSPConLai.TabIndex = 2;
            this.SoLuongSPConLai.TabStop = true;
            this.SoLuongSPConLai.Text = "Số lượng sản phẩm ";
            this.SoLuongSPConLai.UseVisualStyleBackColor = true;
            this.SoLuongSPConLai.CheckedChanged += new System.EventHandler(this.SoLuongSPConLai_CheckedChanged);
            // 
            // TongSoNhanVien
            // 
            this.TongSoNhanVien.AutoSize = true;
            this.TongSoNhanVien.Location = new System.Drawing.Point(6, 59);
            this.TongSoNhanVien.Name = "TongSoNhanVien";
            this.TongSoNhanVien.Size = new System.Drawing.Size(213, 22);
            this.TongSoNhanVien.TabIndex = 1;
            this.TongSoNhanVien.TabStop = true;
            this.TongSoNhanVien.Text = "Tổng số lượng nhân viên";
            this.TongSoNhanVien.UseVisualStyleBackColor = true;
            this.TongSoNhanVien.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // SanPhamBanChay
            // 
            this.SanPhamBanChay.AutoSize = true;
            this.SanPhamBanChay.Location = new System.Drawing.Point(6, 87);
            this.SanPhamBanChay.Name = "SanPhamBanChay";
            this.SanPhamBanChay.Size = new System.Drawing.Size(176, 22);
            this.SanPhamBanChay.TabIndex = 0;
            this.SanPhamBanChay.TabStop = true;
            this.SanPhamBanChay.Text = "Sản phẩm bán chạy";
            this.SanPhamBanChay.UseVisualStyleBackColor = true;
            this.SanPhamBanChay.CheckedChanged += new System.EventHandler(this.SanPhamBanChay_CheckedChanged);
            // 
            // Xoa
            // 
            this.Xoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Xoa.Location = new System.Drawing.Point(449, 565);
            this.Xoa.Name = "Xoa";
            this.Xoa.Size = new System.Drawing.Size(83, 34);
            this.Xoa.TabIndex = 15;
            this.Xoa.Text = "Xóa ";
            this.Xoa.UseVisualStyleBackColor = true;
            this.Xoa.Click += new System.EventHandler(this.Xoa_Click);
            // 
            // Sua
            // 
            this.Sua.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sua.Location = new System.Drawing.Point(549, 567);
            this.Sua.Name = "Sua";
            this.Sua.Size = new System.Drawing.Size(85, 32);
            this.Sua.TabIndex = 16;
            this.Sua.Text = "Sửa";
            this.Sua.UseVisualStyleBackColor = true;
            this.Sua.Click += new System.EventHandler(this.Sua_Click);
            // 
            // btnUser
            // 
            this.btnUser.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUser.Location = new System.Drawing.Point(783, 164);
            this.btnUser.Name = "btnUser";
            this.btnUser.Size = new System.Drawing.Size(150, 31);
            this.btnUser.TabIndex = 18;
            this.btnUser.Text = "Quản lí User";
            this.btnUser.UseVisualStyleBackColor = true;
            this.btnUser.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // cmbRole
            // 
            this.cmbRole.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Location = new System.Drawing.Point(783, 61);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(150, 31);
            this.cmbRole.TabIndex = 19;
            this.cmbRole.Text = "Quản lí quyền";
            this.cmbRole.SelectedIndexChanged += new System.EventHandler(this.cmbRole_SelectedIndexChanged);
            // 
            // QuanLy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(1295, 637);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.btnUser);
            this.Controls.Add(this.Sua);
            this.Controls.Add(this.Xoa);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ThongKe);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ImportAll);
            this.Controls.Add(this.Import);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.DangXuat);
            this.Controls.Add(this.XuatDuLieu);
            this.Controls.Add(this.NhapLieu);
            this.Name = "QuanLy";
            this.Text = "Quản lí ";
            this.Load += new System.EventHandler(this.QuanLy_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button NhapLieu;
        private System.Windows.Forms.Button XuatDuLieu;
        private System.Windows.Forms.Button ThongKe;
        private System.Windows.Forms.Button DangXuat;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button Import;
        private System.Windows.Forms.Button ImportAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RadioButton SoLuongSPConLai;
        private System.Windows.Forms.RadioButton TongSoNhanVien;
        private System.Windows.Forms.RadioButton SanPhamBanChay;
        private System.Windows.Forms.RadioButton KhachHangGanDay;
        private System.Windows.Forms.Label lblTopN;
        private System.Windows.Forms.TextBox txtTopN;
        private System.Windows.Forms.Button Xoa;
        private System.Windows.Forms.Button Sua;
        private System.Windows.Forms.RadioButton ThemNhanVien;
        private System.Windows.Forms.RadioButton LuongThucLanh;
        private System.Windows.Forms.RadioButton TongLuongThucLanh;
        private System.Windows.Forms.RadioButton LietKeSanPham;
        private System.Windows.Forms.RadioButton ChiTietHoaDon;
        private System.Windows.Forms.Button btnUser;
        private System.Windows.Forms.ComboBox cmbRole;
    }
}