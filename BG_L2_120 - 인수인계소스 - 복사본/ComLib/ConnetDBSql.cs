using System;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;

using System.Collections;
using System.Collections.Generic;
using System.Data.OracleClient;


namespace ComLib
{

	/// <summary>
	/// This is a Finex ADO.NET database application
	/// We chose Microsoft SQL Server database (as reflected in the ADO.NET SQL objects)
	/// becase it's highly optimized with MS-SQL database
	/// and we chose class design pattern is singleton class
	/// </summary>
	public class ConnectDBSql
	{
        // Global session connection object	& variable
		private SqlConnection con = new SqlConnection();

		private int SQLPort = 1433;
		private string Server1 = "10.216.80.130";
        private bool IsDBAvailable = false;

        // Singleton constructer
        protected static ConnectDBSql _Instance = null;
		protected ConnectDBSql()	{}
		
		public static ConnectDBSql GetInstance()
		{
			if (_Instance == null)
					_Instance = new ConnectDBSql();

			return _Instance;
		}
 

		// Connect database (need defined database name)
		public bool Open(string db)
		{
			// Connection string
			string	connStr = "Server=10.216.80.130;uid=sa;pwd=level2;DataBase="+db;
			
			// Check current database connection state
			if(con.State == ConnectionState.Open)
				con.Close();

			// Connect database
			try
			{
				con.ConnectionString = connStr;
				con.Open();
			}
			catch(Exception e)
			{
				//MessageBox.Show("[ERR] : "+e.Message);
				//return false;
				e.ToString();

				Close();

				System.Diagnostics.Process[] mProcess = System.Diagnostics.Process.GetProcessesByName(Application.ProductName);
				foreach (System.Diagnostics.Process p in mProcess)
					p.Kill();
			}
			return true;
		}

        public bool CheckDbConnection()
        {
            // Connect database
            try
            {
                if (con.State != ConnectionState.Open)
                con.Open();
                IsDBAvailable = true;
                return IsDBAvailable;
            }
            catch (Exception e)
            {
                IsDBAvailable = true;
                return IsDBAvailable;
            }
        }

        public bool Open_mdb(string db)
        {
            // Connection string
            string connStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = D:\작업방\Boso3.0\DataBase\" + db;

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
                //MessageBox.Show("[ERR] : "+e.Message);
                //return false;
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
		public SqlDataReader ExecuteReader(string sql)
		{
			SqlCommand command = new SqlCommand(sql, con);
			SqlDataReader reader = null;

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

				SQLServerCheck();
			}

			return reader;
		}


		// Execute non data query
		public bool ExecuteNonQuery(string sql)
		{
			SqlCommand command = new SqlCommand(sql, con);

			try
			{
				command.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());

				SQLServerCheck();

				return false;
			}

			return true;
		}


		// get dataset
		public DataSet ReturnDataSet(string sql)
		{
			DataSet dataSet = new DataSet();
			SqlDataAdapter adapter = new SqlDataAdapter(sql, con);

			try
			{
				adapter.Fill(dataSet);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());

				SQLServerCheck();
			}
			
