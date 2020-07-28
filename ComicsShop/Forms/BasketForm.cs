using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComicsShop.Forms
{
    public partial class BasketForm : Form
    {
        public BasketForm()
        {
            InitializeComponent();
        }
        //закрытие окна.
        private void exitLabel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //Перекрашиваение крестика.
        private void exitLabel_MouseEnter(object sender, EventArgs e)
        {
            exitLabel.ForeColor = Color.FromArgb(80, 80, 80);
        }

        private void exitLabel_MouseLeave(object sender, EventArgs e)
        {
            exitLabel.ForeColor = Color.FromArgb(0, 0, 0);
        }
        //Перемещение окна за верхнюю панель.
        Point lastPoint;
        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }

        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        //кнопка Личный кабинет загружает форму личного кабинета если пользователь авторизован, в ином случае загружает форму авторизации.
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if ((UserData.Login == null) && (UserData.Password == null))
            {
                LogInForm newForm = new LogInForm();
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
            }
            else
            {
                CabinetForm newForm = new CabinetForm();
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
            }
        }
        //кнопка домой, загружает форму главного экрана.
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MainForm newForm = new MainForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //переменные: количество страниц корзины, текущая страница, общая стоимость товаров корзины, таблица товаров находящихся в корзине, массив ComicClass в котором будут содержаться данные о товарах корзины, массив ProductPanelClass в котором будут содержаться элементы формы отвечающие за параметры товара, для отображения их в форму.
        int countOfPages;
        int currentPage = 1;
        int sum = 0;
        DataTable comicTable = new DataTable();
        ComicClass[] C;
        ProductPanelClass[] P = new ProductPanelClass[4];
        //функция отображения данных из элемента ComicClass в элементы формы заданные в элементе ProductPanelClass.
        void ShowPan(ProductPanelClass Pi, ComicClass Ci)
        {


            Pi.pic.Load(Ci.PicURL);
            Pi.name.Text = Ci.name_comic;
            Pi.price.Text = Ci.price.ToString() + '$';
            Pi.count.Text = Ci.count.ToString();
            Pi.pan.Visible = true;
        }
        //Отображение корзины при загрузке формы
        private void BasketForm_Load(object sender, EventArgs e)
        {
            UsersDB db = new UsersDB();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            
            //вычисление количества страниц в корзине.
            if (UserData.Bascket.CountProducts % 4 == 0)
                countOfPages = (UserData.Bascket.CountProducts / 4);
            else
                countOfPages = ((UserData.Bascket.CountProducts / 4) + 1);

            C = new ComicClass[UserData.Bascket.CountProducts];

            Pages.Text = currentPage.ToString();
            //заполнение P элементами формы BasketForm для последующего отображения в них информации о соответствующих товарах.
            P[0] = new ProductPanelClass();
            P[0].pan = Product1Pan;
            P[0].name = Product1Name;
            P[0].price = Product1Price;
            P[0].count = Product1count;
            P[0].pic = Product1Pic;

            P[1] = new ProductPanelClass();
            P[1].pan = Product2Pan;
            P[1].name = Product2Name;
            P[1].price = Product2Price;
            P[1].count = Product2count;
            P[1].pic = Product2Pic;

            P[2] = new ProductPanelClass();
            P[2].pan = Product3Pan;
            P[2].name = Product3Name;
            P[2].price = Product3Price;
            P[2].count = Product3count;
            P[2].pic = Product3Pic;

            P[3] = new ProductPanelClass();
            P[3].pan = Product4Pan;
            P[3].name = Product4Name;
            P[3].price = Product4Price;
            P[3].count = Product4count;
            P[3].pic = Product4Pic;

            int i = 0;
            //заполнение С значениями из таблицы comics на основе ID имеющихся в корзине-словариуке пользователя UserData.Basket.OrderIdCount .
            foreach (KeyValuePair<int, int> kvp in UserData.Bascket.OrderIdCount)
            {
                
                if (i < UserData.Bascket.CountProducts)
                {
                    MySqlCommand command = new MySqlCommand("SELECT * FROM `comics` WHERE `id_c` = @id", db.getConnection());
                    command.Parameters.Add("@id", MySqlDbType.VarChar).Value = kvp.Key;
                    adapter.SelectCommand = command;
                    adapter.Fill(comicTable);
                    C[i] = new ComicClass();
                    C[i].ID = int.Parse(comicTable.Rows[i][0].ToString());
                    C[i].PicURL = comicTable.Rows[i][1].ToString();
                    C[i].name_comic = comicTable.Rows[i][2].ToString();
                    C[i].description = comicTable.Rows[i][3].ToString();
                    C[i].price = int.Parse(comicTable.Rows[i][4].ToString());
                    C[i].count = kvp.Value;
                    C[i].author = comicTable.Rows[i][6].ToString();
                    C[i].genre = comicTable.Rows[i][7].ToString();
                    sum+= kvp.Value * int.Parse(comicTable.Rows[i][4].ToString());
                }
                i++;
            }
            //отображение 4х товаров корзины, соответствующих данной странице на экран.
            if (i > 0)
            {
                for (int j = 0; j < 4; j++)
                    if (j < UserData.Bascket.CountProducts)
                        ShowPan(P[j], C[j]);
            }
                sumlabel.Text = sum.ToString() + "$";
                UserData.Bascket.sum = sum;
        }
        //кнопка следующая страница "перелистывает" страницу на одну вперед и отображает 4 товара находящихся на этой странице.
        private void button2_Click(object sender, EventArgs e)
        {
            if (countOfPages > currentPage)
            {
                for (int j = 0; j < 4; j++)
                {
                    P[j].pan.Visible = false;
                }
                currentPage++;
                Pages.Text = currentPage.ToString();
                
                for (int j = 0; j < 4; j++)
                {
                    if ((j + currentPage * 4 - 4) < comicTable.Rows.Count)
                        ShowPan(P[j], C[j + currentPage * 4 - 4]);
                }
            }
        }
        //кнопка предыдущая страница "перелистывает" страницу на одну назад и отображает 4 товара находящихся на этой странице.
        private void button3_Click(object sender, EventArgs e)
        {
            if (1 < currentPage)
            {
                for (int j = 0; j < 4; j++)
                {
                    P[j].pan.Visible = false;
                }
                currentPage--;
                Pages.Text = currentPage.ToString();
                for (int j = 0; j < 4; j++)
                {
                    if ((j + currentPage * 4 - 4) < comicTable.Rows.Count)
                        ShowPan(P[j], C[j + currentPage * 4 - 4]);
                }
            }
        }
        //удаление товара отображаемого в 1й панели из корзины.
        private void Product1Delete_Click(object sender, EventArgs e)
        {
            UserData.Bascket.OrderIdCount.Remove(C[currentPage * 4 - 4].ID);
            UserData.Bascket.CountProducts--;
            Forms.BasketForm newForm = new BasketForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //удаление товара отображаемого в 2й панели из корзины.
        private void Product2Delete_Click(object sender, EventArgs e)
        {
            UserData.Bascket.OrderIdCount.Remove(C[currentPage * 4 - 3].ID);
            UserData.Bascket.CountProducts--;
            Forms.BasketForm newForm = new BasketForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //удаление товара отображаемого в 3й панели из корзины.
        private void Product3Delete_Click(object sender, EventArgs e)
        {
            UserData.Bascket.OrderIdCount.Remove(C[currentPage * 4 - 2].ID);
            UserData.Bascket.CountProducts--;
            Forms.BasketForm newForm = new BasketForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //удаление товара отображаемого в 4й панели из корзины.
        private void Product4Delete_Click(object sender, EventArgs e)
        {
            UserData.Bascket.OrderIdCount.Remove(C[currentPage * 4 - 1].ID);
            UserData.Bascket.CountProducts--;
            Forms.BasketForm newForm = new BasketForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //кнопка оформления заказа открывает форму оформления заказа.
        private void button1_Click(object sender, EventArgs e)
        {
            Forms.OrderForm newForm = new OrderForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
    }
}
