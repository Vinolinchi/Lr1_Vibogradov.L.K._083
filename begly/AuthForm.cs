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
    public partial class AuthForm : Form
    {
        public AuthForm()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegForm regForm = new RegForm(this);
            regForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int authed = 0;
            try
            {
                OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.dbConnectionString);
                conn.Open();
                OleDbCommand checkLogin = conn.CreateCommand();
                checkLogin.CommandText = "SELECT Код_клиента FROM Клиенты WHERE Номер = '" + textBox1.Text + "' AND Пароль = '" + maskedTextBox1.Text + "'";
                authed = Convert.ToInt32(checkLogin.ExecuteScalar());
                conn.Close();
            }
            catch
            {

            }
            if (textBox1.Text == Properties.Settings.Default.Логин_администратора && maskedTextBox1.Text == Properties.Settings.Default.Пароль_администратора)
            {
                OrdersForm ordersList = new OrdersForm(this);
                ordersList.Show();
                this.Hide();
            }
            else
            {
                if (authed > 0)
                {
                    CatalogForm catalogForm = new CatalogForm(this, authed);
                    catalogForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Увы, вы ввели неправильный логи или пароль.\nПопытайтесь ещё раз.", "Войти не получилось", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }
    }
}
