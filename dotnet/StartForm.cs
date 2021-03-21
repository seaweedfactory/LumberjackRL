using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LumberjackRL.Core.UI;
using LumberjackRL.Core;

namespace LumberjackRL
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            Quinoa gameObject = new Quinoa();
            gameObject.getUI().getForm().ShowDialog(this);
            Close();
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            btnPlay.PerformClick();
        }
    }
}
