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
    public partial class CatalogEditorForm : Form
    {
        Form parentForm;

        public CatalogEditorForm(Form parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        private void CatalogEditor_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dbDataSet.Товары". При необходимости она может быть перемещена или удалена.
            this.товарыTableAdapter.Fill(this.dbDataSet.Товары);

        }

        private void CatalogEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Show();
        }

        private void saveChanges()
        {
            товарыBindingSource.EndEdit();
            товарыTableAdapter.Update(dbDataSet);
            dbDataSet.AcceptChanges();
            товарыTableAdapter.Fill(dbDataSet.Товары);
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
                    товарыBindingSource.RemoveAt(item.Index);
                }
                saveChanges();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbDataSet.Товары.AddТоварыRow(textBox6.Text, textBox5.Text, textBox4.Text, Convert.ToInt32(numericUpDown4.Value), Convert.ToInt32(numericUpDown3.Value));
            this.saveChanges();
        }
    }
}
