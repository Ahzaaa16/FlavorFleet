using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlavorFleet.View
{
    public partial class frmKitchenView : Form
    {
        public frmKitchenView()
        {
            InitializeComponent();
        }

        private void frmKitchenView_Load(object sender, EventArgs e)
        {
            GetOrder();
        }

        private void GetOrder()
        {
            flowLayoutPanel1.Controls.Clear();
            string qry1 = "SELECT * FROM main where status = 'Pending'";
            MySqlCommand cmd1 = new MySqlCommand(qry1, MainClass.con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd1);
            DataTable dt1 = new DataTable();
            da.Fill(dt1);

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                FlowLayoutPanel p1 = new FlowLayoutPanel();
                p1.AutoSize = true;
                p1.Width = 250;
                p1.Height = 350;
                p1.FlowDirection = FlowDirection.TopDown;
                p1.BorderStyle = BorderStyle.FixedSingle;
                p1.Margin = new Padding(10, 10, 10, 10);

                FlowLayoutPanel p2 = new FlowLayoutPanel();
                p2.AutoSize = true;
                p2.BackColor = Color.FromArgb(0, 0, 0);
                p1.Width = 250;
                p1.Height = 125;
                p2.FlowDirection = FlowDirection.TopDown;
                p2.Margin = new Padding(0, 0, 0, 0);

                Label lb1 = new Label();
                lb1.ForeColor = Color.White;
                lb1.Margin = new Padding(10, 10, 3, 0);
                lb1.AutoSize = true;
                lb1.Text = "Table: " + dt1.Rows[i]["tables"].ToString();

                Label lb2 = new Label();
                lb2.ForeColor = Color.White;
                lb2.Margin = new Padding(10, 5, 3, 0);
                lb2.AutoSize = true;
                lb2.Text = "Waiter name: " + dt1.Rows[i]["waiter"].ToString();

                Label lb3 = new Label();
                lb3.ForeColor = Color.White;
                lb3.Margin = new Padding(10, 5, 3, 0);
                lb3.AutoSize = true;
                lb3.Text = "Order Time: " + dt1.Rows[i]["time"].ToString();

                Label lb4 = new Label();
                lb4.ForeColor = Color.White;
                lb4.Margin = new Padding(10, 5, 3, 10);
                lb4.AutoSize = true;
                lb4.Text = "Order Type: " + dt1.Rows[i]["order_type"].ToString();

                p2.Controls.Add(lb1);
                p2.Controls.Add(lb2);
                p2.Controls.Add(lb3);
                p2.Controls.Add(lb4);
                p1.Controls.Add(p2);


                int mid = 0;

                mid = Convert.ToInt32(dt1.Rows[i]["id"].ToString());

                string qry2 = "SELECT * FROM main INNER JOIN detail ON main.id = detail.main_id INNER JOIN product ON product.pID = detail.product_id WHERE main.id = " + mid + "";
                MySqlCommand cmd2 = new MySqlCommand(qry2, MainClass.con);
                MySqlDataAdapter da2 = new MySqlDataAdapter(cmd2);
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);


                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    Label lb5 = new Label();
                    lb5.ForeColor = Color.Black;
                    lb5.Margin = new Padding(10, 5, 3, 0);
                    lb5.AutoSize = true;

                    int no = j + 1;

                    lb5.Text = $"{no}. {dt2.Rows[j]["pName"].ToString()} - {dt2.Rows[j]["qty"].ToString()}";

                    p1.Controls.Add(lb5);
                }

                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.AutoRoundedCorners = true;
                b.Size = new Size(100,35);
                b.ForeColor = Color.FromArgb(0, 0, 0);
                b.FillColor = Color.FromArgb(255, 164, 6);
                b.Margin = new Padding(30,5,3,10);
                b.Text = "Complete";
                b.Tag = dt1.Rows[i]["id"].ToString();

                b.Click += new EventHandler(b_click);
                p1.Controls.Add(b);


                flowLayoutPanel1.Controls.Add(p1);
            }
        }

        private void b_click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as Guna.UI2.WinForms.Guna2Button).Tag.ToString());
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            if (guna2MessageDialog1.Show("Are you sure you want to delete?") == DialogResult.Yes)
            {
                string qry = "UPDATE main SET status  =  'Complate' where  id = @ID";
                Hashtable ht = new Hashtable();
                ht.Add("@ID", id);

                if(MainClass.SQL(qry,ht)> 0)
                {
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Show("Saved Successfully");
                }
                GetOrder();
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
