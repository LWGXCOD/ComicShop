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
    public partial class OrderForm : Form
    {
        public OrderForm()
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
        //кнопка на гавную
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MainForm newForm = new MainForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //кнопка отмена, возвращает пользователя в форму корзина.
        private void button2_Click(object sender, EventArgs e)
        {
            Forms.BasketForm newForm = new BasketForm();
            newForm.Left = this.Left;
            newForm.Top = this.Top;
            newForm.Show();
            this.Hide();
        }
        //при загрузке формы отображает сумму заказа в label2.Text .
        private void OrderForm_Load(object sender, EventArgs e)
        {
            label2.Text = UserData.Bascket.sum.ToString() + "$";
        }
        //кнопка оформления заказа
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string orderstring  = UserData.Bascket.GetOrder();
            
            UsersDB db = new UsersDB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command;

            int[] countP = new int[UserData.Bascket.CountProducts];
            int i = 0;
            //проверка на наличие достаточного колличества товаров.
            foreach (KeyValuePair<int, int> kvp in UserData.Bascket.OrderIdCount)
            {
                command = new MySqlCommand("SELECT * FROM `comics` WHERE `id_c` = @id", db.getConnection());
                command.Parameters.Add("@id", MySqlDbType.VarChar).Value = kvp.Key;
                db.openConnection();

                
                
                adapter.SelectCommand = command;
                adapter.Fill(table);
                if (int.Parse(table.Rows[0][5].ToString()) < kvp.Value)
                {
                    //при недостаточном количестве вывод сообщения о том что товара нет.
                    MessageBox.Show("Больше нет достаточного количества выбраных вами товара " + table.Rows[0][2].ToString() + ".");
                    return;
                }
                else
                {
                    countP[i] = int.Parse(table.Rows[0][5].ToString());
                    i++;
                }
                
               
            }
            i = 0;
            //уменьшение количества товаров в таблице comics на заказанное число.
            foreach (KeyValuePair<int, int> kvp in UserData.Bascket.OrderIdCount)
            {
                command = new MySqlCommand("UPDATE `comics` SET  `count` = @count WHERE `id_c` = @id", db.getConnection());
                command.Parameters.Add("@id", MySqlDbType.VarChar).Value = kvp.Key;
                command.Parameters.Add("@count", MySqlDbType.VarChar).Value = countP[i]-kvp.Value;

                db.openConnection();

                if (command.ExecuteNonQuery() == 1) ;
                else
                    MessageBox.Show("Ошибка.");
            }
            //создание новой записи в таблице orders.
            command = new MySqlCommand("INSERT INTO `orders`(`order_list`, `id_client`, `price`) VALUES (@orstr, @idc, @p)", db.getConnection());
            command.Parameters.Add("@orstr", MySqlDbType.VarChar).Value = orderstring;
            command.Parameters.Add("@idc", MySqlDbType.Int32).Value = UserData.ID;
            command.Parameters.Add("@p", MySqlDbType.Int32).Value = UserData.Bascket.sum;

            db.openConnection();



            if (command.ExecuteNonQuery() == 1)
            {
                Forms.OrderInfoForm newForm = new OrderInfoForm();
                newForm.Left = this.Left;
                newForm.Top = this.Top;
                newForm.Show();
                this.Hide();
            }
            else
                MessageBox.Show("Ошибка!");

            db.closeConnection();
           
        }
    }
}
