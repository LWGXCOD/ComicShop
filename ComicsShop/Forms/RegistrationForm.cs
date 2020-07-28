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
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        //Закрытие приложения.
        private void exitLabel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        //Перекрашивание крестика.
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
        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
        //Регистрация.
        private void Registerbutton_Click(object sender, EventArgs e)
        {
            //вывод уведомления о пустых полях.
            if (LoginField.Text == "")
            {
                MessageBox.Show("Введите Логин");
                return;
            }
            if (NameField.Text == "")
            {
                MessageBox.Show("Введите Имя");
                return;
            }
            if (MobileField.Text == "")
            {
                MessageBox.Show("Введите Номер телефона");
                return;
            }
            if (SurNameField.Text == "")
            {
                MessageBox.Show("Введите Фамилию");
                return;
            }
            if (PassField.Text == "")
            {
                MessageBox.Show("Введите Пароль");
                return;
            }
            if (EmailField.Text == "")
            {
                MessageBox.Show("Введите E-mail");
                return;
            }
            //проверка на занятость e-mail адресса,если адрес занят то регистрация не удалась.
            if (isEmailExist())
            {
                MessageBox.Show("e-mail занят");
                return;
            }
            //проверка на зянятость логина, если логин занят то регистрация не удалась.
            if (isLoginExist())
            {
                MessageBox.Show("login занят");
                return;
            }
            //создание новой записи в таблице users с заданными пользователем данными.
            UsersDB db = new UsersDB();
            MySqlCommand command = new MySqlCommand("INSERT INTO `users`(`login`, `password`, `email`, `name`, `surname`, `mobile`) VALUES (@login, @password, @email, @name, @surname, @mobile)", db.getConnection());
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = LoginField.Text;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = PassField.Text;
            command.Parameters.Add("@email", MySqlDbType.VarChar).Value = EmailField.Text;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = NameField.Text;
            command.Parameters.Add("@surname", MySqlDbType.VarChar).Value = SurNameField.Text;
            command.Parameters.Add("@mobile", MySqlDbType.VarChar).Value = MobileField.Text;

            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                LogInForm newForm = new LogInForm();
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
                MessageBox.Show("Регистрация прошла успешно!");
            }
            else
                MessageBox.Show("Ошибка регистрации.");

            db.closeConnection();
        }

        //Проверка на существующий логин true - логин занят
        public Boolean isLoginExist()
        {
            UsersDB db = new UsersDB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @ul", db.getConnection());
            command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = LoginField.Text;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Логин занят");
                return true;
            }
            else
            {
                return false;
            }
        }

        //Проверка на существующий email true - email занят
        public Boolean isEmailExist()
        {
            UsersDB db = new UsersDB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `email` = @ue", db.getConnection());
            command.Parameters.Add("@ue", MySqlDbType.VarChar).Value = EmailField.Text;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("E-mail занят");
                return true;
            }
            else
            {
                return false;
            }
        }
        //Открытие окна авторизации
        private void button2_Click(object sender, EventArgs e)
        {
            LogInForm newForm = new LogInForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //кнопка на главную.
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MainForm newForm = new MainForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //подсказка для кнопки на главную.
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(pictureBox1, "На главную");
        }
    }
}
