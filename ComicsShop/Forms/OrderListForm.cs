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
    public partial class OrderListForm : Form
    {
        public OrderListForm()
        {
            InitializeComponent();
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
        //кнопка на главную.
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MainForm newForm = new MainForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
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
        //функция отображения панели с заказом в форму.
        void ShowPan(ProductPanelClass Pi, ComicClass Ci)
        {
            Pi.name.Text = Ci.ID.ToString();
            Pi.price.Text = Ci.price.ToString() + '$';
            Pi.genre.Text = Ci.genre.ToString();
            Pi.pan.Visible = true;
        }
        //переменные: количество страниц, текущая страница, таблица с информацией о заказах, массив С в котором будет информация о заказах, массив Р для отображения заказов в форму, таблица в которую будет считываться информация о комиксах.
        int countOfPages;
        int currentPage = 1;
        DataTable orderTable = new DataTable();
        ComicClass[] C;
        ProductPanelClass[] P = new ProductPanelClass[4];
        DataTable table = new DataTable();

        private void OrderListForm_Load(object sender, EventArgs e)
        {
            //считывание информации о заказах в таблицу.
            UsersDB db = new UsersDB();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `orders`", db.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(orderTable);

            C = new ComicClass[orderTable.Rows.Count];
            //подсчет количества страниц заказов.
            if (orderTable.Rows.Count % 4 == 0)
                countOfPages = orderTable.Rows.Count / 4;
            else
                countOfPages = (orderTable.Rows.Count / 4) + 1;

            Pages.Text = currentPage.ToString();
            //присвоение массиву Р элементов формы.
            P[0] = new ProductPanelClass();
            P[0].pan = Product1Pan;
            P[0].name = Product1Name;
            P[0].price = Product1Price;
            P[0].genre = Product1names;


            P[1] = new ProductPanelClass();
            P[1].pan = Product2Pan;
            P[1].name = Product2Name;
            P[1].price = Product2Price;
            P[1].genre = Product2names;

            P[2] = new ProductPanelClass();
            P[2].pan = Product3Pan;
            P[2].name = Product3Name;
            P[2].price = Product3Price;
            P[2].genre = Product3names;

            P[3] = new ProductPanelClass();
            P[3].pan = Product4Pan;
            P[3].name = Product4Name;
            P[3].price = Product4Price;
            P[3].genre = Product4names;
            //занесение информации о каждом заказе из таблицы в массив С
            for (int i = 0; i < orderTable.Rows.Count; i++)
            {
                
                
                if (i < orderTable.Rows.Count)
                {
                    C[i] = new ComicClass();
                    C[i].genre = "";
                    C[i] = new ComicClass();
                    C[i].ID = int.Parse(orderTable.Rows[i][0].ToString());
                    C[i].price = int.Parse(orderTable.Rows[i][3].ToString());
                    //расшифровка строки в словарь со значениями <ID, количество>.
                    Dictionary<int, int>  M = UserData.Bascket.GetIds(orderTable.Rows[i][1].ToString());
                    int j = 0;

                    foreach (KeyValuePair<int, int> kvp in M)
                    {
                        //формирования строки с названиями и количеством комиксов в данном заказе.
                        command = new MySqlCommand("SELECT * FROM `comics` WHERE `id_c` = @id", db.getConnection());
                        command.Parameters.Add("@id", MySqlDbType.VarChar).Value = kvp.Key;
                        db.openConnection();
                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                        C[i].genre += table.Rows[0][2] + " " + kvp.Value.ToString() + "; ";
                        table.Clear();
                        j++;
                    }
                    //отображение первых 4х заказов в форму.
                    if (i<4)
                        ShowPan(P[i], C[i]);
                }
            }
        }
        //кнопка следующая страница считывает из таблицы orders данные о заказе и из таблицы comics данные о комиксах имеющихся в заказе и отображает соответствующие странице 4 товара.
        private void button2_Click(object sender, EventArgs e)
        {
            UsersDB db = new UsersDB();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            
            MySqlCommand command = new MySqlCommand("SELECT * FROM `orders`", db.getConnection());
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
                    if (i < orderTable.Rows.Count)
                    {
                        C[i] = new ComicClass();
                        C[i].ID = int.Parse(orderTable.Rows[i][0].ToString());
                        C[i].price = int.Parse(orderTable.Rows[i][3].ToString());
                        Dictionary<int, int> M = UserData.Bascket.GetIds(orderTable.Rows[i][1].ToString());
                        int j = 0;

                        foreach (KeyValuePair<int, int> kvp in M)
                        {
                            command = new MySqlCommand("SELECT * FROM `comics` WHERE `id_c` = @id", db.getConnection());
                            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = kvp.Key;
                            db.openConnection();
                            adapter.SelectCommand = command;
                            adapter.Fill(table);
                            C[i].genre += table.Rows[0][2] + " " + kvp.Value.ToString() + "шт.; ";
                            table.Clear();
                            j++;
                        }
                    }

                }
                for (int j = 0; j < 4; j++)
                {
                    if ((j + currentPage * 4 - 4) < orderTable.Rows.Count)
                        ShowPan(P[j], C[j + currentPage * 4 - 4]);
                }
            }
        }
        //кнопка предыдущая страница считывает из таблицы orders данные о заказе и из таблицы comics данные о комиксах имеющихся в заказе и отображает соответствующие странице 4 товара.
        private void button3_Click(object sender, EventArgs e)
        {
            UsersDB db = new UsersDB();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `orders`", db.getConnection());
            if (1 < currentPage)
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
                    if (i < orderTable.Rows.Count)
                    {
                        C[i] = new ComicClass();
                        C[i].ID = int.Parse(orderTable.Rows[i][0].ToString());
                        C[i].price = int.Parse(orderTable.Rows[i][3].ToString());
                        Dictionary<int, int> M = UserData.Bascket.GetIds(orderTable.Rows[i][1].ToString());
                        int j = 0;

                        foreach (KeyValuePair<int, int> kvp in M)
                        {
                            command = new MySqlCommand("SELECT * FROM `comics` WHERE `id_c` = @id", db.getConnection());
                            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = kvp.Key;
                            db.openConnection();
                            adapter.SelectCommand = command;
                            adapter.Fill(table);
                            C[i].genre += table.Rows[0][2] + " " + kvp.Value.ToString() + "; ";
                            table.Clear();
                            j++;
                        }
                    }

                }
                for (int j = 0; j < 4; j++)
                {
                    if ((j + currentPage * 4 - 4) < orderTable.Rows.Count)
                        ShowPan(P[j], C[j + currentPage * 4 - 4]);
                }
            }
        }
        //кнопка заказ выдан для 1й панели
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            UsersDB db = new UsersDB();
            MySqlCommand command = new MySqlCommand("DELETE FROM `orders` WHERE `id_order` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = C[currentPage*4-4].ID;


            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                OrderListForm newForm = new OrderListForm();
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
                MessageBox.Show("Заказ выдан!");
            }
            else
                MessageBox.Show("Ошибка.");

            db.closeConnection();
        }




        //кнопка заказ выдан для 2й панели
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            UsersDB db = new UsersDB();
            MySqlCommand command = new MySqlCommand("DELETE FROM `orders` WHERE `id_order` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = C[currentPage * 4 - 3].ID;


            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                OrderListForm newForm = new OrderListForm();
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
                MessageBox.Show("Заказ выдан!");
            }
            else
                MessageBox.Show("Ошибка.");

            db.closeConnection();
        }
        //кнопка заказ выдан для 3й панели
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            UsersDB db = new UsersDB();
            MySqlCommand command = new MySqlCommand("DELETE FROM `orders` WHERE `id_order` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = C[currentPage * 4 - 2].ID;


            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                OrderListForm newForm = new OrderListForm();
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
                MessageBox.Show("Заказ выдан!");
            }
            else
                MessageBox.Show("Ошибка.");

            db.closeConnection();
        }
        //кнопка заказ выдан для 4й панели
        private void pictureBox10_Click(object sender, EventArgs e)
        {
            UsersDB db = new UsersDB();
            MySqlCommand command = new MySqlCommand("DELETE FROM `orders` WHERE `id_order` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = C[currentPage * 4 - 1].ID;


            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                OrderListForm newForm = new OrderListForm();
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
                MessageBox.Show("Заказ выдан!");
            }
            else
                MessageBox.Show("Ошибка.");

            db.closeConnection();
        }  
    }
}
