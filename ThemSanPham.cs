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
    public partial class ThemSanPham : Form
    {
        public string MaSP => textBox1.Text.Trim();
        public string TenSP => textBox2.Text.Trim();
        public string Loai => textBox3.Text.Trim();
        public string Size => textBox4.Text.Trim();
        public decimal Gia => decimal.Parse(textBox5.Text.Trim());
        public int SoLuong => int.Parse(textBox6.Text.Trim());
        public ThemSanPham()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
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
