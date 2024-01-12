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
    public partial class MyOrdersForm : Form
    {
        Form parentForm;
        int user;

        public MyOrdersForm(Form parentForm, int user)
        {
            InitializeComponent();

            this.parentForm = parentForm;
            this.user = user;
        }

        private void executeQuery(string query)
        {
            OleDbConnection conn = null;
            DataTable FullDataTable = new DataTable();
            DataTable ShemaDataTable = new DataTable();
            OleDbDataReader dataReader;
            OleDbCommand myCommand;
            object[] objectRow;
            DataRow myDataRow;
            conn = new OleDbConnection();
            conn.ConnectionString = Properties.Settings.Default.dbConnectionString;
            conn.Open();
            myCommand = conn.CreateCommand();
            myCommand.CommandText = query;
            dataReader = myCommand.ExecuteReader();
            ShemaDataTable = dataReader.GetSchemaTable();
            objectRow = new object[dataReader.FieldCount];
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                FullDataTable.Columns.Add(ShemaDataTable.Rows[i]["ColumnName"].ToString(),
                                    ((System.Type)ShemaDataTable.Rows[i]["DataType"]));
            }
            while (dataReader.Read())
            {
                dataReader.GetValues(objectRow);
                myDataRow = FullDataTable.Rows.Add(objectRow);
            }
            dataGridView1.DataSource = FullDataTable;
            dataReader.Close();
            conn.Close();
        }

        private void MyOrdersForm_Load(object sender, EventArgs e)
        {
            executeQuery("SELECT Заказы.Код_заказа as 'Код заказа', Заказы.Время_заказа as 'Время закзаа', Заказы.Дополнительная_информация as 'Дополнительная информация', Заказы.Количество, Товары.Код_товара as 'Код товара', Товары.Название_товара as 'Название товара', (Товары.Цена*Заказы.Количество - (Товары.Цена*Заказы.Количество*Клиенты.Скидка/100) + ((Товары.Цена*Заказы.Количество - (Товары.Цена*Заказы.Количество*Клиенты.Скидка/100))*"+Properties.Settings.Default.НДС.ToString()+")/100) as Стоимость " +
                         "FROM Клиенты INNER JOIN (Товары INNER JOIN Заказы ON Товары.[Код_товара] = Заказы.[Код_товара]) ON Клиенты.Код_клиента = Заказы.Код_клиента WHERE Заказы.Код_клиента = " + user.ToString()+" AND Заказы.Реализован = true");
        }

        private void MyOrdersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.dbConnectionString);
                conn.Open();
                OleDbCommand checkLogin = conn.CreateCommand();
                checkLogin.CommandText = "SELECT Название_товара, Цена, Полная_информация FROM Товары WHERE Код_товара = " + item.Cells[4].ToString();
                var prodInfo = checkLogin.ExecuteReader();
                while (prodInfo.Read())
                {
                    MessageBox.Show("Название модели: " + prodInfo["Название_товара"] + "\n" +
                                    "Стоимость: " + prodInfo["Цена"] + "\n" +
                                    "Характеристики:\n" + prodInfo["Полная_информация"].ToString());
                }
                prodInfo.Close();
                conn.Close();
            }
        }
    }
}
