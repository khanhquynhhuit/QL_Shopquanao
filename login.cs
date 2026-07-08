using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Form_QL_ShopQuanAo
{
    public class login
    {
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string MaNV { get; set; }
        public string Quyen { get; set; }
        public login() { }
        public login(string taiKhoan, string matKhau, string manv, string quyen)
        {
            TaiKhoan = taiKhoan;
            MatKhau = matKhau;
            MaNV = manv;
            Quyen = quyen;
        }
    }
}