			return dataSet;
		}

		// get dataset (use public int Fill(DataSet, string) )
		public DataSet ReturnDataSet(string sql, string table)
		{
			DataSet dataSet = new DataSet();
			SqlDataAdapter adapter = new SqlDataAdapter(sql, con);

			try
			{
				adapter.Fill(dataSet, table);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());

				SQLServerCheck();
			}
			
			return dataSet;
		}

        //FindDataTable
        public DataTable ReturnDataTable(string _table, string _sql)
		{
			DataSet ds = new DataSet();

			SqlDataAdapter adapter = new SqlDataAdapter(_sql, con);

			try
			{
				adapter.Fill(ds, _table);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());

				SQLServerCheck();
			}
			
			return ds.Tables[_table];
		}

        //Get Current Data Base Connection Infomation
        public void GetState()
		{
			string  Msg = " [Connect String] : "+con.ConnectionString;
					Msg += "\n [Connection TimeOut] : "+con.ConnectionTimeout;
					Msg += "\n [Database] : "+con.Database;
					Msg += "\n [DataSource] : "+con.DataSource;
					Msg += "\n [PacketSize] : "+con.PacketSize;
					Msg += "\n [Server Version] : "+con.ServerVersion;
					Msg += "\n [State] : "+con.State;
					Msg += "\n [Work Station ID] : "+con.WorkstationId;

			MessageBox.Show(Msg);
		}



		public string GetDataSource()
		{
			return con.DataSource;
		}

		//SQL Server Active Check	//////////////////////////////////////////////////////
		public int SQLServerCheck()
		{
			Socket s = connectSocket(Server1, SQLPort);

//			if (s.Connected == true && s != null)
//			{
//				return 1;
//			}
//			else
//			{
//				MessageBox.Show("Server 연결 실패. 프로그램을 종료합니다.");
//				
//				Close();
//
//				System.Diagnostics.Process[] mProcess = System.Diagnostics.Process.GetProcessesByName(Application.ProductName);
//				foreach (System.Diagnostics.Process p in mProcess)
//					p.Kill();
//
//				return 0;
//			}
			return 0;
		}


		private static Socket connectSocket(string server, int port)
		{
			Socket s = null;
			IPHostEntry iphe = null;
    
			try
			{
				// Get host related information.
				iphe = Dns.Resolve(server);
    
    
				// Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
				// an exception to be thrown if the host IP Address is not compatible with the address family
				// (typical in the IPv6 case).
				foreach(IPAddress ipad in iphe.AddressList)
				{
					IPEndPoint ipe = new IPEndPoint(ipad, port);

					Socket tmpS = 
						new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					
					tmpS.Connect(ipe);

					if(tmpS.Connected)
					{
						s = tmpS;
						break;
					}
					else
						continue;
				}
			}
    
			catch(SocketException e) 
			{
				e.ToString();
				MessageBox.Show("DB Server 연결 실패. 프로그램을 종료합니다.");

				ConnectDBSql.GetInstance().Close();

				System.Diagnostics.Process[] mProcess = System.Diagnostics.Process.GetProcessesByName(Application.ProductName);
				foreach (System.Diagnostics.Process p in mProcess)
					p.Kill();


				//				MessageBox.Show("[SocketException] : " + e.Message);
				//				Console.WriteLine("SocketException caught!!!");
				//				Console.WriteLine("Source : " + e.Source);
				//				Console.WriteLine("Message : " + e.Message);
			}
			catch(Exception e)
			{
				e.ToString();
				MessageBox.Show("DB Server 연결 실패. 프로그램을 종료합니다.");

				ConnectDBSql.GetInstance().Close();

				System.Diagnostics.Process[] mProcess = System.Diagnostics.Process.GetProcessesByName(Application.ProductName);
				foreach (System.Diagnostics.Process p in mProcess)
					p.Kill();
				//				MessageBox.Show("[Exception] : " + e.Message);
				//				Console.WriteLine("Exception caught!!!");
				//				Console.WriteLine("Source : " + e.Source);
				//				Console.WriteLine("Message : " + e.Message);
			}
			return s;

		}


        public DataTable FindDataTable(string sql)
        {
            SqlConnection conn = null;
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            try
            {
                conn.Open();
                ocmd = new SqlCommand(sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");

                dt = dset.Tables[0];
            }
            catch (Exception ex)
            {
                //MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■sql■\n" + sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        //사용자 정의 
        public DataTable Find_CD(string category)
        {
            SqlConnection conn = null;
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string sql = string.Empty;

            try
            {
                sql = string.Format("SELECT CD_ID, CD_NM  FROM TB_CM_COM_CD WHERE CATEGORY = '{0}' AND USE_YN = 'Y'  ORDER BY SORT_SEQ, CD_ID", category);
                conn = new SqlConnection(sql);
                conn.Open();

                ocmd = new SqlCommand(sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];

                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■sql■\n" + sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        public DataTable Find_TC_CD(string category, string line_gp)
        {
            SqlConnection conn = null;
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string sql = string.Empty;

            try
            {
                string sql1 = string.Format("SELECT CD_ID, ");
                sql1 += string.Format("CD_NM    ");
                sql1 += string.Format("FROM TB_CM_COM_CD  ");
                sql1 += string.Format("WHERE CATEGORY = 'TC_CD' AND USE_YN = 'Y'");
                if (line_gp != "%")
                {
                    sql1 += string.Format("AND COLUMNA = '{0}' ", line_gp);
                }
                sql1 += string.Format("ORDER BY SORT_SEQ  ");

                conn = new SqlConnection(sql1);
                conn.Open();


                ocmd = new SqlCommand(sql1, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■sql■\n" + sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        public DataTable Find_CD_GOOD_NG(string category)
        {
            SqlConnection conn = null;
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string sql = string.Empty;

            try
            {
                sql = string.Format("SELECT CD_ID, CD_NM  FROM TB_CM_COM_CD WHERE CATEGORY = '{0}'  ORDER BY SORT_SEQ, CD_ID", category);
                conn = new SqlConnection(sql);
                conn.Open();

                ocmd = new SqlCommand(sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■sql■\n" + sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        public DataTable GetScreeAcl(string UserGrp)
        {
            DataTable dt = null;

            string sql1 = string.Empty;

            sql1 += string.Format("SELECT ");
            sql1 += string.Format("    B.PAGE_ID ");
            sql1 += string.Format("   ,NVL(A.INQ_ACL,'N') AS INQ_ACL ");
            sql1 += string.Format("   ,NVL(A.REG_ACL,'N') AS REG_ACL ");
            sql1 += string.Format("   ,NVL(A.MOD_ACL,'N') AS MOD_ACL ");
            sql1 += string.Format("   ,NVL(A.DEL_ACL,'N') AS DEL_ACL ");
            sql1 += string.Format("FROM TB_CM_ACL A, TB_CM_SCR B ");
            sql1 += string.Format("WHERE ");
            sql1 += string.Format("    A.ACL_GRP_ID = '{0}'", UserGrp);
            sql1 += string.Format("AND A.SCR_ID = B.SCR_ID ");
            sql1 += string.Format("AND B.USE_YN = 'Y' ");

            dt = FindDataTable(sql1);

            return dt;
        }

        public bool SetCombo(ComboBox cb, string categoryNm)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                List<string> list = new List<string>();
                //list.Add("전체");

                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    list.Add(dt.Rows[row].ItemArray[1].ToString());
                }
                foreach (var item in list)
                {
                    cb.Items.Add(item);
                }
                if (dt.Rows.Count > 0)
                {
                    //첫번째 아이템 선택
                    cb.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool SetCombo(ComboBox cb, string categoryNm, string ExtractItemNM, bool AddTotal)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ExtractItemNM))
                    {
                        if (row["CD_NM"].ToString() != ExtractItemNM)
                        {
                            arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        }
                    }
                    else
                    {
                        arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }


                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";

                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }

        public bool SetCombo(ComboBox cb, string categoryNm, string ExtractItemNM, bool AddTotal, string selected_id)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ExtractItemNM))
                    {
                        if (row["CD_NM"].ToString() != ExtractItemNM)
                        {
                            arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        }
                    }
                    else
                    {
                        arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }


                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == selected_id)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool SetCombo_3(ComboBox cb, string categoryNm, string ExtractItemNM, bool AddTotal, string selected_id)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ExtractItemNM))
                    {
                        if (row["CD_NM"].ToString() != ExtractItemNM)
                        {
                            arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        }
                    }
                    else
                    {
                        arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }


                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                //첫번째 아이템 선택
                cb.SelectedIndex = 2;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == selected_id)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //특정 항목들을 제거하기위한 설정
        public bool SetCombo(ComboBox cb, string categoryNm, List<string> Extractlist, bool AddTotal)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                bool exists = false;
                foreach (DataRow row in dt.Rows)
                {

                    //리스트에서 해당 항목의 이름이 같은 경우가 있는지 체크

                    //string result = Extractlist.Find(s => s.StartsWith(row["CD_NM"].ToString()));
                    exists = Extractlist.Exists(element => element == row["CD_NM"].ToString());

                    // 해당 항목이 제외할 아이템리스트에 없는경우  ==> 추가해야할경우
                    if (!exists)
                    {
                        arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }
                    // 해당 항목이  제외할 아이템리스트에 없는경우 ==> 추가해야하지 않아야할경우

                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }


        //교정, 면취의 정보만 가진 공정 cb
        public bool SetCombo(ComboBox cb, string categoryNm, string ExtractItemNM, bool AddTotal, List<string> list)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ExtractItemNM))
                    {
                        if (row["CD_NM"].ToString() != ExtractItemNM)
                        {
                            arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());


                        }
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            if (row["CD_NM"].ToString() == item)
                            {
                                arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());

                            }

                        }
                    }


                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }

        public bool SetCombo(ComboBox cb, string categoryNm, bool AddTotal, string selected_id)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);
                cb.DataSource = null;
                cb.Items.Clear();

                DataTable dt = Find_CD(categoryNm);


                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                //cb.Selecteditem = ck.StrKey2;

                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == selected_id)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public void SetComboYN(ComboBox cb)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);
                cb.DataSource = null;
                cb.Items.Clear();

                ArrayList arrType1 = new ArrayList();

                arrType1.Add(new DictionaryList("Y", "Y"));
                arrType1.Add(new DictionaryList("N", "N"));


                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.SelectedIndex = 1;

            }
            catch (Exception ex)
            {
                return;
            }

            return;
        }

        public bool SetCombo_TC_CD(ComboBox cb, string categoryNm, string line_gp, bool AddTotal, string selected_id)
        {
            try
            {
                // ComBoBox
                cb.DataSource = null;
                cb.Items.Clear();

                DataTable dt = Find_TC_CD(categoryNm, line_gp);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["CD_ID"].ToString() + "  " + row["CD_NM"].ToString(), row["CD_ID"].ToString()));
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                cb.DropDownStyle = ComboBoxStyle.DropDownList;

                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                //cb.Selecteditem = ck.StrKey2;

                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == selected_id)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public void InsertLogForSearch(string userid, Button btn)
        {
            string user_id = userid;
            string btn_gp = "1";
            string scr_id;

            string pageid = string.IsNullOrEmpty(btn.Parent.Parent.Parent.Name) ? btn.Parent.Parent.Name : btn.Parent.Parent.Parent.Name;

            if (string.IsNullOrEmpty(Find_Scr_ID_By_PAGE_ID(pageid)))
            {
                return;
            }
            else
            {
                scr_id = Find_Scr_ID_By_PAGE_ID(pageid);
            }

            try
            {
                ConnectDBSql.GetInstance().Open("MDMS");
                string sql1 = string.Empty;
                sql1 += string.Format(" INSERT INTO TB_CM_SCR_USEHIS ");
                sql1 += string.Format("      ( ");
                sql1 += string.Format("         USER_ID ");
                sql1 += string.Format("        ,SCR_ID ");
                sql1 += string.Format("        ,USE_DATE_SEQ ");
                sql1 += string.Format("        ,USE_DDTT ");
                sql1 += string.Format("        ,BTN_GP ");
                sql1 += string.Format("        ,REGISTER ");
                sql1 += string.Format("        ,REG_DDTT ");
                sql1 += string.Format("       ) ");
                sql1 += string.Format(" VALUES ( ");
                sql1 += string.Format("           '{0}' ", user_id);
                sql1 += string.Format("          ,'{0}' ", scr_id);
                sql1 += string.Format("          ,( ");
                sql1 += string.Format("             SELECT NVL(MAX(USE_DATE_SEQ),0) + 1 FROM  TB_CM_SCR_USEHIS ");
                sql1 += string.Format("             WHERE  USER_ID = '{0}' ", user_id);
                sql1 += string.Format("             AND    SCR_ID  = '{0}' ", scr_id);
                sql1 += string.Format("             AND    TO_CHAR(USE_DDTT,'YYYYMMDD') = TO_CHAR(SYSDATE,'YYYYMMDD') ");
                sql1 += string.Format("            ) ");
                sql1 += string.Format("          , sysdate ");
                sql1 += string.Format("          , '{0}' ", btn_gp);
                sql1 += string.Format("          , '{0}' ", user_id);
                sql1 += string.Format("          , sysdate ");
                sql1 += string.Format("         ) ");

                ConnectDBSql.GetInstance().ExecuteNonQuery(sql1);
                //실행후 성공
                ConnectDBSql.GetInstance().Close();

            }
            catch (System.Exception ex)
            {
            }
        }

        public string Find_Scr_ID_By_PAGE_ID(string page_id)
        {
            string rtn = "";

            string strQry = string.Empty;

            strQry += string.Format("select  ");
            strQry += string.Format("    ROWNUM AS L_NO");
            strQry += string.Format("   ,SCR_ID ");
            strQry += string.Format("FROM  ");
            strQry += string.Format("    TB_CM_SCR  ");
            strQry += string.Format("WHERE  ");
            strQry += string.Format("    PAGE_ID = '{0}' ", page_id);
            strQry += string.Format("AND ROWNUM = 1      ");

            DataTable olddt = FindDataTable(strQry);

            if (olddt.Rows.Count > 0)
            {
                rtn = olddt.Rows[0]["SCR_ID"].ToString();
            }

            return rtn;
        }

    }//class
 
}//namespace
