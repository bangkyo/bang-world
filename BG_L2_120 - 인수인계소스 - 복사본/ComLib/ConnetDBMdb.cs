using System;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;


namespace ComLib
{
    class ConnectDBMdb
    {
       // Global session connection object	& variable
        private OleDbConnection con = new OleDbConnection();

		// Singleton constructer
		protected static ConnectDBMdb _Instance = null;
		protected ConnectDBMdb()	{}
		
		public static ConnectDBMdb GetInstance()
		{
			if (_Instance == null)
					_Instance = new ConnectDBMdb();

			return _Instance;
		}
 

		// Connect database (need defined database name)
		public bool Open(string db)
		{
            // Connection string
            string directory = "..\\DataBase\\";
            string connStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + directory + db;

            // Check current database connection state
            if (con.State == ConnectionState.Open)
                con.Close();

            // Connect database
            try
            {
                con.ConnectionString = connStr;
                con.Open();
            }
            catch (Exception e)
            {
                e.ToString();

                Close();

                System.Diagnostics.Process[] mProcess = System.Diagnostics.Process.GetProcessesByName(Application.ProductName);
                foreach (System.Diagnostics.Process p in mProcess)
                    p.Kill();
            }
            return true;
        }

		// Disconnect database
		public void Close()
		{
			if(con.State == ConnectionState.Open)
				con.Close();
		}


		//Change Database (Define DataBase Name)
		public bool dbChange(string db)
		{
			// Check current database connection state
			if(con.State == ConnectionState.Closed)
				Open(db);

			//Change Data base
            try
			{
				con.ChangeDatabase(db);
			}
			catch(Exception e)
			{
				MessageBox.Show("[ERR] : "+e.Message);
				return false;
			}

			return true;

		}//DBChange


		// Execute data read 
		//SqlDataReader를 사용하는 동안, 관련 SqlConnection은 SqlDataReader를 처리하고 닫는 것 외의 작업이 SqlConnection에서 수행되지 않습니다. 
		//이는 SqlDataReader의 Close 메서드가 호출될 때까지 해당합니다. 
        public OleDbDataReader ExecuteReader(string sql)
		{
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;

            OleDbCommand command = new OleDbCommand(sql, con);
            OleDbDataReader reader = null;

			try
			{
				reader = command.ExecuteReader();
			}
			catch(Exception ex)
			{
				MessageBox.Show("[ERR]"+ex.Message);
				
				if(con.State == ConnectionState.Open)
				{
					MessageBox.Show(con.State.ToString()+" : open");
				}
				else if(con.State == ConnectionState.Closed)
				{
					MessageBox.Show(con.State.ToString()+" : close");
				}
			}

			return reader;
		}


		// Execute non data query
		public bool ExecuteNonQuery(string sql)
		{
            OleDbCommand command = new OleDbCommand(sql, con);

			try
			{
				command.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
				return false;
			}

			return true;
		}


		// get dataset
		public DataSet ReturnDataSet(string sql)
		{
			DataSet dataSet = new DataSet();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, con);

			try
			{
				adapter.Fill(dataSet);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
			
			return dataSet;
		}

		// get dataset (use public int Fill(DataSet, string) )
		public DataSet ReturnDataSet(string sql, string table)
		{
			DataSet dataSet = new DataSet();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, con);

			try
			{
				adapter.Fill(dataSet, table);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
			
			return dataSet;
		}

		public DataTable ReturnDataTable(string _table, string _sql)
		{
			DataSet ds = new DataSet();
            string table_name = _table + ".mdb";
            GetInstance().Open(table_name);
            OleDbDataAdapter adapter = new OleDbDataAdapter(_sql, con);

			try
			{
				adapter.Fill(ds, _table);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
			
			return ds.Tables[_table];
		}

		public string GetDataSource()
		{
			return con.DataSource;
		}

    }
}