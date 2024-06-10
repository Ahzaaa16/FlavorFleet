using FlavorFleet.Model;
using FlavorFleet.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlavorFleet
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        static frmMain _obj;

        public static frmMain Instance
        {
            get { if (_obj == null) { _obj = new frmMain(); } return _obj; }
        }


        //add control in mainform

        public   void  AddControls(Form f)
        {
            CenterPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            CenterPanel.Controls.Add(f);
            f.Show();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblUser.Text = MainClass.USER;
            _obj = this;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CenterPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            AddControls(new frmHome());
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnCategories_Click(object sender, EventArgs e)
        {
            AddControls(new frmCategoryView());
        }

        private void BtnTables_Click(object sender, EventArgs e)
        {
            AddControls(new frmTable());
        }

        private void BtnStaff_Click(object sender, EventArgs e)
        {
            AddControls(new frmStaffView());
        }

        private void BtnProducts_Click(object sender, EventArgs e)
        {
            AddControls(new frmProductView());
        }

        private void BtnPOS_Click(object sender, EventArgs e)
        {
            frmPOS frm = new frmPOS();
            frm.Show();
        }

        private void BtnKitchen_Click(object sender, EventArgs e)
        {
            AddControls(new frmKitchenView());
        }

        private void btnExit2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
