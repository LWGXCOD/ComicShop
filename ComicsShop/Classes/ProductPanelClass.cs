using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicsShop
{
    //Класс содержащий переменные - элементы формы для удобного их заполнения и отображения на экран впоследсвии.
    class ProductPanelClass
    {
        public System.Windows.Forms.Panel pan { get; set; }
        public System.Windows.Forms.Label name { get; set; }
        public System.Windows.Forms.Label price { get; set; }
        public System.Windows.Forms.Label genre { get; set; }
        public System.Windows.Forms.PictureBox pic { get; set; }
        public System.Windows.Forms.Button button { get; set; }
        public System.Windows.Forms.Label count { get; set; }
        public System.Windows.Forms.TextBox countb { get; set; }
    }
}
