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
    public partial class UserConfigForm : Form
    {
        Form parentForm;
        int user;
        public UserConfigForm(Form parentForm, int user)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.user = user;
        }

        private void UserConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Show();
        }

        private void UserConfigForm_Load(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.dbConnectionString);
            conn.Open();
            OleDbCommand checkLogin = conn.CreateCommand();
            checkLogin.CommandText = "SELECT Номер, Пароль, Скидка, ФИО, Адрес FROM Клиенты WHERE Код_клиента = " + user.ToString();
            var info = checkLogin.ExecuteReader();
            while (info.Read())
            {
                textBox1.Text = info["Номер"].ToString();
                textBox2.Text = info["ФИО"].ToString();
                textBox3.Text = info["Адрес"].ToString();
                textBox4.Text = info["Скидка"].ToString();
            }
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.dbConnectionString);
            conn.Open();
            OleDbCommand comm = conn.CreateCommand();
            comm.CommandText = "UPDATE Клиенты SET ФИО = '"+textBox2.Text+"', Адрес = '"+textBox3.Text+"' WHERE Код_клиента = " + user.ToString();
            var info = comm.ExecuteNonQuery();
            if (info>0) {
                MessageBox.Show("Данные успешно обновлены!");
            } else {
                MessageBox.Show("Увы, что-то пошло не так!");
            }
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text == maskedTextBox2.Text)
            {
                OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.dbConnectionString);
                conn.Open();
                OleDbCommand comm = conn.CreateCommand();
                comm.CommandText = "UPDATE Клиенты SET Пароль = '" + maskedTextBox1.Text + "' WHERE Код_клиента = " + user.ToString();
                var info = comm.ExecuteNonQuery();
                if (info > 0)
                {
                    MessageBox.Show("Пароль успешно изменён!");
                }
                else
                {
                    MessageBox.Show("Увы, что-то пошло не так!");
                }
                conn.Close();
            }
            else
            {
                MessageBox.Show("Введённые пароли должны совпадать!");
            }
        }
    }
}
