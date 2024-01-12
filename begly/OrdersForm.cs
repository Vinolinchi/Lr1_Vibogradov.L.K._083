using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;

namespace begly
{
    public partial class OrdersForm : Form
    {
        Form parentForm;
        public OrdersForm(Form parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        private void saveChanges()
        {
            заказыBindingSource.EndEdit();
            заказыTableAdapter.Update(dbDataSet);
            dbDataSet.AcceptChanges();
            заказыTableAdapter.Fill(dbDataSet.Заказы);
        }

        private void OrdersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Show();
        }

        private void каталогToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CatalogEditorForm catalogEditorForm = new CatalogEditorForm(this);
            catalogEditorForm.Show();
            this.Hide();
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm(this);
            settingsForm.Show();
            this.Hide();
        }

        private void пользователиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UsersForm usersForm = new UsersForm(this);
            usersForm.Show();
            this.Hide();
        }

        private void OrdersForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dbDataSet.Заказы". При необходимости она может быть перемещена или удалена.
            this.заказыTableAdapter.Fill(this.dbDataSet.Заказы);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.dbConnectionString);
                conn.Open();
                OleDbCommand checkLogin = conn.CreateCommand();
                checkLogin.CommandText = "SELECT Название_товара, Цена, Полная_информация FROM Товары WHERE Код_товара = " + item.Cells[1].Value.ToString();
                var prodInfo = checkLogin.ExecuteReader();
                while (prodInfo.Read())
                {
                    MessageBox.Show("Название модели: " + prodInfo["Название_товара"].ToString() + "\n" +
                                    "Стоимость: " + prodInfo["Цена"].ToString() + "\n" +
                                    "Характеристики:\n" + prodInfo["Полная_информация"].ToString());
                }
                prodInfo.Close();
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.dbConnectionString);
                conn.Open();
                OleDbCommand checkLogin = conn.CreateCommand();
                checkLogin.CommandText = "SELECT * FROM Клиенты WHERE Код_клиента = " + item.Cells[1].Value.ToString();
                var prodInfo = checkLogin.ExecuteReader();
                while (prodInfo.Read())
                {
                    MessageBox.Show("Номер: " + prodInfo["Номер"].ToString() + "\n" +
                                    "ФИО: " + prodInfo["ФИО"].ToString() + "\n" +
                                    "Адрес: " + prodInfo["Адрес"].ToString() +"\n"+
                                    "Персональная скидка: " + prodInfo["Скидка"].ToString());
                }
                prodInfo.Close();
                conn.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.dbConnectionString);
                conn.Open();
                OleDbCommand checkLogin = conn.CreateCommand();
                checkLogin.CommandText = "SELECT Скидка FROM Клиенты WHERE Код_клиента = " + item.Cells[2].Value.ToString();
                var discount = Convert.ToInt32(checkLogin.ExecuteScalar());
                checkLogin.CommandText = "SELECT Цена FROM Товары WHERE Код_товара = " + item.Cells[1].Value.ToString();
                var price = Convert.ToInt64(checkLogin.ExecuteScalar());
                conn.Close();
                double total = Convert.ToDouble(Convert.ToInt32(item.Cells[5].Value.ToString()) * price);
                total = total - (total * Convert.ToDouble(discount)) / Convert.ToDouble(100);
                total = total + (total * Convert.ToDouble(Properties.Settings.Default.НДС)) / Convert.ToDouble(100);
                MessageBox.Show( total.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить это?\nЭто действие невозможно отменить.", "Подтвердите удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    заказыBindingSource.RemoveAt(item.Index);
                }
                saveChanges();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox1.Checked;
            foreach (var control in Controls.Cast<Control>())
            {
                foreach (var binding in control.DataBindings.Cast<Binding>())
                {
                    binding.WriteValue();
                }
            }
            saveChanges();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"Справка.pdf");
        }
    }
}
