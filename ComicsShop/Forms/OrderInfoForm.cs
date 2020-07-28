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
    public partial class OrderInfoForm : Form
    {
        public OrderInfoForm()
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
        //Перемещение окна .
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
            //очистка корзины
            UserData.Bascket.CountProducts = 0;
            UserData.Bascket.OrderIdCount.Clear();
            UserData.Bascket.sum = 0;
            UserData.Bascket.OrderString = null;
        }
        //кнопка на главную.
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            MainForm newForm = new MainForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
            //очистка корзины
            UserData.Bascket.CountProducts = 0;
            UserData.Bascket.OrderIdCount.Clear();
            UserData.Bascket.sum = 0;
            UserData.Bascket.OrderString = null;
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
                //очистка корзины
                UserData.Bascket.CountProducts = 0;
                UserData.Bascket.OrderIdCount.Clear();
                UserData.Bascket.sum = 0;
                UserData.Bascket.OrderString = null;
            }
            else
            {
                CabinetForm newForm = new CabinetForm();
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
                //очистка корзины
                UserData.Bascket.CountProducts = 0;
                UserData.Bascket.OrderIdCount.Clear();
                UserData.Bascket.sum = 0;
                UserData.Bascket.OrderString = null;
            }
        }

        private void OrderInfoForm_Load(object sender, EventArgs e)
        {
            //отображение номера заказа и суммы в форму.
            label4.Text = UserData.Bascket.sum.ToString()+"$";
            UsersDB db = new UsersDB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `orders` WHERE `order_list` = @ol AND `id_client` =@idc ORDER BY `id_order` DESC", db.getConnection());
            command.Parameters.Add("@ol", MySqlDbType.VarChar).Value = UserData.Bascket.OrderString;
            command.Parameters.Add("@idc", MySqlDbType.VarChar).Value = UserData.ID;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                label2.Text = table.Rows[0][0].ToString();
            }
            else
            {
                MessageBox.Show("Ошибка!");
            }
        }

    }
}
