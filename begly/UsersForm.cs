using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace begly
{
    public partial class UsersForm : Form
    {
        Form parentForm;
        public UsersForm(Form parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dbDataSet.Клиенты". При необходимости она может быть перемещена или удалена.
            this.клиентыTableAdapter.Fill(this.dbDataSet.Клиенты);

        }

        private void saveChanges()
        {
            клиентыBindingSource.EndEdit();
            клиентыTableAdapter.Update(dbDataSet);
            dbDataSet.AcceptChanges();
            клиентыTableAdapter.Fill(dbDataSet.Клиенты);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (Control c in editPanel.Controls)
                {
                    foreach (Binding b in c.DataBindings)
                    {
                        b.WriteValue();
                    }
                }
                saveChanges();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить это?\nЭто действие невозможно отменить.", "Подтвердите удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    клиентыBindingSource.RemoveAt(item.Index);
                }
                saveChanges();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbDataSet.Клиенты.AddКлиентыRow(textBox4.Text, textBox8.Text, textBox5.Text, textBox6.Text, Convert.ToInt32(numericUpDown1.Value));
            this.saveChanges();
        }

        private void UsersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Show();
        }
    }
}
