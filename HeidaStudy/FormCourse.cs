using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Resources;
using System.Reflection;

namespace HeidaStudy
{
    public partial class FormCourse : DevComponents.DotNetBar.Metro.MetroForm
    {
        string[] strSelected = new string[2];

        public FormCourse()
        {
            InitializeComponent();
        }

        public string[] SelectCourse()
        {
            InitCourse();
            this.ShowDialog();
            return strSelected;
        }

        private void InitCourse()
        {
            string str = Properties.Resources.course;
            string[] lines = str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string[] strs = line.Split(',');
                ListBoxItem item = new ListBoxItem();
                item.Name = strs[0];
                item.Text = strs[1];
                lstCourse.Items.Add(item);
            }
        }

        private void lstCourse_DoubleClick(object sender, EventArgs e)
        {
            Submit();
        }

        private void Submit()
        {
            if (lstCourse.SelectedItems.Count > 0)
            {
                ListBoxItem item = lstCourse.SelectedItems[0];
                strSelected[0] = item.Name;
                strSelected[1] = item.Text;
                this.Close();
            }
        }

        private void FormCourse_Load(object sender, EventArgs e)
        {
            lstCourse.SelectedIndex = 0;
        }

    }
}