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
    public partial class PVForm : Form
    {
        int ID;
        //конструктор принимает и запоминает ID просматриваемого товара.
        public PVForm(int IDProduct)
        {
            InitializeComponent();
            ID = IDProduct;
        }
        //закрытие приложения.
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
        //Перемещение окна.
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
        //кнопка на главную
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MainForm newForm = new MainForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //кнопка назад возвращает в форму поиска.
        private void button3_Click(object sender, EventArgs e)
        {
            SearchForm newForm = new SearchForm("");
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //переменная которая будет содержать информацию о просматриваемом комиксе.
        ComicClass C = new ComicClass();
        private void ProductViewForm_Load(object sender, EventArgs e)
        {
            if (UserData.Login != null)
            {
                //если пользователь авторизован отображать кнопки корзина и в корзину.
                pictureBox2.Visible = true;
                Product1Buy.Visible = true;
            }

            if (UserData.Access == true)
            {
                //если пользователь является администратором то отображать кнопки удалить товар, изменить товар.
                Deletebutton.Visible = true;
                Changebutton.Visible = true;
            }

            UsersDB db = new UsersDB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            //поиск товара с полученным в конструкторе ID в таблице comics.
            MySqlCommand command = new MySqlCommand("SELECT * FROM `comics` WHERE `id_c` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = ID;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                //запись данных о товаре в С.
                C.ID = ID;
                C.PicURL = table.Rows[0][1].ToString();
                C.name_comic = table.Rows[0][2].ToString();
                C.description = table.Rows[0][3].ToString();
                C.price = int.Parse(table.Rows[0][4].ToString());
                C.count = int.Parse(table.Rows[0][5].ToString());
                C.author = table.Rows[0][6].ToString();
                C.genre = table.Rows[0][7].ToString();
                C.publish = table.Rows[0][8].ToString();
                C.artist = table.Rows[0][9].ToString();

                if (C.count == 0)
                {
                    //если товара нет отобразить "товара нет в наличии".
                    label8.Visible = true;
                }
                //отобразить информацию о товаре.
                ProductPic.Load(C.PicURL);
                ProductName.Text = C.name_comic;
                Price.Text ="Цена: " + C.price.ToString()+"$";
                Author.Text = "Автор: "+C.author;
                Artist.Text = "Художник: "+C.artist;
                Publish.Text = "Издатель: "+C.publish;
                Genre.Text = "Жанр: "+C.genre;
                Description.Text = C.description;
            }
            else
            {
                MessageBox.Show("ошибка");
            }
        }
        //увеличение значения в countProduct.Text на 1.
        private void button1_Click(object sender, EventArgs e)
        {
            if (int.Parse(countProduct.Text) < C.count)
                countProduct.Text = (int.Parse(countProduct.Text) + 1).ToString();
        }
        //уменьшение значения в countProduct.Text на 1.
        private void button2_Click(object sender, EventArgs e)
        {
            if (int.Parse(countProduct.Text) > 0)
                countProduct.Text = (int.Parse(countProduct.Text) - 1).ToString();
        }
        //кнопка личный кабинет.
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
        //кнопка удаления товара из таблицы.
        private void Deletebutton_Click(object sender, EventArgs e)
        {
            UsersDB db = new UsersDB();
            MySqlCommand command = new MySqlCommand("DELETE FROM `comics` WHERE `id_c` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = C.ID;


            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                //если удаление прошло успешно возврат на форму поиска.
                SearchForm newForm = new SearchForm("");
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
                MessageBox.Show("Товар удален успешно!");
            }
            else
                MessageBox.Show("Ошибка.");

            db.closeConnection();
        }
        //кнопка изменения товара.
        private void Changebutton_Click(object sender, EventArgs e)
        {
            //вызов формы добавления товара с параметром ID для изменения товара с этим ID.
            AddProductForm newForm = new AddProductForm(ID);
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //добавление выбранного количества товаров в корзину.
        private void Product1Buy_Click(object sender, EventArgs e)
        {
            try
            {
                int co;
                UserData.Bascket.OrderIdCount.TryGetValue(ID, out co);
                if ((int.Parse(countProduct.Text) + co) <= C.count)
                {
                    if (int.Parse(countProduct.Text) > 0)
                    {
                        //добавление в корзину.
                        UserData.Bascket.OrderIdCount.Add(ID, int.Parse(countProduct.Text));
                        UserData.Bascket.CountProducts++;
                        MessageBox.Show("Товар добавлен в корзину!");
                    }
                }
                else
                    MessageBox.Show($"Недостаточно шт. товара!\nВ наличии: {C.count-co} шт.");
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Количество товаров указано неверно!");
            }
            catch (System.ArgumentException)
            {
                //добавление товара если такой товар уже ест в корзине.
                int c;
                UserData.Bascket.OrderIdCount.TryGetValue(ID,out c);
                UserData.Bascket.OrderIdCount.Remove(ID);
                UserData.Bascket.OrderIdCount.Add(ID, int.Parse(countProduct.Text)+c);
                 MessageBox.Show("Товар добавлен в корзину!");
            }
        }
        //кнопка корзина.
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Forms.BasketForm newForm = new Forms.BasketForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }

        private void ProductName_MouseEnter(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(ProductName, ProductName.Text);
        }

    }
}
