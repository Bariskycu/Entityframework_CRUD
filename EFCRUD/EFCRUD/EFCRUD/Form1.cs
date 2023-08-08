using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFCRUD
{
    public partial class Form1 : Form
    {
        Product model = new Product();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            txtId.Text = txtName.Text = txtQuantityPerUnit.Text = txtUnitPrice.Text = txtUnitsInStock.Text = txtUnitsOnOrder.Text = txtReordersLevel.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.ProductID = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.ProductName = txtName.Text.Trim();
            model.QuantityPerUnit = txtQuantityPerUnit.Text.Trim();
            model.UnitPrice = Convert.ToDecimal(txtUnitPrice.Text.Trim());
            model.UnitsInStock = Convert.ToInt16(txtUnitsInStock.Text.Trim());
            model.UnitsOnOrder = Convert.ToInt16(txtUnitsOnOrder.Text.Trim());
            model.ReorderLevel = Convert.ToInt16(txtReordersLevel.Text.Trim());
            using (NorthwindEntities db =new NorthwindEntities())
            {
                if (model.ProductID == 0)
                    db.Products.Add(model);
                else
                    db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            Clear();
            PopulateDataGridView();
            MessageBox.Show("Başarıyla Kaydedildi");
        }

        void PopulateDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            using (NorthwindEntities db =new NorthwindEntities())
            {
                dataGridView1.DataSource = db.Products.ToList<Product>();
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow.Index != -1)
            {
                model.ProductID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ProductID"].Value);
                using (NorthwindEntities db =new NorthwindEntities())
                {
                    model = db.Products.Where(x => x.ProductID == model.ProductID).FirstOrDefault();
                    model.ProductName = txtName.Text;
                    model.QuantityPerUnit = txtQuantityPerUnit.Text;
                    model.UnitPrice = Convert.ToInt32(txtUnitPrice.Text);
                    model.UnitsOnOrder = Convert.ToInt16(txtUnitsOnOrder.Text);
                    model.ReorderLevel = Convert.ToInt16(txtReordersLevel.Text);
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Kaydı Silmek İstiyormusunuz?","EF CRUD Operation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (NorthwindEntities db =new NorthwindEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.Products.Attach(model);
                    db.Products.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Clear();
                    MessageBox.Show("Kayıt Silindi");
                }
            }
        }
    }
}
