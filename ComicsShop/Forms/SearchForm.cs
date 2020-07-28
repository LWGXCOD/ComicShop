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
    public partial class SearchForm : Form
    {
        string request;
        //сохранение переданной в конструкторе в качестве аргумента строки поискового запроса.
        public SearchForm(string req)
        {
            request = req;
            InitializeComponent();
        }
        //Закрытие приложения.
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
        //кнопка раскрывающая и скрывающая сортировку по жанрам
        private void genreButton_Click(object sender, EventArgs e)
        {
            if (AuthorPanel.Visible == false)
            {
                AuthorPanel.Visible = true;
                AuthorPanel.Enabled = true;
                genreButton.Text = genreButton.Text.Trim('˅');
                genreButton.Text = genreButton.Text.Insert(26, "˄");

            }
            else if (AuthorPanel.Visible == true)
            {
                AuthorPanel.Visible = false;
                AuthorPanel.Enabled = false;
                genreButton.Text = genreButton.Text.Trim('˄');
                genreButton.Text = genreButton.Text.Insert(26, "˅");
            }
        }
        //переменные: количество страниц, текущая страница, таблица комиксов, массив С для хранения информации о комиксах, массив Р для отображения информации в элементы формы.
        int countOfPages;
        int currentPage = 1;
        DataTable comicTable = new DataTable();
        ComicClass[] C;
        ProductPanelClass[] P = new ProductPanelClass[4];
        //функция отображающая в элементы панели Р информацию о комиксе из С.
        void ShowPan(ProductPanelClass Pi, ComicClass Ci)
        {

            
            Pi.pic.Load(Ci.PicURL);
            Pi.name.Text = Ci.name_comic;
            Pi.price.Text = Ci.price.ToString() + '$';
            Pi.genre.Text = Ci.genre;
            Pi.pan.Visible = true;
        }
        
        private void SearchForm_Load(object sender, EventArgs e)
        {
            if (UserData.Login != null)
            {
                //отображение кнопки корзина и кнопок в корзину для авторизированных пользователей.
                pictureBox2.Visible = true;
                Product1Buy.Visible = true;
                Product2Buy.Visible = true;
                Product3Buy.Visible = true;
                Product4Buy.Visible = true;
            }
            //помещение в поисковую строку текста переданного в форму в качестве аргумента.
            SearchField.Text = request;

            UsersDB db = new UsersDB();

            AddProductPic.Visible = false;

            if (UserData.Access == true)
            {
                //отображение кнопки добавления товара при уровне доступа: администратор.
                AddProductPic.Visible = true;
            }

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `comics`", db.getConnection());
            //затолнение таблицы каталогом комиксов.
            adapter.SelectCommand = command;
            adapter.Fill(comicTable);

            C = new ComicClass[comicTable.Rows.Count];



            //подсчет количества страниц
            if (comicTable.Rows.Count % 4 == 0)
                countOfPages = comicTable.Rows.Count / 4;
            else
                countOfPages = (comicTable.Rows.Count / 4) + 1;

            Pages.Text = currentPage.ToString();



            //присвоение элементов панели соответствующим элементам массива Р.
            P[0] = new ProductPanelClass();
            P[0].pan = Product1Pan;
            P[0].name = Product1Name;
            P[0].price = Product1Price;
            P[0].genre = Product1genre;
            P[0].button = Product1Buy;
            P[0].pic = Product1Pic;

            P[1] = new ProductPanelClass();
            P[1].pan = Product2Pan;
            P[1].name = Product2Name;
            P[1].price = Product2Price;
            P[1].genre = Product2genre;
            P[1].button = Product2Buy;
            P[1].pic = Product2Pic;

            P[2] = new ProductPanelClass();
            P[2].pan = Product3Pan;
            P[2].name = Product3Name;
            P[2].price = Product3Price;
            P[2].genre = Product3genre;
            P[2].button = Product3Buy;
            P[2].pic = Product3Pic;

            P[3] = new ProductPanelClass();
            P[3].pan = Product4Pan;
            P[3].name = Product4Name;
            P[3].price = Product4Price;
            P[3].genre = Product4genre;
            P[3].button = Product4Buy;
            P[3].pic = Product4Pic;
            
            //считывание и отображение первых 4х товаров.
            for (int i = 0; i < 4; i++)
            {
                if (i < comicTable.Rows.Count)
                {
                    C[i] = new ComicClass();
                    C[i].ID = int.Parse(comicTable.Rows[i][0].ToString());
                    C[i].PicURL = comicTable.Rows[i][1].ToString();
                    C[i].name_comic = comicTable.Rows[i][2].ToString();
                    C[i].description = comicTable.Rows[i][3].ToString();
                    C[i].price = int.Parse(comicTable.Rows[i][4].ToString());
                    C[i].count = int.Parse(comicTable.Rows[i][5].ToString());
                    C[i].author = comicTable.Rows[i][6].ToString();
                    C[i].genre = comicTable.Rows[i][7].ToString();
                    ShowPan(P[i], C[i]);
                }
            }

            button1_Click(sender, e);
        }
        //открытие следующей страницы с отображением соответствующих ей товаров.
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
                int i = (currentPage * 4) - 4;
                for (; i <= (currentPage * 4 - 1); i++)
                {
                    if (i < comicTable.Rows.Count)
                    {
                        C[i] = new ComicClass();
                        C[i].ID = int.Parse(comicTable.Rows[i][0].ToString());
                        C[i].PicURL = comicTable.Rows[i][1].ToString();
                        C[i].name_comic = comicTable.Rows[i][2].ToString();
                        C[i].description = comicTable.Rows[i][3].ToString();
                        C[i].price = int.Parse(comicTable.Rows[i][4].ToString());
                        C[i].count = int.Parse(comicTable.Rows[i][5].ToString());
                        C[i].author = comicTable.Rows[i][6].ToString();
                        C[i].genre = comicTable.Rows[i][7].ToString();
                    }
                    
                }
                for (int j = 0; j < 4; j++)
                {
                    if ((j + currentPage * 4 -4) < comicTable.Rows.Count)
                        ShowPan(P[j], C[j + currentPage * 4 - 4]);
                }
            }
        }
        //открытие предыдущей страницы с отображением соответствующих ей товаров.
        private void button3_Click(object sender, EventArgs e)
        {
            if ( 1 < currentPage)
            {
                for (int j = 0; j < 4; j++)
                {
                    P[j].pan.Visible = false;
                }
                currentPage--;
                Pages.Text = currentPage.ToString();
                int i = (currentPage * 4) - 4;
                for (; i <= (currentPage * 4 - 1); i++)
                {
                    if (i < comicTable.Rows.Count)
                    {
                        C[i] = new ComicClass();
                        C[i].ID = int.Parse(comicTable.Rows[i][0].ToString());
                        C[i].PicURL = comicTable.Rows[i][1].ToString();
                        C[i].name_comic = comicTable.Rows[i][2].ToString();
                        C[i].description = comicTable.Rows[i][3].ToString();
                        C[i].price = int.Parse(comicTable.Rows[i][4].ToString());
                        C[i].count = int.Parse(comicTable.Rows[i][5].ToString());
                        C[i].author = comicTable.Rows[i][6].ToString();
                        C[i].genre = comicTable.Rows[i][7].ToString();
                    }

                }
                for (int j = 0; j < 4; j++)
                {
                    if ((j + currentPage * 4 - 4) < comicTable.Rows.Count)
                        ShowPan(P[j], C[j + currentPage * 4 - 4]);
                }
            }
        }
        //кнопка добавления нового товара доступная только администратору.
        private void AddProductPic_Click(object sender, EventArgs e)
        {
            Forms.AddProductForm newForm = new Forms.AddProductForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //подсказка на главную.
        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(pictureBox3, "На главную");
        }
        //подсказка личный кабинет.
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(pictureBox1, "Личный кабинет");
        }
        //подсказка корзина.
        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(pictureBox2, "Корзина");
        }
        //подсказка добавление товара.
        private void AddProductPic_MouseEnter(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(AddProductPic, "Добавление товара");
        }
        //кнопка личный кабинет
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
        //кнопка найти
        private void button1_Click(object sender, EventArgs e)
        {
            //проверка на то что выбран хотя бы 1 жанр.
            if (!(AuthorcheckBox1.Checked || AuthorcheckBox2.Checked || AuthorcheckBox3.Checked || AuthorcheckBox4.Checked || AuthorcheckBox5.Checked || AuthorcheckBox6.Checked))
            {
                MessageBox.Show("Не выбрано ни одного жанра!");
                return;
            }
            //скрытие панелей.
            for (int i = 0; i < 4; i++)
            {
                P[i].pan.Visible = false;
            }

            UsersDB db = new UsersDB();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command;
            //задание SQL команды для поиска по разным полям(название, художник, издатель, автор)
            if (radioButton2.Checked)
            {
                if (SearchField.Text == "")
                {
                    command = new MySqlCommand("SELECT * FROM `comics` WHERE (`genre` = @1 OR `genre` = @2 OR `genre` = @3 OR `genre` = @4 OR `genre` = @5 OR `genre` = @6)", db.getConnection());
                }
                else
                    command = new MySqlCommand("SELECT * FROM `comics` WHERE `artist` LIKE @art AND (`genre` = @1 OR `genre` = @2 OR `genre` = @3 OR `genre` = @4 OR `genre` = @5 OR `genre` = @6)", db.getConnection());
            }
            else if (radioButton3.Checked)
            {
                if (SearchField.Text == "")
                {
                    command = new MySqlCommand("SELECT * FROM `comics` WHERE (`genre` = @1 OR `genre` = @2 OR `genre` = @3 OR `genre` = @4 OR `genre` = @5 OR `genre` = @6)", db.getConnection());
                }
                else
                command = new MySqlCommand("SELECT * FROM `comics` WHERE `publish` LIKE @pb AND (`genre` = @1 OR `genre` = @2 OR `genre` = @3 OR `genre` = @4 OR `genre` = @5 OR `genre` = @6)", db.getConnection());
            }
            else if (radioButton1.Checked)
            {
                if (SearchField.Text == "")
                {
                    command = new MySqlCommand("SELECT * FROM `comics` WHERE (`genre` = @1 OR `genre` = @2 OR `genre` = @3 OR `genre` = @4 OR `genre` = @5 OR `genre` = @6)", db.getConnection());
                }
                else
                command = new MySqlCommand("SELECT * FROM `comics` WHERE `author` LIKE @au AND (`genre` = @1 OR `genre` = @2 OR `genre` = @3 OR `genre` = @4 OR `genre` = @5 OR `genre` = @6)", db.getConnection());
            }
            else
            {
                if (SearchField.Text == "")
                {
                    command = new MySqlCommand("SELECT * FROM `comics` WHERE (`genre` = @1 OR `genre` = @2 OR `genre` = @3 OR `genre` = @4 OR `genre` = @5 OR `genre` = @6)", db.getConnection());
                }
                else
                command = new MySqlCommand("SELECT * FROM `comics` WHERE `name_c` LIKE @nc AND (`genre` = @1 OR `genre` = @2 OR `genre` = @3 OR `genre` = @4 OR `genre` = @5 OR `genre` = @6)", db.getConnection());
            }



            command.Parameters.Add("@nc", MySqlDbType.VarChar).Value = "%"+SearchField.Text+"%";
            command.Parameters.Add("@art", MySqlDbType.VarChar).Value = "%"+SearchField.Text+ "%";
            command.Parameters.Add("@pb", MySqlDbType.VarChar).Value = "%"+SearchField.Text+ "%";
            command.Parameters.Add("@au", MySqlDbType.VarChar).Value = "%"+SearchField.Text+ "%";

            string savetext = "";
            //заполнение заглушек в зависимости от выбраных жанров.

            if (AuthorcheckBox1.Checked)
            {
                command.Parameters.Add("@1", MySqlDbType.VarChar).Value = AuthorcheckBox1.Text;
                savetext = AuthorcheckBox1.Text;
            }
            if (AuthorcheckBox2.Checked)
            {
                command.Parameters.Add("@2", MySqlDbType.VarChar).Value = AuthorcheckBox2.Text;
                savetext = AuthorcheckBox2.Text;
            }
            if (AuthorcheckBox3.Checked)
            {
                command.Parameters.Add("@3", MySqlDbType.VarChar).Value = AuthorcheckBox3.Text;
                savetext = AuthorcheckBox3.Text;
            }
            if (AuthorcheckBox4.Checked)
            {
                command.Parameters.Add("@4", MySqlDbType.VarChar).Value = AuthorcheckBox4.Text;
                savetext = AuthorcheckBox4.Text;
            }
            if (AuthorcheckBox5.Checked)
            {
                command.Parameters.Add("@5", MySqlDbType.VarChar).Value = AuthorcheckBox5.Text;
                savetext = AuthorcheckBox5.Text;
            }
            if (AuthorcheckBox6.Checked)
            {
                command.Parameters.Add("@6", MySqlDbType.VarChar).Value = AuthorcheckBox6.Text;
                savetext = AuthorcheckBox6.Text;
            }

            if (!AuthorcheckBox1.Checked)
            {
                command.Parameters.Add("@1", MySqlDbType.VarChar).Value = savetext;
            }
            if (!AuthorcheckBox2.Checked)
            {
                command.Parameters.Add("@2", MySqlDbType.VarChar).Value = savetext;
            }
            if (!AuthorcheckBox3.Checked)
            {
                command.Parameters.Add("@3", MySqlDbType.VarChar).Value = savetext;
            }
            if (!AuthorcheckBox4.Checked)
            {
                command.Parameters.Add("@4", MySqlDbType.VarChar).Value = savetext;
            }
            if (!AuthorcheckBox5.Checked)
            {
                command.Parameters.Add("@5", MySqlDbType.VarChar).Value = savetext;
            }
            if (!AuthorcheckBox6.Checked)
            {
                command.Parameters.Add("@6", MySqlDbType.VarChar).Value = savetext;
            }
            //поиск по выбранному полю и выбранным жанрам и заполнение таблицы.
            DataTable searchTable = new DataTable();
            adapter.SelectCommand = command;
            adapter.Fill(searchTable);
            comicTable = searchTable;

            C = new ComicClass[comicTable.Rows.Count];

            currentPage = 1;
            //подсчет количества страниц.
            if (comicTable.Rows.Count % 4 == 0)
                countOfPages = comicTable.Rows.Count / 4;
            else
                countOfPages = (comicTable.Rows.Count / 4) + 1;

            Pages.Text = currentPage.ToString();
            //поиск не дал результатов.
            if (comicTable.Rows.Count == 0)
            {
                MessageBox.Show("Ничего не найдено");
            }
            //отображение перввых 4х товаров в форму.
            for (int i = 0; i < 4; i++)
            {
                if (i < comicTable.Rows.Count)
                {
                    C[i] = new ComicClass();
                    C[i].ID = int.Parse(comicTable.Rows[i][0].ToString());
                    C[i].PicURL = comicTable.Rows[i][1].ToString();
                    C[i].name_comic = comicTable.Rows[i][2].ToString();
                    C[i].description = comicTable.Rows[i][3].ToString();
                    C[i].price = int.Parse(comicTable.Rows[i][4].ToString());
                    C[i].count = int.Parse(comicTable.Rows[i][5].ToString());
                    C[i].author = comicTable.Rows[i][6].ToString();
                    C[i].genre = comicTable.Rows[i][7].ToString();
                    ShowPan(P[i], C[i]);
                }

            }
        }
        //открытие формы просмотра для первого товара.
        private void Product1View_Click(object sender, EventArgs e)
        {
            Forms.PVForm newForm = new Forms.PVForm(C[currentPage * 4 - 4].ID);
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //открытие формы просмотра для второго товара.
        private void Product2View_Click(object sender, EventArgs e)
        {
            Forms.PVForm nForm = new Forms.PVForm(C[currentPage * 4 - 3].ID);
            nForm.Left = this.Left;
            nForm.Top = this.Top;
            nForm.Show();
            this.Hide();

        }
        //открытие формы просмотра для третьего товара.
        private void Product3View_Click(object sender, EventArgs e)
        {
            Forms.PVForm nForm = new Forms.PVForm(C[currentPage * 4 - 2].ID);
            nForm.Left = this.Left;
            nForm.Top = this.Top;
            nForm.Show();
            this.Hide();
        }
        //открытие формы просмотра для четвертого товара.
        private void Product4View_Click(object sender, EventArgs e)
        {
            Forms.PVForm nForm = new Forms.PVForm(C[currentPage * 4 - 1].ID);
            nForm.Left = this.Left;
            nForm.Top = this.Top;
            nForm.Show();
            this.Hide();
        }
        //кнопка добавления первого товара в корзинну.
        private void Product1Buy_Click(object sender, EventArgs e)
        {
            int id = C[currentPage * 4 - 4].ID;
            int co;
            UserData.Bascket.OrderIdCount.TryGetValue(C[currentPage * 4 - 4].ID, out co);
            if ((!UserData.Bascket.OrderIdCount.ContainsKey(C[currentPage * 4 - 4].ID))||(co < C[currentPage * 4 - 4].count))
            {
                try
                {
                    UserData.Bascket.OrderIdCount.Add(id, 1);
                    UserData.Bascket.CountProducts++;
                    MessageBox.Show("Товар добавлен в корзину!");

                }
                catch (System.ArgumentException)
                {
                    int c;
                    UserData.Bascket.OrderIdCount.TryGetValue(id, out c);
                    UserData.Bascket.OrderIdCount.Remove(id);
                    UserData.Bascket.OrderIdCount.Add(id, 1 + c);
                    MessageBox.Show("Товар добавлен в корзину!");
                }
            }
            else
                MessageBox.Show($"Недостаточно шт. товара!\nВ наличии: {C[currentPage * 4 - 4].count - co} шт.");
        }
        //кнопка добавления второго товара в корзинну.
        private void Product2Buy_Click(object sender, EventArgs e)
        {
            int id = C[currentPage * 4 - 3].ID;
            int co;
            UserData.Bascket.OrderIdCount.TryGetValue(C[currentPage * 4 - 3].ID, out co);
            if ((!UserData.Bascket.OrderIdCount.ContainsKey(C[currentPage * 4 - 3].ID))||(co < C[currentPage * 4 - 3].count))
            {
                try
                {
                    UserData.Bascket.OrderIdCount.Add(id, 1);
                    UserData.Bascket.CountProducts++;
                    MessageBox.Show("Товар добавлен в корзину!");

                }
                catch (System.ArgumentException)
                {
                    int c;
                    UserData.Bascket.OrderIdCount.TryGetValue(id, out c);
                    UserData.Bascket.OrderIdCount.Remove(id);
                    UserData.Bascket.OrderIdCount.Add(id, 1 + c);
                    MessageBox.Show("Товар добавлен в корзину!");
                }
            }
            else
                MessageBox.Show($"Недостаточно шт. товара!\nВ наличии: {C[currentPage * 4 - 3].count - co} шт.");
        }
        //кнопка добавления третьего товара в корзинну.
        private void Product3Buy_Click(object sender, EventArgs e)
        {
            int id = C[currentPage * 4 - 2].ID;
            int co;
            UserData.Bascket.OrderIdCount.TryGetValue(C[currentPage * 4 - 2].ID, out co);
            if ((!UserData.Bascket.OrderIdCount.ContainsKey(C[currentPage * 4 - 2].ID))||(co < C[currentPage * 4 - 2].count))
            {
                try
                {
                    UserData.Bascket.OrderIdCount.Add(id, 1);
                    UserData.Bascket.CountProducts++;
                    MessageBox.Show("Товар добавлен в корзину!");

                }
                catch (System.ArgumentException)
                {
                    int c;
                    UserData.Bascket.OrderIdCount.TryGetValue(id, out c);
                    UserData.Bascket.OrderIdCount.Remove(id);
                    UserData.Bascket.OrderIdCount.Add(id, 1 + c);
                    MessageBox.Show("Товар добавлен в корзину!");
                }
            }
            else
                MessageBox.Show($"Недостаточно шт. товара!\nВ наличии: {C[currentPage * 4 - 2].count - co} шт.");
        }
        //кнопка добавления четвертого товара в корзинну.
        private void Product4Buy_Click(object sender, EventArgs e)
        {
            int id = C[currentPage * 4 - 1].ID;
            int co;
            UserData.Bascket.OrderIdCount.TryGetValue(C[currentPage * 4 - 1].ID, out co);
            if  (!UserData.Bascket.OrderIdCount.ContainsKey(C[currentPage * 4 - 1].ID)||(co < C[currentPage * 4 - 1].count))
            {
                try
                {
                    UserData.Bascket.OrderIdCount.Add(id, 1);
                    UserData.Bascket.CountProducts++;
                    MessageBox.Show("Товар добавлен в корзину!");

                }
                catch (System.ArgumentException)
                {
                    int c;
                    UserData.Bascket.OrderIdCount.TryGetValue(id, out c);
                    UserData.Bascket.OrderIdCount.Remove(id);
                    UserData.Bascket.OrderIdCount.Add(id, 1 + c);
                    MessageBox.Show("Товар добавлен в корзину!");
                }
            }
            else
                MessageBox.Show($"Недостаточно шт. товара!\nВ наличии: {C[currentPage * 4 - 1].count - co} шт.");
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


    }
}
