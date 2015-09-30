using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace HeidaStudy
{
    public partial class FormDonate : DevComponents.DotNetBar.Metro.MetroForm
    {
        public FormDonate()
        {
            InitializeComponent();
        }

        public void Donate()
        {
            this.ShowDialog();
        }
    }
}