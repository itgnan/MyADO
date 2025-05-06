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

namespace WindowsFormsApp1
{
    public partial class frmBank : Form
    {
        string connectionString = string.Empty;
        public frmBank()
        {
            connectionString = "Server=(localDb)\\MSSqlLocalDB; Database=BRPV;Trusted_Connection=True";
            InitializeComponent();
        }

        private void frmBank_Load(object sender, EventArgs e)
        {
            loadCustomers();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand($"insert into bank(custid, custname, balance) values ( {txtId.Text}, '{txtName.Text}', {txtBalance.Text} )");
            cmd.Connection = sqlConnection;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();  // it is some thing update delete create // Execute Reader 
            cmd.Connection.Close();

            loadCustomers();
            MessageBox.Show("Record Created Successfully");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // What is use of Using Statement 
            // How we are going to use the parameterized Querys // SQl Injection Attacks
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string updateQuery = "update bank set custname= @Name, balance = @balance where custid = @id";
                using (SqlCommand cmd = new SqlCommand(updateQuery, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@id", txtId.Text);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@balance", txtBalance.Text);
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                    connectionString.Clone();
                }
            }
            loadCustomers();
            MessageBox.Show("Record Updated Successfully");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // What is use of Using Statement 
            // How we are going to use the parameterized Querys // SQl Injection Attacks
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string updateQuery = "delete from bank where custid = @id";
                using (SqlCommand cmd = new SqlCommand(updateQuery, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@id", txtId.Text);
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                    connectionString.Clone();
                }
            }
            loadCustomers();
            MessageBox.Show("Record Deleted Successfully");
        }

        private void loadCustomers()
        {
            string selectQuery = "select custid, custname, balance from bank";
            SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connectionString);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            gvCustomer.DataSource = dt;
        }
    }
}

