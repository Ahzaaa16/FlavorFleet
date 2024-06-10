using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlavorFleet.Model
{
    public partial class frmWaiterSelect : Form
    {
        public frmWaiterSelect()
        {
            InitializeComponent();
        }

        public string waiterName;

        private void frmWaiterSelect_Load(object sender, EventArgs e)
        {
            string qry = "SELECT * FROM staff where role like 'waiter'";
            MySqlCommand cmd = new MySqlCommand(qry, MainClass.con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            foreach (DataRow row in dt.Rows)
            {
                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.Text = row["name"].ToString();
                b.Width = 150;
                b.Height = 50;
                b.ForeColor = Color.FromArgb(254, 164, 6);
                b.FillColor = Color.FromArgb(0, 0, 0);
                b.HoverState.FillColor = Color.FromArgb(255, 164, 6);
                b.HoverState.ForeColor = Color.FromArgb(0, 0, 0);

                b.Click += new EventHandler(b_Click);
                flowLayoutPanel1.Controls.Add(b);

            }
        }

        private void b_Click(object sender, EventArgs e)
        {
            waiterName = (sender as Guna.UI2.WinForms.Guna2Button).Text.ToString();
            this.Close();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
