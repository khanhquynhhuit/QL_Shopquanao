using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;

namespace Form_QL_ShopQuanAo
{

    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Mở form chào mừng đầu tiên
            Application.Run(new FormWelcome());
        }
    }
}
