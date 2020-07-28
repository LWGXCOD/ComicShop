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
    public partial class ChangePassForm : Form
    {
        public ChangePassForm()
        {
            InitializeComponent();
        }
        //закрытие приложения
        private void exitLabel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //перекрашивание крестика
        private void exitLabel_MouseEnter(object sender, EventArgs e)
        {
            exitLabel.ForeColor = Color.FromArgb(80, 80, 80);
        }

        private void exitLabel_MouseLeave(object sender, EventArgs e)
        {
            exitLabel.ForeColor = Color.FromArgb(0, 0, 0);
        }
        //кнопка личный кабинет
        private void button2_Click(object sender, EventArgs e)
        {
            CabinetForm newForm = new CabinetForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //перемещение формы за верхнюю панель.
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
        //кнопка изменить пароль
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            //обработка пустого ввода.
            if (OldPassField.Text == "")
            {
                MessageBox.Show("Введите Пароль");
                return;
            }
            if (NewPassField.Text == "")
            {
                MessageBox.Show("Введите новый Пароль");
                return;
            }
            if (NewPassConfirmField.Text == "")
            {
                MessageBox.Show("Подтвердите новый Пароль");
                return;
            }
            //обработка неверно введённого пароля.
            if (UserData.Password != OldPassField.Text)
            {
                MessageBox.Show("Неверный пароль!");
            }
            //обработка несовпадения новых паролей.
            else if (NewPassField.Text != NewPassConfirmField.Text)
            {
                MessageBox.Show("Пароли не совпадают!");
            }
            else 
            {
                //Сохнанение нового пароля в БД
                UserData.Password = NewPassField.Text;

                UsersDB db = new UsersDB();
                MySqlCommand command = new MySqlCommand("UPDATE `users` SET  `password` = @password WHERE `id` = @id", db.getConnection());
                command.Parameters.Add("@id", MySqlDbType.VarChar).Value = UserData.ID;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = UserData.Password;

                db.openConnection();

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Пароль успешно изменен!");

                    CabinetForm newForm = new CabinetForm();
                    newForm.Left = this.Left;
                    newForm.Top = this.Top;
                    newForm.Show();
                    this.Hide();
                }
                else
                    MessageBox.Show("Ошибка.");

                db.closeConnection();
            }
        }
    }
}
