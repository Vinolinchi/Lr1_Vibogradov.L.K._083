using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace begly
{
    public partial class CatalogForm : Form
    {
        Form parentForm;
        int user;
        public CatalogForm(Form parentForm, int user)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.user = user;
        }

        private void CatalogForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dbDataSet.Товары". При необходимости она может быть перемещена или удалена.
            this.товарыTableAdapter.Fill(this.dbDataSet.Товары);

            comboBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                MessageBox.Show("Название модели: "+item.Cells[1].Value.ToString()+"\n"+
                                "Стоимость: "+item.Cells[4].Value.ToString()+"\n"+
                                "Характеристики:\n"+item.Cells[3].Value.ToString());
            }
        }

        private void CatalogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Show();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                if (Convert.ToInt32(item.Cells[5].Value) > 0)
                {
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                }
            }
        }

        private void управлениеИнформациейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserConfigForm userConfigForm = new UserConfigForm(this, user);
            userConfigForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                OrderNowForm orderNowForm = new OrderNowForm(this, user, Convert.ToInt32(item.Cells[0].Value));
                orderNowForm.Show();
                this.Hide();
            }
        }

        private void моиЗаказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyOrdersForm myOrdersForm = new MyOrdersForm(this, user);
            myOrdersForm.Show();
            this.Hide();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"Справка.pdf");
        }

        private bool cmpStr<T>(string s, T c1, T c2) where T : IComparable<T>
        {
            int res = c1.CompareTo(c2);

            switch (s)
            {
                case ">":
                    return res > 0;
                case ">=":
                    return res >= 0;
                case "=":
                    return res == 0;
                case "<=":
                    return res <= 0;
                case "<":
                    return res < 0;
                default:
                    throw new ArgumentException();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            товарыBindingSource.SuspendBinding();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Visible = true;
                if (checkBox1.Checked && dataGridView1[1, i].Value.ToString().IndexOf(textBox1.Text) < 0)
                {
                    dataGridView1.Rows[i].Visible = false;
                    continue;
                }
                if (checkBox2.Checked && dataGridView1[1, i].Value.ToString().IndexOf(textBox2.Text) < 0)
                {
                    dataGridView1.Rows[i].Visible = false;
                    continue;
                }
                if (checkBox3.Checked && !cmpStr<long>(comboBox1.Text, Convert.ToInt32(dataGridView1[4, i].Value.ToString()), Convert.ToInt64(numericUpDown1.Value)))
                {
                    dataGridView1.Rows[i].Visible = false;
                    continue;
                }
            }
            товарыBindingSource.ResumeBinding();
        }
    }
}
