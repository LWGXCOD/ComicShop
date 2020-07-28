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

namespace ComicsShop
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        //закрытие приложения.
        private void exitLabel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //перемещение окна за верхнюю панель.
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
        //перекрашивание крестика.
        private void exitLabel_MouseEnter(object sender, EventArgs e)
        {
            exitLabel.ForeColor = Color.FromArgb(80, 80, 80);
        }

        private void exitLabel_MouseLeave(object sender, EventArgs e)
        {
            exitLabel.ForeColor = Color.FromArgb(0, 0, 0);
        }
        //подсказка на кнопку личный кабинет.
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(pictureBox1, "Личный кабинет");
        }
        //кнопка личный кабинет.
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if ((UserData.Login == null) && (UserData.Password == null))
            {
                //если пользователь не авторизован загружается окно авторизации.
                LogInForm newForm = new LogInForm();
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
            }
            else 
            {
                //если пользователь вошел в свой аккаунт открывается форма личный кабинет.
                CabinetForm newForm = new CabinetForm();
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
            }
        }
        //подсказка на кнопке корзина.
        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(pictureBox2, "Корзина");
        }
        //переменные в которых будет хранится информация о трёх последних добавленных комиксах для того чтобы отображать их в поле "новинки".
        ComicClass C1 = new ComicClass(), C2 = new ComicClass(), C3 = new ComicClass();
        private void MainForm_Load(object sender, EventArgs e)
        {
            //проверка на администраторский уровень доступа.
            if (UserData.Access == true)
            {
                //отображение кнопки просмотра списка заказов если пользователь администратор.
                pictureBox4.Visible = true;
            }

            if (UserData.Login != null)
            {
                //отображение кнопки корзина если пользователь авторизован.
                pictureBox2.Visible = true;
            }

            
            //получение из таблицы comics информации о последних добавленных 3х комиксах
            UsersDB db = new UsersDB();

            DataTable comicTable = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            
            MySqlCommand command = new MySqlCommand("SELECT * FROM `comics` ORDER BY `id_c` DESC", db.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(comicTable);
            //занесение этой информации в С1 С2 и С3 и отображение названия и картинки новинок.
            if (comicTable.Rows.Count > 0)
            {
                C1.ID = int.Parse(comicTable.Rows[0][0].ToString());
                C1.PicURL = comicTable.Rows[0][1].ToString();
                C1.name_comic = comicTable.Rows[0][2].ToString();
                C1.description = comicTable.Rows[0][3].ToString();
                C1.price = int.Parse(comicTable.Rows[0][4].ToString());
                ComicPic1.Load(C1.PicURL);
                ComicName1.Text = C1.name_comic;
                if (comicTable.Rows.Count > 1)
                {
                    C2.ID = int.Parse(comicTable.Rows[1][0].ToString());
                    C2.PicURL = comicTable.Rows[1][1].ToString();
                    C2.name_comic = comicTable.Rows[1][2].ToString();
                    C2.description = comicTable.Rows[1][3].ToString();
                    C2.price = int.Parse(comicTable.Rows[1][4].ToString());
                    ComicPic2.Load(C2.PicURL);
                    ComicName2.Text = C2.name_comic;
                    if (comicTable.Rows.Count > 2)
                    {
                        C3.ID = int.Parse(comicTable.Rows[2][0].ToString());
                        C3.PicURL = comicTable.Rows[2][1].ToString();
                        C3.name_comic = comicTable.Rows[2][2].ToString();
                        C3.description = comicTable.Rows[2][3].ToString();
                        C3.price = int.Parse(comicTable.Rows[2][4].ToString());
                        ComicPic3.Load(C3.PicURL);
                        ComicName3.Text = C3.name_comic;
                    }
                }
            }
            else
            {
                //сообщение о отсутствии комиксов в каталоге.
                MessageBox.Show("No comics");
            }


        }
        //кнопка найти открывает форму поиска передавая в неё строку записаную в textBox1.Text .
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            SearchForm newForm = new SearchForm(textBox1.Text);
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //кнопка корзины.
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Forms.BasketForm newForm = new Forms.BasketForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //открывает форму просмотра товара по клику на его название.
        private void ComicName2_Click(object sender, EventArgs e)
        {
            Forms.PVForm newForm = new Forms.PVForm(C2.ID);
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();

        }
        //открывает форму просмотра товара по клику на его название.
        private void ComicName3_Click(object sender, EventArgs e)
        {
            Forms.PVForm newForm = new Forms.PVForm(C3.ID);
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //открывает форму просмотра товара по клику на его изображение.
        private void ComicPic1_Click(object sender, EventArgs e)
        {
            Forms.PVForm newForm = new Forms.PVForm(C1.ID);
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //открывает форму просмотра товара по клику на его изображение.
        private void ComicPic2_Click(object sender, EventArgs e)
        {
            Forms.PVForm newForm = new Forms.PVForm(C2.ID);
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //открывает форму просмотра товара по клику на его изображение.
        private void ComicPic3_Click(object sender, EventArgs e)
        {
            Forms.PVForm newForm = new Forms.PVForm(C3.ID);
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //кнопка лист заказов открывает доступную администратору форму списка активных заказов.
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Forms.OrderListForm newForm = new Forms.OrderListForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //открывает форму просмотра товара по клику на его название.
        private void ComicName1_Click(object sender, EventArgs e)
        {
            Forms.PVForm newForm = new Forms.PVForm(C1.ID);
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
    }
}
