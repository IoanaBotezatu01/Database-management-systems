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
using System.Configuration;

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

       

        public Form1()
        {
            InitializeComponent();
            this.dataGridView1.SelectionMode= DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.SelectionMode= DataGridViewSelectionMode.FullRowSelect;
            FillData();
            this.button1.Click+=new EventHandler(this.button1_Click);
        }
        void FillData()
        {
            
            conn = new SqlConnection(getConnectionString());
            ds = new DataSet();
           
           
            daShops = new SqlDataAdapter(getParentQuery(), conn);
            SqlCommandBuilder cb = new SqlCommandBuilder(daShops);
            daShops.Fill(ds, getParentTable());

            daCoffeeTables = new SqlDataAdapter(getChildQuery(), conn);
            cb = new SqlCommandBuilder(daCoffeeTables);
            daCoffeeTables.Fill(ds, getChildTable());

            DataRelation drel = new DataRelation("fk_child_parent",
                ds.Tables[getParentTable()].Columns[getPKName()],
                ds.Tables[getChildTable()].Columns[getFKName()]);
            ds.Relations.Add(drel);


           
            bsShops = new BindingSource();
            bsShops.DataSource = ds;
            bsShops.DataMember = getParentTable();

            bsCoffeeTables = new BindingSource();
            bsCoffeeTables.DataSource = bsShops;
            bsCoffeeTables.DataMember = "fk_child_parent";

            //cmdBuilder.GetUpdateCommand();
            dataGridView1.DataSource=bsShops;
            dataGridView2.DataSource=bsCoffeeTables;    

        }

        private string getPKName()
        {
            return ConfigurationManager.AppSettings["parent_table_pk"];
        }

        private string getFKName()
        {
            return ConfigurationManager.AppSettings["child_table_fk"];
        }

        private string getParentTable()
        {
            return ConfigurationManager.AppSettings["parent_table"];
        }

        private string getParentQuery()
        {
            return ConfigurationManager.AppSettings["parentQuery"];
        }

        private string getChildTable()
        {
            return ConfigurationManager.AppSettings["child_table"];
        }
        private string getChildQuery()
        {
            return ConfigurationManager.AppSettings["childQuery"];
        }

        private void button1_Click(object sender,EventArgs e)
        {

            this.daShops.Update(this.ds, getParentTable());
            this.daCoffeeTables.Update(this.ds, getChildTable());

        }

        string getConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["connection_string"].ConnectionString.ToString();
        }
      
    }
}