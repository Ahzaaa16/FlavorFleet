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
    public partial class frmStaffAdd : SampleAdd
    {
        public frmStaffAdd()
        {
            InitializeComponent();
        }

        public int id = 0;

        private void frmStaffAdd_Load(object sender, EventArgs e)
        {

        }
        public override void btnSave_Click(object sender, EventArgs e)
        {
            string qry = "";

            if (id == 0)
            {
                qry = "INSERT INTO staff (name, phone, role) VALUES (@Name, @phone, @role)";
            }
            else
            {
                qry = "UPDATE staff SET name = @Name, phone = @phone, role = @role WHERE id = @id";
            }

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txtName.Text);
            ht.Add("@phone", txtPhone.Text);
            ht.Add("@role", cbRole.Text);

            // Contoh membersihkan nilai input, Anda mungkin perlu melakukan ini secara lebih komprehensif
            string cleanedName = txtName.Text.Trim();
            string cleanedPhone = txtPhone.Text.Trim();
            string cleanedRole = cbRole.Text.Trim();

            if (MainClass.SQL(qry, ht) > 0)
            {
                MessageBox.Show("Saved successfully..");
                id = 0;
                txtName.Text = "";
                txtPhone.Text = "";
                cbRole.SelectedIndex = -1;
                txtName.Focus();
            }
        }


        private void role_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
