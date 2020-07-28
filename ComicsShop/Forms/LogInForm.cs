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
    public partial class LogInForm : Form
    {
        public LogInForm()
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

        //Кнопка регистрации открывает форму регистрации
        private void button2_Click(object sender, EventArgs e)
        {
            RegistrationForm newForm = new RegistrationForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }


        //кнопка логин выполняет проверку сауществования пользователя с введёнными логином и паролем в БД, если такой пользователь существует, открывается форма личный кабинет, а его данные заносятся в UserDаta.
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string Userlogin = Loginfield.Text;
            string Userpass = Passfield.Text;

            UsersDB db = new UsersDB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            //задание команды поиска пользователя по паролю и логину.
            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @ul AND `password` = @up", db.getConnection());
            command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = Userlogin;
            command.Parameters.Add("@up", MySqlDbType.VarChar).Value = Userpass;

            adapter.SelectCommand = command;
            adapter.Fill(table);
            //проверка на авторизацию
            if (table.Rows.Count > 0)
            {
                //успешная авторизация.
                UserData.ID = int.Parse(table.Rows[0][0].ToString());
                UserData.Login = table.Rows[0][1].ToString();
                UserData.Password = table.Rows[0][2].ToString();
                UserData.Email = table.Rows[0][3].ToString();
                UserData.Access = Convert.ToBoolean(int.Parse(table.Rows[0][4].ToString()));
                UserData.Name = table.Rows[0][5].ToString();
                UserData.SurName = table.Rows[0][6].ToString();
                UserData.Mobile = table.Rows[0][7].ToString();
                UserData.Bascket.CountProducts = 0;

                CabinetForm newForm = new CabinetForm();
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
            }
            else 
            {
                //не удалось авторизоваться.
                MessageBox.Show("Неверный логин или пароль!");
            }
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
        //подсказка для кнопки на главную.
        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            ToolTip t = new ToolTip();
            t.SetToolTip(pictureBox3, "На главную");
        }

    }
}
