using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlavorFleet.Model
{
    public partial class frmPOS : Form
    {
        public frmPOS()
        {
            InitializeComponent();
        }

        public int MainID = 0;
        public string OrderType = "";
        public int? driverID = null;
        public int? detail_id = null;
        public string customerName = "";
        public string customerPhone = "";
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
            AddCategory();

            ProductPanel.Controls.Clear();
            LoadProduct();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AddCategory()
        {
            string qry = "SELECT * FROM category";
            MySqlCommand cmd = new MySqlCommand(qry, MainClass.con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            BtnFood.Controls.Clear();
            BtnDrinks.Controls.Clear();
            BtnDessert.Controls.Clear();

            // Tambahkan event handler untuk tombol "Show All Category"
            btnCat.Click += new EventHandler(btnCat_Click);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.FillColor = Color.FromArgb(7, 54, 46); // Default color
                    b.Size = new Size(248, 53);
                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b.Text = row["name"].ToString();

                    b.Click += new EventHandler(b_Click);

                }
            }
        }

        private void BtnCategory_Click(object sender, EventArgs e, string category)
        {
            // Hanya tampilkan produk dengan kategori yang sesuai
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PCategory.ToLower().Contains(category);
            }
        }

        private void btnCat_Click(object sender, EventArgs e)
        {
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = true;
            }
        }

        private void BtnFood_Click(object sender, EventArgs e)
        {
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PCategory.ToLower().Contains("food");
            }
        }

        private void BtnDrinks_Click(object sender, EventArgs e)
        {
            // Hanya tampilkan produk dengan kategori "Drink"
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PCategory.ToLower().Contains("drink");
            }
        }

        private void BtnDessert_Click(object sender, EventArgs e)
        {
            // Hanya tampilkan produk dengan kategori "Dessert"
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PCategory.ToLower().Contains("snack");
            }
        }

        private void b_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button clickedButton = (Guna.UI2.WinForms.Guna2Button)sender;

            ResetButtonState(BtnFood);
            ResetButtonState(BtnDrinks);
            ResetButtonState(BtnDessert);

            clickedButton.Checked = true;
            clickedButton.FillColor = Color.Transparent;

            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PCategory.ToLower().Contains(clickedButton.Text.Trim().ToLower());
            }
        }

        private void ResetButtonState(Control container)
        {
            foreach (Control ctrl in container.Controls)
            {
                if (ctrl is Guna.UI2.WinForms.Guna2Button button)
                {
                    button.Checked = false;
                    button.FillColor = Color.FromArgb(7, 54, 46);
                }
            }
        }






        private void btnExit_Click_1(object sender, EventArgs e)
        {
            
        }

        private void btnExit_Click_2(object sender, EventArgs e)
        {
            this.Close();
        }


        private void AddItems(string id, string pID, string name, string cat, string price, Image pimage)
        {
            var w = new ucProduct()
            {
                PName = name,
                PPrice = price,
                PCategory = cat,
                PImage = pimage,
                id = Convert.ToInt32(pID),
            };

            ProductPanel.Controls.Add(w);

            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;
                {
                    foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                    {
                        if (Convert.ToInt32(item.Cells["productID"].Value) == wdg.id)
                        {
                            int newQty = Convert.ToInt32(item.Cells["dgvQty"].Value) + 1;
                            item.Cells["dgvQty"].Value = newQty;
                            double newTotal = newQty * Convert.ToDouble(item.Cells["dgvPrice"].Value);
                            item.Cells["dgvAmount"].Value = newTotal;
                            GetTotal();
                            return;
                        }
                    }

                    double itemPrice = Convert.ToDouble(wdg.PPrice);
                    guna2DataGridView1.Rows.Add(new object[] { 0, 0, wdg.id, wdg.PName, 1, itemPrice, itemPrice });
                    GetTotal();
                }
            };
        }


        private void GetTotal()
        {
            double tot = 0;
            txtTotal.Text = "";

            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                tot += double.Parse(item.Cells["dgvAmount"].Value.ToString());
            }
            txtTotal.Text = tot.ToString("N2");
        }


        private void LoadProduct()
        {
            string qry = "SELECT * FROM product INNER JOIN category ON category.id = product.category_id ";
            MySqlCommand cmd = new MySqlCommand(qry, MainClass.con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            foreach (DataRow item in dt.Rows)
            {
                Byte[] imagearray = (byte[])item["pImage"];
                byte[] imagebytearray = imagearray;

                AddItems("0",item["pID"].ToString(), item["pName"].ToString(), item["name"].ToString(),
                    item["pPrice"].ToString(), Image.FromStream(new MemoryStream(imagearray)));
            }
        }

        private void ProductPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach(var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PName.ToLower().Contains(txtSearch.Text.Trim().ToLower());
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
           
        }

        private void guna2DataGridView1_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int count = 0;

            foreach(DataGridViewRow row in guna2DataGridView1.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        

        private void txtTotal_Click(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            guna2DataGridView1.Rows.Clear();
            MainID = 0;
            txtTotal.Text = "0";
        }

        // Tambahkan variabel untuk melacak status tombol
        private bool btnDelClicked = false;

        private void btnDel_Click(object sender, EventArgs e)
        {
            // Membersihkan atau mereset elemen UI
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;

            OrderType = "Delivery";

            frmAddCustomer frm = new frmAddCustomer();
            frm.mainID = MainID;
            frm.OrderType = OrderType;
            MainClass.BlurBackground(frm);

            if (frm.txtName.Text != "")
            {
                driverID = frm.driverID;
                lblDriverName.Text = "Customer Name: " + frm.txtName.Text + " Phone: " + frm.txtPhone.Text + " Driver: " + frm.cbDrive.Text;
                lblDriverName.Visible = true;
                customerName = frm.txtName.Text;
                customerPhone = frm.txtPhone.Text;
            }
        }


        private void btnTake_Click(object sender, EventArgs e)
        {

            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Take Away";

            frmAddCustomer frm = new frmAddCustomer();
            frm.mainID = MainID;
            frm.OrderType = OrderType;  
            MainClass.BlurBackground(frm);

            if (frm.txtName.Text != "")
            {
                driverID = frm.driverID;
                lblDriverName.Text = "Customer Name : " + frm.txtName.Text + " Phone :" + frm.txtPhone.Text;
                lblDriverName.Visible = true;
                customerName = frm.txtName.Text;
                customerPhone = frm.txtPhone.Text;
            }
        }

        private void btnDin_Click(object sender, EventArgs e)
        {
           OrderType = "Din In";
            lblDriverName.Visible= false;
           frmTableSelect frm = new frmTableSelect();
            MainClass.BlurBackground(frm);
            if (frm.TableName != "")
            {
                lblTable.Text = frm.TableName;
                lblTable.Visible=true;
            }
            else
            {
                lblTable.Text = "";
                lblTable.Visible = false;
            }


            frmWaiterSelect frm2 = new frmWaiterSelect();
            MainClass.BlurBackground(frm2);
            if (frm2.waiterName != "")
            {
                lblWaiter.Text = frm2.waiterName;
                lblWaiter.Visible=true;
            }
            else
            {
                lblWaiter.Text = "";
                lblWaiter.Visible = false;
            }
        }

        private void btnKot_Click(object sender, EventArgs e)
        {
            string qry1 = "";
            string qry2 = "";

            int detail_id = 0;

            if (MainID == 0)
            {
                qry1 = "INSERT INTO main (date, time, tables, waiter, status, order_type, total, received, changes, drive_id, custName , custPhone) " +
                       "VALUES (@date, @time, @tables, @waiter, @status, @order_type, @total, @received, @changes, @drive, @custName, @custPhone); " +
                       "SELECT LAST_INSERT_ID();";
            }
            else
            {
                qry1 = "UPDATE main SET date = @date, time = @time, tables = @tables, waiter = @waiter, status = @status, order_type = @order_type, total = @total, received = @received, changes = @changes " +
                       "WHERE id = @ID";
            }

            MySqlCommand cmd = new MySqlCommand(qry1, MainClass.con);
            cmd.Parameters.AddWithValue("@ID", MainID);
            cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("HH:mm:ss"));
            cmd.Parameters.AddWithValue("@tables", lblTable.Text);
            cmd.Parameters.AddWithValue("@waiter", lblWaiter.Text);
            cmd.Parameters.AddWithValue("@status", "Pending");
            cmd.Parameters.AddWithValue("@order_type", OrderType);
            cmd.Parameters.AddWithValue("@total", Convert.ToDouble(txtTotal.Text));
            cmd.Parameters.AddWithValue("@received", 0.0);
            cmd.Parameters.AddWithValue("@changes", 0.0);
            cmd.Parameters.AddWithValue("@drive", driverID);
            cmd.Parameters.AddWithValue("@custName", customerName);
            cmd.Parameters.AddWithValue("@custPhone", customerPhone);

            if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }

            if (MainID == 0)
            {
                MainID = Convert.ToInt32(cmd.ExecuteScalar()); // Use ExecuteScalar to get the last inserted ID
            }
            else
            {
                cmd.ExecuteNonQuery();
            }

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                if (row.Cells["dgvid"].Value == null || row.Cells["dgvid"].Value == DBNull.Value)
                {
                    detail_id = 0;
                }
                else
                {
                    detail_id = Convert.ToInt32(row.Cells["dgvid"].Value);
                }

                if (detail_id == 0)
                {
                    qry2 = "INSERT INTO detail (main_id, product_id, qty, price, amount) VALUES (@MainID, @proID, @qty, @price, @amount)";
                }
                else
                {
                    qry2 = "UPDATE detail SET product_id = @proID, qty = @qty, price = @price, amount = @amount WHERE id = @ID";
                }

                MySqlCommand cmd2 = new MySqlCommand(qry2, MainClass.con);
                cmd2.Parameters.AddWithValue("@ID", detail_id);
                cmd2.Parameters.AddWithValue("@MainID", MainID);
                cmd2.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["productID"].Value));
                cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells["dgvAmount"].Value));

                if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }
                cmd2.ExecuteNonQuery();
                if (MainClass.con.State == ConnectionState.Open) { MainClass.con.Close(); }
            }

            guna2MessageDialog1.Show("Saved Successfully");
            MainID = 0;
            detail_id = 0;
            guna2DataGridView1.Rows.Clear();
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            txtTotal.Text = "00";
            lblDriverName.Text = "";
        }

        public int id  = 0;

        private void btnBill_Click(object sender, EventArgs e)
        {
            frmBillList frm = new frmBillList();
            MainClass.BlurBackground(frm);

            if (frm.MainID >0)
            {
                id = frm.MainID;
                MainID = frm.MainID;
                LoadEntries();
            }
        }

        private void LoadEntries()
        {
            string qry = "SELECT * FROM main INNER JOIN detail ON main.id = detail.main_id INNER JOIN product ON product.pID = detail.product_id WHERE main.id = @id";
            MySqlCommand cmd = new MySqlCommand(qry, MainClass.con);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    string orderType = dt.Rows[0]["order_type"].ToString();
                    if (orderType == "Delivery")
                    {
                        btnDel.Checked = true;
                        lblWaiter.Visible = false;
                        lblTable.Visible = false;
                    }
                    else if (orderType == "Take away")
                    {
                        btnTake.Checked = true;
                        lblWaiter.Visible = false;
                        lblTable.Visible = false;
                    }
                    else
                    {
                        btnDin.Checked = true;
                        lblWaiter.Visible = true;
                        lblTable.Visible = true;
                    }

                    guna2DataGridView1.Rows.Clear();

                    foreach (DataRow item in dt.Rows)
                    {
                        lblTable.Text = item["tables"].ToString();
                        lblWaiter.Text = item["waiter"].ToString();
                        string detailId = item["id"].ToString();
                        string proName = item["pName"].ToString();
                        string proId = item["product_id"].ToString();
                        string qty = item["qty"].ToString();
                        string price = item["price"].ToString();
                        string amount = item["amount"].ToString();

                        object[] obj = { 0, detailId, proId, proName, qty, price, amount };
                        guna2DataGridView1.Rows.Add(obj);
                    }
                    GetTotal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading entries: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            fmCheckout frm = new fmCheckout
            {
                MainID = id,
                amt = Convert.ToDouble(txtTotal.Text)
            };
            MainClass.BlurBackground(frm);

            // Reset state
            MainID = 0;
            guna2DataGridView1.Rows.Clear();
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            txtTotal.Text = "00";
        }


        private void btnHold_Click(object sender, EventArgs e)
        {
            string qry1 = "";
            string qry2 = "";

            int detail_id = 0;

            if(OrderType == "")
            {
                guna2MessageDialog1.Show("Please select order type");
                return;
            }

            if (MainID == 0)
            {
                qry1 = "INSERT INTO main (date, time, tables, waiter, status, order_type, total, received, changes, drive_id, custName , custPhone) " +
                      "VALUES (@date, @time, @tables, @waiter, @status, @order_type, @total, @received, @changes, @drive, @custName, @custPhone); " +
                      "SELECT LAST_INSERT_ID();";
            }
            else
            {
                qry1 = "UPDATE main SET date = @date, time = @time, tables = @tables, waiter = @waiter, status = @status, order_type = @order_type, total = @total, received = @received, changes = @changes " +
                       "WHERE id = @ID";
            }

            MySqlCommand cmd = new MySqlCommand(qry1, MainClass.con);
            cmd.Parameters.AddWithValue("@ID", MainID);
            cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("HH:mm:ss"));
            cmd.Parameters.AddWithValue("@tables", lblTable.Text);
            cmd.Parameters.AddWithValue("@waiter", lblWaiter.Text);
            cmd.Parameters.AddWithValue("@status", "Hold");
            cmd.Parameters.AddWithValue("@order_type", OrderType);
            cmd.Parameters.AddWithValue("@total", Convert.ToDouble(txtTotal.Text));
            cmd.Parameters.AddWithValue("@received", 0.0);
            cmd.Parameters.AddWithValue("@changes", 0.0);
            cmd.Parameters.AddWithValue("@drive", DBNull.Value);
            cmd.Parameters.AddWithValue("@custName", customerName);
            cmd.Parameters.AddWithValue("@custPhone", customerPhone);

            if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }

            if (MainID == 0)
            {
                MainID = Convert.ToInt32(cmd.ExecuteScalar()); // Use ExecuteScalar to get the last inserted ID
            }
            else
            {
                cmd.ExecuteNonQuery();
            }

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                if (row.Cells["dgvid"].Value == null || row.Cells["dgvid"].Value == DBNull.Value)
                {
                    detail_id = 0;
                }
                else
                {
                    detail_id = Convert.ToInt32(row.Cells["dgvid"].Value);
                }

                if (detail_id == 0)
                {
                    qry2 = "INSERT INTO detail (main_id, product_id, qty, price, amount) VALUES (@MainID, @proID, @qty, @price, @amount)";
                }
                else
                {
                    qry2 = "UPDATE detail SET product_id = @proID, qty = @qty, price = @price, amount = @amount WHERE id = @ID";
                }

                MySqlCommand cmd2 = new MySqlCommand(qry2, MainClass.con);
                cmd2.Parameters.AddWithValue("@ID", detail_id);
                cmd2.Parameters.AddWithValue("@MainID", MainID);
                cmd2.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["productID"].Value));
                cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells["dgvAmount"].Value));

                if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }
                cmd2.ExecuteNonQuery();
                if (MainClass.con.State == ConnectionState.Open) { MainClass.con.Close(); }
            }

            guna2MessageDialog1.Show("Saved Successfully");
            MainID = 0;
            detail_id = 0;
            guna2DataGridView1.Rows.Clear();
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            txtTotal.Text = "00";
            lblDriverName.Text = "";
        }
    }
}
