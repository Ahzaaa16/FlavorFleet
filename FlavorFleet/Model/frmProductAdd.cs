using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlavorFleet.Model
{
    public partial class frmProductAdd : SampleAdd
    {
        public frmProductAdd()
        {
            InitializeComponent();
        }

        public int id = 0; 
        public int cID = 0;

        private void frmProductAdd_Load(object sender, EventArgs e)
        {
            string qry = "Select id 'id' , name 'name' from category";
            MainClass.CBFill(qry, cbCat);

            if(cID > 0)
            {
                cbCat.SelectedValue = cID;
            }


            if(id > 0)
            {
               ForUpdateLoadData();
            }
        }
        string filepath;
        Byte[] imageByteArray;
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images (*.jpg, *.png)|*.jpg;*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filepath = ofd.FileName;
                try
                {
                    using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                    {
                        txtImg.Image = Image.FromStream(fs);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        public override void btnSave_Click(object sender, EventArgs e)
        {
            string qry = "";

            if (id == 0)
            {
                qry = "INSERT INTO product (pName, pPrice, category_id,pImage) VALUES (@Name, @price, @cat ,@img)";
            }
            else
            {
                qry = "UPDATE product SET pName = @Name, pPrice = @price, category_id = @cat ,  pImage = @img where pID = @id";
            }

            //img

            Image temp = new Bitmap(txtImg.Image);
            MemoryStream ms = new MemoryStream();
            temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            imageByteArray = ms.ToArray();

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txtName.Text);
            ht.Add("@price", txtPrice.Text);
            ht.Add("@cat", Convert.ToInt32(cbCat.SelectedValue));
            ht.Add("@img", imageByteArray);




            if (MainClass.SQL(qry, ht) > 0)
            {
                MessageBox.Show("Saved successfully..");
                id = 0;
                cID = 0;
                txtName.Text = "";
                txtPrice.Text = "";
                cbCat.SelectedIndex = 0;
                cbCat.SelectedIndex = -1;
                txtImg.Image = null;
                txtName.Focus();
            }
        }


        private void ForUpdateLoadData()
        {
            string qry = "SELECT * FROM product where pID = " + id + "";
            MySqlCommand cmd = new MySqlCommand(qry, MainClass.con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            if(dt.Rows.Count > 0)
            {
                txtName.Text = dt.Rows[0]["pName"].ToString();
                txtPrice.Text = dt.Rows[0]["pPrice"].ToString();


                Byte[] imageArray = (byte[])(dt.Rows[0]["pImage"]);
                byte[] imageByteArray = imageArray;
                txtImg.Image = Image.FromStream(new  MemoryStream(imageByteArray));
            }
        }
    }
}
