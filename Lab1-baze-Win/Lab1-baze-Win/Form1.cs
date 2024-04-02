using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1_baze_Win
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        SqlDataAdapter daShops;
        SqlDataAdapter daCoffeeTables;
        DataSet ds;
        BindingSource bsShops;
        BindingSource bsCoffeeTables;

        SqlCommandBuilder cmdBuilder;
        string queryShops;
        string queryCoffeeTables;

        public Form1()
        {
            InitializeComponent();
            this.dataGridView1.SelectionMode= DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.SelectionMode= DataGridViewSelectionMode.FullRowSelect;
            //InitializeDataGridViews();
            FillData();
            this.button1.Click+=new EventHandler(this.button1_Click);
        }
        void FillData()
        {
            conn = new SqlConnection(getConnectionString());
            queryShops = "Select * From Shops";
            queryCoffeeTables = "Select * From CoffeeTables";
            daShops = new SqlDataAdapter(queryShops, conn);
            daCoffeeTables = new SqlDataAdapter(queryCoffeeTables, conn);
            ds = new DataSet();
            daShops.Fill(ds, "Shops");
            daCoffeeTables.Fill(ds, "CoffeeTables");
            cmdBuilder = new SqlCommandBuilder(daCoffeeTables);

            ds.Relations.Add("ShopsTables",
                ds.Tables["Shops"].Columns["shop_id"],
                ds.Tables["CoffeeTables"].Columns["shop_id"]);

            bsShops = new BindingSource();
            bsShops.DataSource = ds.Tables["Shops"];
            bsCoffeeTables = new BindingSource(bsShops, "ShopsTables");
            this.dataGridView1.DataSource = bsShops;
            this.dataGridView2.DataSource = bsCoffeeTables;

            cmdBuilder.GetUpdateCommand();

        }
        private void button1_Click(object sender,EventArgs e)
        {
           
            daCoffeeTables.Update(ds, "CoffeeTables");
            
        }

        string getConnectionString()
        {
            return "Data Source=DESKTOP-6I5B5HJ;Initial Catalog=lab2-coffee-Shop-F;" + "Integrated Security=true;";
        }
        //void InitializeDataGridViews()
        //{
        //    dataGridView1 = new DataGridView();
        //    dataGridView2 = new DataGridView();

        //    // Set properties for dataGridView1 and dataGridView2 if needed
        //    dataGridView1.Dock = DockStyle.Left;
        //    dataGridView2.Dock = DockStyle.Right;

        //    // Add them to the form's controls collection
        //    this.Controls.Add(dataGridView1);
        //    this.Controls.Add(dataGridView2);
        //}
    }
}