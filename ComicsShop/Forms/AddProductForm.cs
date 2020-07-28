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
    public partial class AddProductForm : Form
    {
        //стандартный конструктор вызывает форму добавления нового товара.
        public AddProductForm()
        {
            InitializeComponent();
        }
        int ID = 0;
        //Конструктор принимающий ID вызывает форму изменения имеющегося товара.
        public AddProductForm(int ID_C)
        {
            InitializeComponent();
            ID = ID_C;
        }

        //Закрытие окна
        private void exitLabel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //Перекрашиваение крестика
        private void exitLabel_MouseEnter(object sender, EventArgs e)
        {
            exitLabel.ForeColor = Color.FromArgb(80, 80, 80);
        }

        private void exitLabel_MouseLeave(object sender, EventArgs e)
        {
            exitLabel.ForeColor = Color.FromArgb(0, 0, 0);
        }
        //Перемещение окна мышью за верхнюю панель
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
        //Кнопка Отмена возвращает пользователя в форму поиска.
        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            SearchForm newForm = new SearchForm("");
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //при выходе из поля для ввода URL загружает изображение по введённому URL в pictureBox1 если поле URLTextBox.Text не пустое.
        private void URLTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (URLTextBox.Text != "")
                {
                    pictureBox1.Load(URLTextBox.Text);
                }
            }
            catch (System.Net.WebException)
            {
                MessageBox.Show("Неверный URL");
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("Неверный URL");
            }
            catch (System.ArgumentException)
            {
                MessageBox.Show("Неверный URL");
            }
        }
        //кнопка очистить очищает содержимое URLTextBox.Text.
        private void button1_Click(object sender, EventArgs e)
        {
            URLTextBox.Text = "";
        }
        //Кнопка создать создает новый товар и записывает данные о нем из полей формы в БД таблицу comics.
        private void Createbutton_Click(object sender, EventArgs e)
        {
            //проверка заполненности полей, в случае незаполнения какого-либо поля выводится вообщение для того чтобы пользователь его заполнил.
            if (NameField.Text == "")
            {
                MessageBox.Show("Введите название товара");
                return;
            }
            if (URLTextBox.Text == "")
            {
                MessageBox.Show("Введите URL изображения");
                return;
            }
            if (AuthorField.Text == "")
            {
                MessageBox.Show("Укажите автора");
                return;
            }
            if (ArtistField.Text == "")
            {
                MessageBox.Show("Укажите художника");
                return;
            }
            if (PublishField.Text == "")
            {
                MessageBox.Show("Укажите издателя");
                return;
            }
            if (GenreField.Text == "")
            {
                MessageBox.Show("Укажите жанр");
                return;
            }
            if (PriceField.Text == "")
            {
                MessageBox.Show("Укажите цену на товар");
                return;
            }
            if (CountField.Text == "")
            {
                MessageBox.Show("Укажите количетво товаров");
                return;
            }
            if (DescriptionField.Text == "")
            {
                MessageBox.Show("Укажите описание");
                return;
            }
            //если такой товар уже существует то кнопка ничего не делает.
            if (isProductExist())
                return;
            //создание команды для SQL запроса.
            UsersDB db = new UsersDB();
            MySqlCommand command = new MySqlCommand("INSERT INTO `comics`(`name_c`, `picture`, `description`, `price`, `count`, `author`, `genre`, `publish`, `artist`) VALUES (@name, @pic, @desc, @price, @count, @author, @genre, @publ, @artist)", db.getConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = NameField.Text;
            command.Parameters.Add("@pic", MySqlDbType.VarChar).Value = URLTextBox.Text;
            command.Parameters.Add("@desc", MySqlDbType.VarChar).Value = DescriptionField.Text;
            command.Parameters.Add("@price", MySqlDbType.VarChar).Value = PriceField.Text;
            command.Parameters.Add("@count", MySqlDbType.VarChar).Value = CountField.Text;
            command.Parameters.Add("@author", MySqlDbType.VarChar).Value = AuthorField.Text;
            command.Parameters.Add("@genre", MySqlDbType.VarChar).Value = GenreField.Text;
            command.Parameters.Add("@publ", MySqlDbType.VarChar).Value = PublishField.Text;
            command.Parameters.Add("@artist", MySqlDbType.VarChar).Value = ArtistField.Text;

            db.openConnection();
            //запись данных в таблицу.
            if (command.ExecuteNonQuery() == 1)
            {
                SearchForm newForm = new SearchForm("");
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
                MessageBox.Show("Товар добавлен успешно!");
            }
            //обработка ошибки выполнения SQL команды.
            else
                MessageBox.Show("Ошибка.");

            db.closeConnection();
        }
        //функция проверяющая существует ли товар с таким названием в таблице comics БД.
        public Boolean isProductExist()
        {
            UsersDB db = new UsersDB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `comics` WHERE `name_c` = @ul", db.getConnection());
            command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = NameField.Text;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Товар с таким именем уже существует");
                return true;
            }
            else
            {
                return false;
            }
        }
        //при загрузки формы происходит проверка вызванного конструктора, если ID отличен от 0 значит надо загрузить форму изменения товара и заполнить её данными комикса с этим ID, в ином случае происходит загрузка формы добавления товара.
        private void AddProductForm_Load(object sender, EventArgs e)
        {
            if (ID != 0)
            {
                label1.Text = "Изменение товара";

                Createbutton.Visible = false;

                Changebutton.Visible = true;

                ComicClass C = new ComicClass();

                UsersDB db = new UsersDB();

                DataTable table = new DataTable();

                MySqlDataAdapter adapter = new MySqlDataAdapter();

                MySqlCommand command = new MySqlCommand("SELECT * FROM `comics` WHERE `id_c` = @id", db.getConnection());
                command.Parameters.Add("@id", MySqlDbType.VarChar).Value = ID;

                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {

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

                    URLTextBox.Text = C.PicURL;
                    pictureBox1.Load(URLTextBox.Text);
                    NameField.Text = C.name_comic;
                    AuthorField.Text = C.author;
                    DescriptionField.Text = C.description;
                    PriceField.Text = C.price.ToString();
                    GenreField.Text = C.genre;
                    PublishField.Text = C.publish;
                    ArtistField.Text = C.artist;
                    CountField.Text = C.count.ToString();

                }
            }
        }
        //кнопка изменить обновляет данные в таблице comics в соответствии с новыми введёнными в форму данными.
        private void Changebutton_Click(object sender, EventArgs e)
        {
            //проверка заполненности полей.
            if (NameField.Text == "")
            {
                MessageBox.Show("Введите название товара");
                return;
            }
            if (URLTextBox.Text == "")
            {
                MessageBox.Show("Введите URL изображения");
                return;
            }
            if (AuthorField.Text == "")
            {
                MessageBox.Show("Укажите автора");
                return;
            }
            if (ArtistField.Text == "")
            {
                MessageBox.Show("Укажите художника");
                return;
            }
            if (PublishField.Text == "")
            {
                MessageBox.Show("Укажите издателя");
                return;
            }
            if (GenreField.Text == "")
            {
                MessageBox.Show("Укажите жанр");
                return;
            }
            if (PriceField.Text == "")
            {
                MessageBox.Show("Укажите цену на товар");
                return;
            }
            if (CountField.Text == "")
            {
                MessageBox.Show("Укажите количетво товаров");
                return;
            }
            if (DescriptionField.Text == "")
            {
                MessageBox.Show("Укажите описание");
                return;
            }

           
            //создание команды SQL для обновления данных в таблице comics.
            UsersDB db = new UsersDB();
            MySqlCommand command = new MySqlCommand("UPDATE `comics` SET `name_c` = @name, `picture` = @pic, `description` = @desc, `price` = @price, `count` = @count, `author` = @author, `genre` = @genre, `publish` = @publ, `artist` = @artist WHERE `id_c` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = ID.ToString();
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = NameField.Text;
            command.Parameters.Add("@pic", MySqlDbType.VarChar).Value = URLTextBox.Text;
            command.Parameters.Add("@desc", MySqlDbType.VarChar).Value = DescriptionField.Text;
            command.Parameters.Add("@price", MySqlDbType.VarChar).Value = PriceField.Text;
            command.Parameters.Add("@count", MySqlDbType.VarChar).Value = CountField.Text;
            command.Parameters.Add("@author", MySqlDbType.VarChar).Value = AuthorField.Text;
            command.Parameters.Add("@genre", MySqlDbType.VarChar).Value = GenreField.Text;
            command.Parameters.Add("@publ", MySqlDbType.VarChar).Value = PublishField.Text;
            command.Parameters.Add("@artist", MySqlDbType.VarChar).Value = ArtistField.Text;

            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                //При успешном выполнении команды происхоит загрузка формы просмотра этого товара.
                PVForm newForm = new PVForm(ID);
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
                MessageBox.Show("Товар изменён успешно!");
            }
            //Обработка ошибки выполнения SQL команды.
            else
                MessageBox.Show("Ошибка.");

            db.closeConnection();
        }
    }
}
