using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Npgsql; 

namespace Postgres02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        NpgsqlConnection dbConnection;

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbHome.Checked)
                {
                    dbConnection = new NpgsqlConnection(dbSource.home);
                }
                else
                {
                    dbConnection = new NpgsqlConnection(dbSource.work);
                }

                //create SQL instruction
                string sendSQL = "UPDATE bog.account SET accovr = " + txtNewOvr.Text
                              + " WHERE accid = " + txtAccId.Text;

                //create a command object, using this SQL and connection
                NpgsqlCommand cmd = new NpgsqlCommand(sendSQL, dbConnection);

                //execute the command
                dbConnection.Open();
                int changed = cmd.ExecuteNonQuery();
                dbConnection.Close();

                //provide feedback and tidy up
                label3.Text = changed.ToString() + " rows updated";
                txtAccId.Clear();
                txtNewOvr.Clear();
                txtAccId.Focus();
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString(),"Error");
            }
        }
    }
}
