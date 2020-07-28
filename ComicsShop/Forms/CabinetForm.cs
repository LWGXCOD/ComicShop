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
    public partial class CabinetForm : Form
    {
        public CabinetForm()
        {
            InitializeComponent();
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

        //Перемещение окна
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
        //кнопка домой.
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MainForm newForm = new MainForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //переменные: таблица заказов оформленных авторизированным пользователем, массив С в котором будет находится информация о комиксах заказыных пользователем, таблица в которую будут считываться комиксы по ID.
        DataTable orderTable = new DataTable();
        ComicClass[] C;
        DataTable table = new DataTable();
        private void CabinetForm_Load(object sender, EventArgs e)
        {
            //заполнение полей информацией о пользователе.
            NameField.Text = UserData.Name;
            SurNameField.Text = UserData.SurName;
            MobileField.Text = UserData.Mobile;
            LoginLabel.Text = UserData.Login;
            EmailLabel.Text = UserData.Email;

            UsersDB db = new UsersDB();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            //поиск в БД заказов оформленных на текущего пользователя.
            MySqlCommand command = new MySqlCommand("SELECT * FROM `orders` WHERE `id_client` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = UserData.ID;

            adapter.SelectCommand = command;
            adapter.Fill(orderTable);

            C = new ComicClass[orderTable.Rows.Count];
            //заполнение С информацией о комиксах заказаных пользователем
            //пробег по всем заказам.
            for (int i = 0; i < orderTable.Rows.Count; i++)
            {


                if (i < orderTable.Rows.Count)
                {
                    C[i] = new ComicClass();
                    C[i].genre = "";
                    C[i] = new ComicClass();
                    C[i].ID = int.Parse(orderTable.Rows[i][0].ToString());
                    C[i].price = int.Parse(orderTable.Rows[i][3].ToString());
                    Dictionary<int, int> M = UserData.Bascket.GetIds(orderTable.Rows[i][1].ToString());
                    int j = 0;
                    //пробег по всем комиксам в текущем заказе
                    foreach (KeyValuePair<int, int> kvp in M)
                    {
                        //создание и занесение в C[i].genre строки в которой перечисленны названия и количество заказаных пользователем комиксов.
                        command = new MySqlCommand("SELECT * FROM `comics` WHERE `id_c` = @id", db.getConnection());
                        command.Parameters.Add("@id", MySqlDbType.VarChar).Value = kvp.Key;
                        db.openConnection();
                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                        C[i].genre += table.Rows[0][2] + " " + kvp.Value.ToString() + "шт.; ";
                        table.Clear();
                        j++;
                    }
                    //отображение информации о казаче в поле label11.
                    label11.Text += "Номер заказа: "+ C[i].ID+" Товары: " +C[i].genre + " Сумма: " + C[i].price.ToString()+"$\n";
                }
            }
           
        }
        //кнопка сохранить изменения обновляет информацию в таблице comics в соответствии с новой введённой пользователем информацией.
        private void button2_Click(object sender, EventArgs e)
        {
            UserData.Name = NameField.Text;
            UserData.SurName = SurNameField.Text;
            UserData.Mobile = MobileField.Text;

            UsersDB db = new UsersDB();
            MySqlCommand command = new MySqlCommand("UPDATE `users` SET  `name` = @name, `surname` = @surname, `mobile` = @mobile WHERE `id` = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = UserData.ID;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = NameField.Text;
            command.Parameters.Add("@surname", MySqlDbType.VarChar).Value = SurNameField.Text;
            command.Parameters.Add("@mobile", MySqlDbType.VarChar).Value = MobileField.Text;

            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Изменения сохранены!");
            }
            else
                MessageBox.Show("Ошибка.");

            db.closeConnection();
        }
        //кнопка выхода из аккаунта обнуляет информацию о пользователе хранящуюся в UserData и загружает форму авторизации.
        private void button1_Click(object sender, EventArgs e)
        {
            UserData.ID = 0;
            UserData.Login = null;
            UserData.Password = null;
            UserData.Email = null;
            UserData.Name = null;
            UserData.SurName = null;
            UserData.Mobile = null;
            UserData.Access = false;
            UserData.Bascket.CountProducts = 0;
            UserData.Bascket.OrderIdCount.Clear();
            UserData.Bascket.OrderString = "";
            UserData.Bascket.sum = 0;

            LogInForm newForm = new LogInForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //кнопка изменить пароль загружает форму изменения пароля.
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            ChangePassForm newForm = new ChangePassForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //подсказка при наведении на кнопку "на главную"
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(pictureBox1, "На главную");
        }
        //кнопка корзина
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
