using Guna.UI2.WinForms;
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

namespace FlavorFleet.Model
{
    public partial class fmCheckout : SampleAdd
    {
        public fmCheckout()
        {
            InitializeComponent();
        }

        private void fmCheckout_Load(object sender, EventArgs e)
        {

        }

        private void fmCheckout_Load_1(object sender, EventArgs e)
        {
            txtBill.Text = amt.ToString();
        }
        public double amt;
        public int MainID = 0;

        private void txtPayment_TextChanged(object sender, EventArgs e)
        {
            double amt = 0;
            double receipt = 0;
            double change = 0;

            double.TryParse(txtBill.Text, out amt);
            double.TryParse(txtPayment.Text, out receipt);


            change = Math.Abs( amt - receipt);

            txtChange.Text = change.ToString();
        }
        public override void btnSave_Click(object sender, EventArgs e)
        {
            string qry = "UPDATE main SET total = @total, received = @rec, changes = @change, status = 'Paid' WHERE id = @id";

            Hashtable ht  = new Hashtable();
            ht.Add("@id", MainID);
            ht.Add("@total" ,txtBill.Text);
            ht.Add("@rec", txtPayment.Text);
            ht.Add("@change", txtChange.Text);

            if(MainClass.SQL(qry, ht)> 0)
            {
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show("Saved Successfull");
                this.Close();
            }
        }

        private void txtBill_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtChange_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
