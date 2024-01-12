using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace begly
{
    public partial class OrderNowForm : Form
    {
        Form parentForm;
        int user;
        int product;

        long price;
        int personalDiscount;

        public OrderNowForm(Form parentForm, int user, int product)
        {
            InitializeComponent();

            this.parentForm = parentForm;
            this.user = user;
            this.product = product;
        }

        private void calc()
        {
            double total = Convert.ToDouble(numericUpDown1.Value*price);
            total = total - (total*Convert.ToDouble(personalDiscount))/Convert.ToDouble(100);
            total = total + (total * Convert.ToDouble(Properties.Settings.Default.НДС)) / Convert.ToDouble(100);
            textBox3.Text = total.ToString();
        }

        private void OrderNowForm_Load(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.dbConnectionString);
            conn.Open();
            OleDbCommand checkLogin = conn.CreateCommand();
            checkLogin.CommandText = "SELECT Скидка FROM Клиенты WHERE Код_клиента = " + user.ToString();
            personalDiscount = Convert.ToInt32(checkLogin.ExecuteScalar());

            checkLogin.CommandText = "SELECT Название_товара, Цена, Количество_на_складе FROM Товары WHERE Код_товара = " + product.ToString();
            var prodInfo = checkLogin.ExecuteReader();
            while (prodInfo.Read())
            {
                price = Convert.ToInt64(prodInfo["Цена"]);
                numericUpDown1.Maximum = Convert.ToInt32(prodInfo["Количество_на_складе"]);
                textBox1.Text = prodInfo["Название_товара"].ToString();
            }
            prodInfo.Close();
            conn.Close();

            personalDiscountLabel.Text = personalDiscountLabel.Text.Replace("**", Properties.Settings.Default.НДС.ToString());
            personalDiscountLabel.Text = personalDiscountLabel.Text.Replace("*", personalDiscount.ToString());

            calc();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            calc();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.dbConnectionString);
            conn.Open();
            OleDbCommand comm = conn.CreateCommand();
            comm.CommandText = "INSERT INTO Заказы(Код_клиента, Код_товара, Время_заказа, Дополнительная_информация, Количество, Реализован) VALUES ("+
                                user.ToString()+", "+product.ToString()+", NOW(), '"+textBox2.Text+"', "+numericUpDown1.Value.ToString()+", false)";
            var info = comm.ExecuteNonQuery();
            if (info > 0)
            {
                MessageBox.Show("Заказ добавлен!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Увы, что-то пошло не так!");
            }
            conn.Close();
        }

        private void OrderNowForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Show();
        }
    }
}
