using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;




namespace EmployeeTracker

{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
                Form1 f1 = new Form1();
                f1.Show();
                this.Hide();
               
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.psswrdtextbox.PasswordChar = this.checkBox1.Checked ? char.MinValue : '*';
        }
    }
}
