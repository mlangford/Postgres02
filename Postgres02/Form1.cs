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

        private void Form1_Load(object sender, EventArgs e)
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
                string sendSQL = "SELECT accid FROM bog.account";

                //create a command object, using this SQL and connection
                NpgsqlCommand cmd = new NpgsqlCommand(sendSQL, dbConnection);

                //execute the command
                dbConnection.Open();
                NpgsqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lbAccounts.Items.Add(dr["accid"].ToString());
                }

                if (lbAccounts.Items.Count > 0)
                    lbAccounts.SelectedIndex = 0;

                dbConnection.Close();
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString(), "Error");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                float newOver;
                if (!float.TryParse(txtNewOvr.Text, out newOver))
                {
                    MessageBox.Show("Invalid Overdraft value");
                    txtNewOvr.Clear();
                    txtNewOvr.Focus();
                    return;
                }
                if (newOver < 0.0F)
                {
                    MessageBox.Show("Invalid Overdraft value");
                    txtNewOvr.Clear();
                    txtNewOvr.Focus();
                    return;
                }
                //create SQL instruction
                string sendSQL = "UPDATE bog.account SET accovr = @newOD"
                              + " WHERE accid = @acc";

                //create a command object, using this SQL and connection
                NpgsqlCommand cmd = new NpgsqlCommand(sendSQL, dbConnection);
                
                //fill the parameter placeholders
                cmd.Parameters.AddWithValue("newOD", newOver);
                cmd.Parameters.AddWithValue("acc", Convert.ToInt32(lbAccounts.SelectedItem));

                //execute the command
                dbConnection.Open();
                int changed = cmd.ExecuteNonQuery();
                dbConnection.Close();

                //provide feedback and tidy up
                label3.Text = changed.ToString() + " rows updated";

                txtNewOvr.Clear();

            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString(),"Error");
            }
        }

    }
}
