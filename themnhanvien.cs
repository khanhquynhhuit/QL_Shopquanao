using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Form_QL_ShopQuanAo
{
    public partial class ThemNhanVien : Form
    {
        public string MaNV => textBox1.Text.Trim();
        public string HoTen => textBox2.Text.Trim();
        public string SDT => textBox3.Text.Trim();
        public string GioiTinh => textBox4.Text.Trim();
        public string ChucVu => textBox5.Text.Trim();
        public DateTime NgayVaoLam => dateTimePicker1.Value;
        public decimal Luong => decimal.Parse(textBox7.Text.Trim());

        public ThemNhanVien()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MaNV) || string.IsNullOrWhiteSpace(HoTen))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
