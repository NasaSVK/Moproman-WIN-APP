using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Data;
using MySql.Data.MySqlClient;
using MySql.Data;

using System.IO;
/// <summary>
/// TUBEXOVA MYSQL DB - V TOMTO PROJEKTE NEPOUZITA
/// </summary>

//<value>\\10.0.0.6\technik\AktualProject\Tubex_FOTO\Photo Logging Camera System\ftpcam</value>
namespace nsAspur
{
    class MySqlDb
    {
    private MySqlConnection connection;
    private string server;
    private string database;
    private string uid;
    private string password;
    public dlgLoguj Loguj;

    //Constructor
    public void DBConnect(dlgLoguj pLoguj)
    {
        Initialize();
        this.Loguj = pLoguj;
    }

    //Initialize values
    private void Initialize()
    {
        //server = "10.50.50.3";
        //server  = Properties.Settings.Default.IPadd;
        database = "tubex_photo_db";
        uid = "root";
        password = "Tubex1234!";
        string connectionString;
        connectionString = "SERVER=" + server + ";" + "DATABASE=" + 
		database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        connection = new MySqlConnection(connectionString);
    }

    //open connection to database
    private bool OpenConnection()
    {
        try
        {
            connection.Open();
            return true;
        }
        catch (MySqlException ex)
        {
            //When handling errors, you can your application's response based 
            //on the error number.
            //The two most common error numbers when connecting are as follows:
            //0: Cannot connect to server.
            //1045: Invalid user name and/or password.
            switch (ex.Number)
            {
                case 0:
                    MessageBox.Show("Cannot connect to server.  Contact administrator");
                    break;

                case 1045:
                    MessageBox.Show("Invalid username/password, please try again");
                    break;
            }
            return false;
        }
    }

    //Close connection
    private bool CloseConnection()
    {
        try
        {
            connection.Close();
            return true;
        }
        catch (MySqlException ex)
        {
            //MessageBox.Show(ex.Message);
            this.Loguj(ex.Message, MessageBoxIcon.Error);                
            return false;
        }
    }


        //#####################################################################################################################
        //#####################################################################################################################        
        //Insert statement
       public void Insert(string pKod, string pPhotoPath)  //table: 1-frontleft 2-frontright 3-rear 4-repas
       {
            try
            {
                byte[] ImageData = null;

                FileStream fs = new FileStream(pPhotoPath, FileMode.Open, FileAccess.Read);

                BinaryReader br = new BinaryReader(fs);

                ImageData = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();

                string CmdString = "INSERT INTO photo_record(kod, photo, datumcas) VALUES(@kod, @photo, @datumcas)";


                MySqlCommand cmd = new MySqlCommand(CmdString, connection);

                cmd.Parameters.Add("@kod", MySqlDbType.VarChar);
                cmd.Parameters.Add("@photo", MySqlDbType.MediumBlob);
                cmd.Parameters.Add("@datumcas", MySqlDbType.DateTime);

                cmd.Parameters["@kod"].Value = pKod;
                cmd.Parameters["@photo"].Value = ImageData;
                cmd.Parameters["@datumcas"].Value = DateTime.Now;


                connection.Open();

                int RowsAffected = cmd.ExecuteNonQuery();

                if (RowsAffected > 0)
                {

                    this.Loguj("Fotka úspešne uložená do databázy!", MessageBoxIcon.Information);
                    //MessageBox.Show("Image saved sucessfully!","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                connection.Close();
            }

            catch (Exception ex)
           {
                //MessageBox.Show(ex.Message, "Chyba pri ukladani zaznamu do DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //throw new Exception("Chyba pri ukladani zaznamu do DB.\n\n" + ex.Message);
                this.Loguj("Fotka nebola uložená do databázy!\n", MessageBoxIcon.Error);
            }

       finally
       {
           if (connection.State == ConnectionState.Open)
           {
               connection.Close();
           }
       }
    }


    public DataSet FillStudentGrid(string Query, string Table)
    {
        if (connection.State == ConnectionState.Closed)
            connection.Open();
        MySqlCommand newCmd = connection.CreateCommand();
        newCmd.Connection = connection;
        newCmd.CommandType = CommandType.Text;
        newCmd.CommandText = Query;

        MySqlDataAdapter da = new MySqlDataAdapter(newCmd);
        DataSet ds = new DataSet();
        da.Fill(ds, Table);

        connection.Close();
        return ds;
    }




    //Delete statement
    public void Delete()
    {
        string query = "DELETE FROM table1";

        if (this.OpenConnection() == true)
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            this.CloseConnection();
        }
    }

    public void DeleteDate()
    {
        string query1 = "DELETE FROM table_fl WHERE date < DATE_SUB(NOW(), INTERVAL 30 DAY)";
        string query2 = "DELETE FROM table_fr WHERE date < DATE_SUB(NOW(), INTERVAL 30 DAY)";
        string query3 = "DELETE FROM table_rea WHERE date < DATE_SUB(NOW(), INTERVAL 30 DAY)";
        string query4 = "DELETE FROM table_rep WHERE date < DATE_SUB(NOW(), INTERVAL 30 DAY)";

        if (this.OpenConnection() == true)
        {
            MySqlCommand cmd = new MySqlCommand(query1, connection);
            cmd.ExecuteNonQuery();

            cmd = new MySqlCommand(query2, connection);
            cmd.ExecuteNonQuery();

            cmd = new MySqlCommand(query3, connection);
            cmd.ExecuteNonQuery();

            cmd = new MySqlCommand(query4, connection);
            cmd.ExecuteNonQuery();


            this.CloseConnection();
        }
    }

    public void Del()
    {
        string query1 = "DELETE FROM table3 WHERE date < DATE_SUB(NOW(), INTERVAL 30 DAY)";

        if (this.OpenConnection() == true)
        {
            MySqlCommand cmd = new MySqlCommand(query1, connection);
            cmd.ExecuteNonQuery();
            this.CloseConnection();
        }
    }

    //Select statement


    //Count statement
    public int Count()
    {
        string query = "SELECT Count(*) FROM table1";
        int Count = -1;

        //Open Connection
        if (this.OpenConnection() == true)
        {
            //Create Mysql Command
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //ExecuteScalar will return one value
            Count = int.Parse(cmd.ExecuteScalar() + "");

            //close Connection
            this.CloseConnection();

            return Count;
        }
         else
        {
            return Count;
        }
    }

    }
}
