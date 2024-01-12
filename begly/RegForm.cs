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
    public partial class RegForm : Form
    {
        Form parentForm;
        public RegForm(Form parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Пожалуйста, введите Ваш номер телефона!"); return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Пожалуйста, введите Вашу Фамилию, Имя, Отчество!"); return;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("Пожалуйста, введите Ваш адрес!"); return;
            }
            if (maskedTextBox1.Text == "")
            {
                MessageBox.Show("Вы должны придумать пароль!"); return;
            }
            if (maskedTextBox1.Text != maskedTextBox2.Text)
            {
                MessageBox.Show("Введённые пароли должны совпадать!"); return;
            }
            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.dbConnectionString);
            conn.Open();
            OleDbCommand regCommand = conn.CreateCommand();
            regCommand.CommandText = "INSERT INTO Клиенты (Номер, ФИО, Адрес, Пароль, Скидка) VALUES ('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', '" + maskedTextBox1.Text + "', 0)";
            try
            {
                int regged = Convert.ToInt32(regCommand.ExecuteNonQuery());
                conn.Close();
                MessageBox.Show("Вы успешно зарегистрировались!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Скорее всего, такой пользователь уже существует!\n\n"+ex.ToString());
                conn.Close();
            }
        }

        private void RegForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Show();
        }
    }
}
