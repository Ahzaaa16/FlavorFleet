using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FlavorFleet.Model
{
    public partial class frmAddCustomer : Form
    {
        public frmAddCustomer()
        {
            InitializeComponent();
        }

        public string OrderType = "";
        public int driverID = 0;
        public string cusName = "";
        public int mainID = 0;
        
        

        private void label_Click(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmAddCustomer_Load(object sender, EventArgs e)
        {
            if( OrderType == "Take Away")
            {
                label.Visible = false;
                cbDrive.Visible = false;
            }
            string qry = "SELECT id 'id' , name  'name'  FROM staff WHERE role = 'driver'";
            MainClass.CBFill(qry, cbDrive);
            if(mainID > 0)
            {
                cbDrive.SelectedValue = driverID;
            }
        }

        private void cbDrive_SelectedIndexChanged(object sender, EventArgs e)
        {
            driverID = Convert.ToInt32(cbDrive.SelectedValue);
        }
    }
}
