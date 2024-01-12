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
    public partial class SettingsForm : Form
    {
        Form parentForm;
        public SettingsForm(Form parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.Логин_администратора;
            textBox2.Text = Properties.Settings.Default.Пароль_администратора;
            numericUpDown2.Value = Convert.ToDecimal(Properties.Settings.Default.НДС);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Логин_администратора = textBox1.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Успешно сохранено!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Пароль_администратора = textBox2.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Успешно сохранено!");
        }

        private void button3_Click(object sender, EventArgs e)
        {

            Properties.Settings.Default.НДС = Convert.ToInt32(numericUpDown2.Value);
            Properties.Settings.Default.Save();
            MessageBox.Show("Успешно сохранено!");
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Show();
        }
    }
}
